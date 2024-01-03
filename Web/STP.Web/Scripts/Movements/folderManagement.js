$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    folderMgmt.loadFolderInit();

    //add > open popup
    $('body').on('click', '.add-new', function (e) {
        e.preventDefault();
        var folderId = $(this).data('id');
        $('#hfFolderId').val(folderId);
        $('#hfIsEdit').val('0');
        $('#AddFolder #folderName').val('');
        $('#AddFolder').modal('show');
    });

    //edit > open popup
    $('body').on('click', '.edit-foldername', function (e) {
        e.preventDefault();
        var folderId = $(this).data('id');
        var folderName = $(this).data('name');
        $('#hfFolderId').val(folderId);
        $('#hfIsEdit').val('1');
        $('#AddFolder #folderName').val(folderName);
        $('#AddFolder').modal('show');
    });

    //delete item
    $('body').on('click', '.delete-foldername', function (e) {
        e.preventDefault();
        var folderId = $(this).data('id');
        ShowWarningPopupDelegate("Are you sure want to delete this folder?", function () {
            $('#WarningPopup').modal('hide');
            folderMgmt.deleteFolder(folderId);
        });       
    });

    // Add/Edit item click in modal popup
    $('#AddFolder').on('click', '#btnAddFolder', function (e) {
        e.preventDefault();
        var isEdit = $('#AddFolder #hfIsEdit').val()=='1';
        var folderId = $('#AddFolder #hfFolderId').val();
        var folderName = $('#AddFolder #folderName').val();
        if (folderName == '') {
            $('#err_folderName').html('Folder name is required!');
            setTimeout(function () {
                $('#err_folderName').html('');
            }, 3000);
            return false;
        }
        if (!isEdit) 
            folderMgmt.addFolder(folderId, folderName);
        else
            folderMgmt.editFolder(folderId, folderName);
    });

    $(document).on('click', '#btnRemoveFromFolder', function (e) {
        e.preventDefault();
        var selectedTrCount = $('tr.selected-tr[draggable="true"] .folder-drag-elem:checked').length;
        var folderIdSelected = $('#FolderID').val();
        if (folderIdSelected == 0 || folderIdSelected == '0') {
            showToastMessage({message: "Please select a folder first!",type: "warning"});
            return;
        }
        if (selectedTrCount < 1) {
            showToastMessage({message: "No Movements Selected",type: "warning"});
            return;
        }

        var selectedTrCount = $('tr.selected-tr[draggable="true"] .folder-drag-elem:checked').length;
        var msg = selectedTrCount == 1 ? "Are you sure you want to remove the selected item?" : "Are you sure you want to remove the selected items?";
        var isApiCalled = false;
        ShowWarningPopupDelegate(msg, function (ev) {
            ev.preventDefault();
            if (!isApiCalled) {
                isApiCalled = true;
                $('#WarningPopup').modal('hide');
                var objArray = [];
                var selectedTrs = $('tr.selected-tr[draggable="true"] .folder-drag-elem:checked');
                var folderIdSelected = $('#FolderID').val();
                selectedTrs.each(function (index, chk) {
                    var currentTr = $(chk).closest('tr.selected-tr[draggable="true"]');
                    var revisionid = $(currentTr).data('revisionid');
                    var versionid = $(currentTr).data('versionid');
                    var notificationid = $(currentTr).data('notificationid');
                    var movementtype = $(currentTr).data('movementtype');
                    var projectid = $(currentTr).data('projectid');
                    var obj = {
                        FolderId: folderIdSelected, MovementRevisionId: revisionid, MovementVersionId: versionid,
                        NotificationId: notificationid, MovementType: movementtype, ProjectId: projectid
                    };
                    objArray.push(obj);
                    //console.log('obj', obj);
                }).promise().done(function () {
                    //console.log('objArray', objArray);
                    folderMgmt.unTagFolder(objArray);
                });
            }
        });

    });

    //untag button click event
    $('body').on('click', '.untag', function (e) {
        e.preventDefault();
        var selectedTrCount = $('tr.selected-tr[draggable="true"] .folder-drag-elem:checked').length;
        var msg = selectedTrCount == 1 ? "Are you sure want to untag selected folder?" : "Are you sure want to untag selected folders?";
        var isApiCalled = false;
        ShowWarningPopupDelegate(msg, function (ev) {
            ev.preventDefault();
            if (!isApiCalled) {
                isApiCalled = true;
                $('#WarningPopup').modal('hide');
                var objArray = [];
                var selectedTrs = $('tr.selected-tr[draggable="true"] .folder-drag-elem:checked');
                var folderIdSelected = $('#FolderID').val();
                selectedTrs.each(function (index, chk) {
                    var currentTr = $(chk).closest('tr.selected-tr[draggable="true"]');
                    var revisionid = $(currentTr).data('revisionid');
                    var versionid = $(currentTr).data('versionid');
                    var notificationid = $(currentTr).data('notificationid');
                    var movementtype = $(currentTr).data('movementtype');
                    var projectid = $(currentTr).data('projectid');
                    var obj = {
                        FolderId: folderIdSelected, MovementRevisionId: revisionid, MovementVersionId: versionid,
                        NotificationId: notificationid, MovementType: movementtype, ProjectId: projectid
                    };
                    objArray.push(obj);
                    //console.log('obj', obj);
                }).promise().done(function () {
                    //console.log('objArray', objArray);
                    folderMgmt.unTagFolder(objArray);
                });
            }
        });
    });

    //close bottom left message div
    $('#lowerleft .close-message').click(function () {
        $('#lowerleft').hide();
        $('#lowerleft .message').html('');
    });


    //===== On left side panel folder click - Load list
    $('body').on('click', 'li.folder-item a.clickable span,li.folder-item a.clickable.a-last-item', function (e) {
        e.preventDefault();
        $('.untag').hide();
        $('#folderInfoMessage').hide();
        var _this = this;
        folderMgmt.clearFilters(function () {
            var folderId = $(_this).closest('li.folder-item').data('id');
            if (folderId == "0" || folderId == "" || folderId == undefined) {
                $('#FolderID').val('');
                folderMgmt.reloadList();
            } else {
                $('#FolderID').val(folderId);
                folderMgmt.reloadList();
            }
            folderMgmt.setActiveClass();
        });
    });

    var menu_btn = document.querySelector("#menu-btn");
    var sidebar = document.querySelector("#folder-nav");
    menu_btn.addEventListener("click", () => {
        sidebar.classList.toggle("active-folder-nav");
        var isOpened = $('.active-folder-nav').length > 0;
        folderMgmt.updateFolderIconOpenStatus(isOpened);
    });    

    //checkbox check event
    $(document).on('change', 'tr[draggable="true"] .folder-drag-elem', function () {
        var selectedTrCount = $('tr.selected-tr[draggable="true"] .folder-drag-elem:checked').length;
        if (this.checked) {
            $('tr[draggable="true"]').addClass('selected-tr');
            //folderMgmt.showUnTagButton();            
        } else {
            var selectedTrCount = $('tr.selected-tr[draggable="true"] .folder-drag-elem:checked').length;
            if (selectedTrCount < 1) {
                $('tr.selected-tr[draggable="true"]').removeClass('selected-tr');
            }
        }
        folderMgmt.changeTitleBasedOnCheckboxSelection();
    });

    //Select all checkbox event
    $(document).on('change', '.folder-drag-elem-select-all', function () {
        $('tr[draggable="true"]').addClass('selected-tr');
        if (this.checked) {
            $('tr.selected-tr[draggable="true"] .folder-drag-elem').prop('checked', true);
            //folderMgmt.showUnTagButton();
        } else {
            $('tr.selected-tr[draggable="true"] .folder-drag-elem').prop('checked', false);
            $('tr.selected-tr[draggable="true"]').removeClass('selected-tr');
            $('.untag').hide();
        }
        folderMgmt.changeTitleBasedOnCheckboxSelection();
    });

});

var folderMgmt = {
    loadFolderInit: function () {
        $.ajax({
            url: '../Movements/GetFolders',
            data: {},
            type: 'GET',
            beforeSend: function () {
                //startAnimation();
            },
            success: function (data) {
                if (data && data.length) {
                    folderMgmt.bindFolderHtmlOnAjaxCall(data);
                }
            },
            error: function () {
            },
            complete: function () {
                //stopAnimation();
            }
        });
    },
    bindFolderHtmlOnAjaxCall: function (data) {
        var html = "";
        for (var i = 0; i < data.length; i++) {
            var itemFirst = data[i];
            html = folderMgmt.generateFolderHtml(html, itemFirst, 0, i);//parent-tree
            if (itemFirst.Children && itemFirst.Children.length > 0) {
                html += '<ul class="child-tree">';
                for (var j = 0; j < itemFirst.Children.length; j++) {
                    var itemSecond = itemFirst.Children[j];
                    html = folderMgmt.generateFolderHtml(html, itemSecond, 1, j); //child-tree
                    if (itemSecond.Children && itemSecond.Children.length > 0) {
                        html += '<ul class="sub-child-tree">';
                        for (var k = 0; k < itemSecond.Children.length; k++) {
                            var itemThird = itemSecond.Children[k];
                            html = folderMgmt.generateFolderHtml(html, itemThird, 2, k, true);
                            html += '</li>';
                        }
                        html += '</ul>';//sub-child-tree
                    }
                    html += '</li>';
                }
                html += '</ul>';//child-tree
            }
            html += '</li>';
        }
        $('.parent-tree-list:not(.default-li)').remove();
        $('ul.tree').append(html);
        folderMgmt.setActiveClass();
    },
    generateFolderHtml: function (html, item, index, i, isAddNotRequired) {
        var addHtml = '<li><a class="dropdown-item add-new" href="#" data-id="' + item.FolderId + '">' +
            '<i class="fa fa-folder"></i> New Folder</a></li>';
        var lastElementClass = "";
        if (isAddNotRequired == true) {
            addHtml = "";
            lastElementClass = "a-last-item";
        }
        html += '<li  draggable="true"  ' +
            ' id="limenu' + index + '' + i + '" class="folder-item parent-tree-list" data-id="' + item.FolderId + '" data-name="' + item.FolderName + '">' +
            '<label  for="menu' + index + '' + i + '"><div class="folder-drop dropend">' +
            '<a class="icon" href="#" data-bs-toggle="dropdown" aria-label="Menu"><i class="fa fa-ellipsis-h" aria-hidden="true"></i></a>' +
            '<ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow">' +
            '' + addHtml + '' +
            '<li><a class="dropdown-item edit-foldername" href="#" data-id="' + item.FolderId + '" data-name="' + item.FolderName + '">' +
            '<i class="fa fa-pencil"></i> Rename</a></li>' +
            '<li><a class="dropdown-item delete-foldername" href="#" data-id="' + item.FolderId + '" data-name="' + item.FolderName + '">' +
            '<i class="fa fa-trash"></i> Delete</a></li>' +
            '</ul>' +
            '</div>' +
            '<a class="clickable ' + lastElementClass+'"><span>' + item.FolderName + '<span></a>' +
            '</label>' +
            '<input class="nav-checkbox" id="menu' + index + '' + i + '" value="" type="checkbox">'; //parent-tree
        return html;
    },
    reloadList: function () {
        if (typeof SearchHaulierList != 'undefined')
            SearchHaulierList();
        if (typeof SearchSOAList != 'undefined')
            SearchSOAList();
        if (typeof FilterSuccessdata != 'undefined')
            FilterSuccessdata();
    },
    clearFilters: function (callBackFn) {
        if ($('#hf_SORTMovement').length > 0) {
            ClearSORTData(1, false);
            if (callBackFn != null && callBackFn != undefined) {
                callBackFn();
            }
        }
        else if ($('#hf_HaulierMovement').length>0) {
            ClearHaulierAdvancedData(false);
            if (callBackFn != null && callBackFn != undefined) {
                callBackFn();
            }
        }
        else if ($('#hf_SOAPoliceMovement').length > 0) {
            ClearAdvanced(1, resetData = 0);
            if (callBackFn != null && callBackFn != undefined) {
                callBackFn();
            }
        }
    },
    setActiveClass: function () {
        var folderIdSelected = $('#FolderID').val();
        $("#folder-nav ul.tree").find("li label.active").removeClass('active');//reset active class
        if (folderIdSelected == '0' || folderIdSelected == '' || folderIdSelected == undefined) {
            var mainLi = $("#folder-nav ul.tree").find("li[data-id='0'] label:first");
            if (mainLi != undefined)
                $(mainLi).addClass('active');
        } else {
            var selectedFolderItem = $("#folder-nav ul.tree").find("li[data-id='" + folderIdSelected+"'] label:first");
            if (selectedFolderItem != undefined) {
                $(selectedFolderItem).addClass('active');

                //Make selected folder's parent open
                var selectedFolderLiParents = $("#folder-nav ul.tree").find("li[data-id='" + folderIdSelected + "']").parents('li.folder-item');
                selectedFolderLiParents.each(function (index, li) {
                    var checkBox = $(li).find('.nav-checkbox:first');
                    $(checkBox).prop('checked',true);
                    
                }).promise().done(function () {
                });
            }
        }
    },
    showUnTagButton: function () {
        var folderIdSelected = $('#FolderID').val();
        if (folderIdSelected == '0' || folderIdSelected == '' || folderIdSelected == undefined) {
            $('.untag').hide();
        } else {
            $('.untag').show();
        }
    },
    addFolder: function (parentId, folderName) {
        $.ajax({
            url: '../Movements/SaveNewFolder',
            data: { ParentId: parentId, FolderName: folderName },
            type: 'POST',
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                if (data && data != 0 && data != 2) {
                    folderMgmt.showSuccessMessage('The folder ' + folderName+' has been successfully added');
                    folderMgmt.loadFolderInit();
                    $('#AddFolder').modal('hide');
                } else if (data == 2) {
                    $('#err_folderName').html('The folder already exist.');
                    setTimeout(function () {
                        $('#err_folderName').html('');
                    }, 2000);
                    return false;
                } else if (data == 0) {
                    $('#err_folderName').html('Something went wrong.');
                    setTimeout(function () {
                        $('#err_folderName').html('');
                    }, 2000);
                    return false;
                }
            },
            error: function () {
            },
            complete: function () {
                stopAnimation();
            }
        });
    },
    editFolder: function (folderId, folderName) {
        $.ajax({
            url: '../Movements/UpdateFolderName',
            data: { FolderId: folderId, FolderName: folderName },
            type: 'POST',
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                if (data && data != 0 && data != 2) {
                    folderMgmt.showSuccessMessage('The folder has been successfully renamed to '+folderName+'');
                    folderMgmt.loadFolderInit();
                    $('#AddFolder').modal('hide');
                } else if (data == 2) {
                    $('#err_folderName').html('The folder already exist.');
                    setTimeout(function () {
                        $('#err_folderName').html('');
                    }, 2000);
                    return false;
                } else if (data == 0) {
                    $('#err_folderName').html('Something went wrong.');
                    setTimeout(function () {
                        $('#err_folderName').html('');
                    }, 2000);
                    return false;
                }
            },
            error: function () {
            },
            complete: function () {
                stopAnimation();
            }
        });
    },
    deleteFolder: function (folderId) {
        $.ajax({
            url: '../Movements/DeleteFolder',
            data: { FolderId: folderId },
            type: 'POST',
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                if (data && data.result != 0) {
                    folderMgmt.showSuccessMessage('The folder has been successfully deleted');
                    $('#FolderID').val('');
                    folderMgmt.bindFolderHtmlOnAjaxCall(data.folders);
                    folderMgmt.reloadList();
                } else if (data == 2) {
                    folderMgmt.showSuccessMessage('The folder not deleted',false);
                    return false;
                }
            },
            error: function () {
            },
            complete: function () {
                stopAnimation();
            }
        });
    },

    dragAndDropItemToFolder: function (ev,li) {
        if (li != undefined) {
            $(li).find('label:first').addClass("hover-selected");
            setTimeout(function () {
                $(li).find('label:first').removeClass("hover-selected");
            }, 1000);

            //POST data to server
            var folderId = $(li).data('id');
            var folderName = $(li).data('name');
            if (folderId == "0" || folderId == "" || folderId == undefined) {
                folderMgmt.showSuccessMessage('Invalid attempt.', false);
                return false;
            }
            var objArray = [];
            var selectedTrCount = $('tr.selected-tr[draggable="true"] .folder-drag-elem:checked').length;
            if (selectedTrCount > 0) {
                // multi select using checkbox and drag -- 
                var selectedTrs = $('tr.selected-tr[draggable="true"] .folder-drag-elem:checked');
                selectedTrs.each(function (index, chk) {
                    var currentTr = $(chk).closest('tr.selected-tr[draggable="true"]');
                    var revisionid = $(currentTr).data('revisionid');
                    var versionid = $(currentTr).data('versionid');
                    var notificationid = $(currentTr).data('notificationid');
                    var movementtype = $(currentTr).data('movementtype');
                    var projectid = $(currentTr).data('projectid');
                    var obj = {
                        FolderId: folderId, MovementRevisionId: revisionid, MovementVersionId: versionid,
                        NotificationId: notificationid, MovementType: movementtype, ProjectId: projectid
                    };
                    objArray.push(obj);
                    //console.log('obj', obj);
                }).promise().done(function () {
                    //console.log('objArray', objArray);
                    folderMgmt.insertItemsToFolderOnDragAndDrop(objArray, folderName);
                });
            } else {
                //Normal drag and drop
                var movementRevisionId = ev.originalEvent.dataTransfer.getData("revisionid");
                var movementVersionId = ev.originalEvent.dataTransfer.getData("versionid");
                var notificationId = ev.originalEvent.dataTransfer.getData("notificationid");
                var movementType = ev.originalEvent.dataTransfer.getData("movementtype");
                var projectId = ev.originalEvent.dataTransfer.getData("projectid");
                var obj = {
                    FolderId: folderId, MovementRevisionId: movementRevisionId, MovementVersionId: movementVersionId,
                    NotificationId: notificationId, MovementType: movementType, ProjectId: projectId
                };
                objArray.push(obj);
                folderMgmt.insertItemsToFolderOnDragAndDrop(objArray, folderName);
            }



        }
    },
    insertItemsToFolderOnDragAndDrop: function (itemObj, folderName) {
        $.ajax({
            url: '../Movements/AddItemToFolder',
            data: JSON.stringify({ 'model': itemObj }),
            //contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                if (data && data != 0 && data != 2) { //2 - duplicate   0 - fail
                    var message = "Item added to " + folderName + "";
                    if (itemObj.length > 1)
                        message = "Items added to " + folderName + "";
                    folderMgmt.showSuccessMessage(message);
                } else if (data == 2) {
                    var message = "Item already added to " + folderName + "";
                    if (itemObj.length > 1)
                        message = "Items already added to " + folderName + "";
                    folderMgmt.showSuccessMessage(message, false);
                    return false;
                } else if (data == 0) {
                    folderMgmt.showSuccessMessage('Something went wrong.', false);
                    return false;
                }
            },
            error: function () {
            },
            complete: function () {
                stopAnimation();
            }
        });
    },
    dragAndDropFolderToFolder: function (ev, li, folderIdPassed) {
        if (li != undefined) {
            $(li).find('label:first').addClass("hover-selected");
            setTimeout(function () {
                $(li).find('label:first').removeClass("hover-selected");
            }, 1000);

            var parentFolderId = $(li).data('id');
            var folderName = $(li).data('name');

            if (parseInt(parentFolderId) == parseInt(folderIdPassed)) {//if user drag folder to same folder
                folderMgmt.showSuccessMessage("Cannot move to same folder!", false);
                return false;
            }

            var obj = {
               ParentFolderId: parentFolderId, FolderId: folderIdPassed
            };

            $.ajax({
                url: '../Movements/MoveFolderToFolder',
                data: JSON.stringify({ 'model': obj }),
                //contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                beforeSend: function () {
                    startAnimation();
                },
                success: function (data) {
                    if (data && data != 0 && data != 2) { //2 - duplicate   0 - fail
                        folderMgmt.showSuccessMessage("Movement added to " + folderName + "");
                        folderMgmt.loadFolderInit();
                        //folderMgmt.reloadList();
                    } else if (data == 2) {
                        folderMgmt.showSuccessMessage("Movement already added to " + folderName + "", false);
                        return false;
                    } else if (data == 0) {
                        folderMgmt.showSuccessMessage('Something went wrong.', false);
                        return false;
                    }
                },
                error: function () {
                },
                complete: function () {
                    stopAnimation();
                }
            });
        }
    },

    showSuccessMessage: function (message, isSuccess = true) {
        showToastMessage({ message: message, type: isSuccess?"success":"error" });
    },
    unTagFolder: function (unTagObj) {
        $.ajax({
            url: '../Movements/RemoveItemsFromFolder',
            data: JSON.stringify({ 'model': unTagObj }),
            //contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                if (data && data != 0 && data != 2) {
                    stopAnimation();
                    var folderName = folderMgmt.getCurrentFolderName();
                    var message = "The selected item has been removed from " + folderName+"";
                    if (unTagObj.length > 1)
                        message = "The selected items have been removed from " + folderName +"";
                    folderMgmt.showSuccessMessage(message);
                    $('.untag').hide();
                    setTimeout(function () {
                        folderMgmt.reloadList();
                    }, 100);
                } else if (data == 2) {
                    folderMgmt.showSuccessMessage('Unable to remove movement.',false);
                    return false;
                } else if (data == 0) {
                    folderMgmt.showSuccessMessage('Something went wrong.', false);
                    return false;
                }
            },
            error: function () {
            },
            complete: function () {
                stopAnimation();
            }
        });
    },
    showHelpMessageOnCheckboxCheck: function () {
        $('#lowerleft').hide();
        var folderIdSelected = $('#FolderID').val();
        var selectedTrCount = $('tr.selected-tr[draggable="true"] .folder-drag-elem:checked').length;
        if (selectedTrCount > 0) {
            var message = 'Add selected items to the folder';
            if (folderIdSelected != "0" && folderIdSelected != 0) //Not "All" or root folder
                message = 'Remove selected items from the folder';// show remove selected items to folder message
            $('#folderInfoMessage .message').html(message);
            $('#folderInfoMessage').fadeIn();
        } else {
            $('#folderInfoMessage').fadeOut();
        }
    },

    changeTitleBasedOnCheckboxSelection: function () {
        var selectedTrCount = $('tr.selected-tr[draggable="true"] .folder-drag-elem:checked').length;
        if (selectedTrCount > 1) {
            $('#folder_Help_Menu').text('Left click a movement and drag to a folder to add all selected items');
            $('.folder-info').attr('data-bs-original-title', 'Left click a movement and drag to a folder to add all selected items');
        } else {
            $('#folder_Help_Menu').text('Left click a movement and drag to a folder to add it');
            $('.folder-info').attr('data-bs-original-title', 'Left click a movement and drag to a folder to add it');
        }
    },

    getCurrentFolderName: function () {
        //folder-item
        var currentFolderName = "";
        var currentFolder = $('.folder-item .active').parent('.folder-item');
        if (currentFolder != undefined) {
            currentFolderName = currentFolder.data('name');
        }
        return currentFolderName;
    },

    updateFolderIconOpenStatus: function (isOpened) {
        $.ajax({
            url: '../Movements/UpdateFolderIconStatus',
            data: { isOpened: isOpened },
            type: 'POST',
            beforeSend: function () {
            },
            success: function (data) {
                
            },
            error: function () {
            },
            complete: function () {
            }
        });
    },
};

//============================================
//==========start of drag and drop functionalities
//============================================
function allowDropFolderManagement(ev) {
    ev.preventDefault();
    //console.log('allowDrop', $(ev.target));
    
    var li = $(ev.target).closest('li')[0];
    if (li != undefined) {
        $('.drop-active').removeClass('drop-active');
        $(li).find('label:first').addClass("drop-active");
    }
}

function dragStartFolderManagement(ev, movementRevisionId, movementVersionId, notificationId, movementType, projectId) {
    //console.log('dragStart', $(ev.target));
    ev.originalEvent.dataTransfer.setData("revisionid", movementRevisionId);
    ev.originalEvent.dataTransfer.setData("versionid", movementVersionId);
    ev.originalEvent.dataTransfer.setData("notificationid", notificationId);
    ev.originalEvent.dataTransfer.setData("movementtype", movementType);
    ev.originalEvent.dataTransfer.setData("projectid", projectId);
}

function dragStartFolder(ev, folderId) {
    //console.log('dragStart', $(ev.target));
    ev.stopPropagation();
    //console.log('dragStartFolder folderId############', folderId);
    ev.originalEvent.dataTransfer.setData("folderid", folderId);
}
function dragEndFolderManagement(ev) {
    //console.log('dragEnd', $(ev.target));
    $('.drop-active').removeClass('drop-active');
}
function dragLeaveFolderManagement(ev) {
    //console.log('dragLeave', $(ev.target));
}
function dropFolderManagement(ev) {
    ev.preventDefault();
    ev.stopImmediatePropagation();
    ev.stopPropagation();    
    var li = $(ev.target).closest('li')[0];
    var folderIdPassed = ev.originalEvent.dataTransfer.getData("folderid");
    //console.log('folderIdPassed>>>>>>>>>>', folderIdPassed);
    if (folderIdPassed == "" || folderIdPassed == undefined) {//On Item drag to folder
        folderMgmt.dragAndDropItemToFolder(ev,li);
    } else {//On folder move > drag one folder to another
        folderMgmt.dragAndDropFolderToFolder(ev, li, folderIdPassed);
    }
    //console.log('li-------', li);
    return false;
}

// ondragover="noAllowDrop(event)"
function noAllowDrop(ev) {
    ev.stopPropagation();
}
$('body').on('drop', '#parentfolder_ul', function (e) {
    dropFolderManagement(e);

});
$('body').on('dragover', '#parentfolder_ul', function (e) {
    allowDropFolderManagement(e);
});
$('body').on('dragleave', '#parentfolder_ul', function (e) {
    dragLeaveFolderManagement(e);
});
$('body').on('dragstart', '.folder-item', function (e) {
    var id = $(this).data("id");
    dragStartFolder(e,id);
});
$('body').on('dragend', '.folder-item', function (e) {
    dragEndFolderManagement(e);
});

$('body').on('dragstart', '.table_folder_item', function (e) {
    var movementRevisionId = $(this).data("revisionid");
    var movementVersionId = $(this).data("versionid");
    var notificationId = $(this).data("notificationid");
    var movementType = $(this).data("movementtype");
    var projectId = $(this).data("projectid");
    dragStartFolderManagement(e, movementRevisionId, movementVersionId, notificationId, movementType, projectId);
});
$('body').on('dragend', '.table_folder_item', function (e) {
    dragEndFolderManagement(e);
});

