var pageName = $('#hf_pageName').val();
$(document).ready(function () {
    SelectMenu(4);
    $('body').on('click', '.manage', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var mode = $(this).data('create');
        ManageInformation(mode, id);
    });
    $('body').on('click', '#btnbackprev', function (e) {
        e.preventDefault();
        BackToPreviousPage();
    });

    function BackToPreviousPage() {
        window.history.back();
    }
    $('body').on('click', '.information', function (e) {
        e.preventDefault();
        var contentid = $(this).data('contentid');
        var mode = $(this).data('mode');
        ManageInformation(mode, contentid);
    });
    $('body').on('click', '#btn-delete', function (e) {
        e.preventDefault();
        var contentid = $(this).data('contentid');
        var name = $(this).data('name');
        Deletes(contentid, name);
    });
});

function ManageInformation(mode, id) {
    
    startAnimation();
    if ($('#pageName').val() == 'news story')
        window.location.href = "/Information/CreateNews" + EncodedQueryString("mode=" + mode + "&ContentId=" + id + "");
    else if ($('#pageName').val() == 'info story')
        window.location.href = "/Information/CreateInformation" + EncodedQueryString("mode=" + mode + "&ContentId=" + id + "");
    else if ($('#pageName').val() == 'download')
        window.location.href = "/Information/CreateDownload" + EncodedQueryString("mode=" + mode + "&ContentId=" + id + "");
    else if ($('#pageName').val() == 'link')
        window.location.href = "/Information/CreateLink" + EncodedQueryString("mode=" + mode + "&ContentId=" + id + "");
}

function Deletes(ContentId, ContentName) {
    let Msg = "Do you want to delete '" + ContentName + "'" + "' ?";
    ShowWarningPopup(Msg, 'DeleteDatas','', ContentId , ContentName);
}
function RedirectToMainPage() {
    CloseSuccessModalPopup();
    window.location.reload();
}


function DeleteDatas(deletedContentId, deletedContentName) {
    CloseWarningPopup();
    let params = '{"deletedContactId":"' + deletedContentId + '"}';
    $.ajax({
        type: "POST",
        url: '../Information/DeleteInformation',
        dataType: "json",
       /* contentType: "application/json; charset=utf-8",*/
        data: params,
        processdata: true,
        success: function (result) {
            console.log(result.Success);
            if (result.Success) {
                ShowSuccessModalPopup('"' + deletedContentName + '"  deleted successfully', 'RedirectToMainPage');
            }
            else {
                ShowErrorPopup('Error on the page');
            }
        },
        error: function (result) {
        }
    });
}

//sortType=0,3 - desc  //sortType=1,2 - asc
function InformationSort(event, param) {
    let sortTypeGlobal = 1;//1-asc
    let sortOrderGlobal = 3;//name
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    SearchList(true);
}

function SearchListOverview(isSort = false) {
    let url = '../Information/NewsOverview';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        data: {
            sortType: $('#SortTypeValue').val(), sortOrder: $('#SortOrderValue').val(),
            page: (isSort ? $('#pageNum').val() : 1), pageSize: $('#pageSizeVal').val()
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('section#banner').html('');
            $('section#banner').html($(response).find('section#banner'));
            $('html, body').animate({
                scrollTop: ($('section#banner').position().top)
            }, 1000);
        },
        error: function (result) {
            location.readload();

        },
        complete: function () {
            stopAnimation();
        }
    });
}
function SearchList(isSort = false) {
    let url = '../Information/ManageNews';
    if ($('#pageName').val() == 'news story')
        url = '../Information/ManageNews';
    else if ($('#pageName').val() == 'info story')
        url = '../Information/ManageHelpAndInformations';
    else if ($('#pageName').val() == 'download')
        url = '../Information/ManageDocuments';
    else if ($('#pageName').val() == 'link')
        url = '../Information/ManageLinks';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        data: {
            sortType: $('#SortTypeValue').val(), sortOrder: $('#SortOrderValue').val(),
            page: (isSort ? $('#pageNum').val() : 1), pageSize: $('#pageSizeVal').val()
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('section#banner').html('');
            $('section#banner').html($(response).find('section#banner'));
        },
        error: function (result) {
            location.readload();

        },
        complete: function () {
            stopAnimation();
        }
    });
}

$('#newspaginator').on('click', 'a', function (e) {
    e.preventDefault();
    let page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    SearchList(true);//using sorting as true to avoid page reset
});
$('body').on('change', '.newspaginator #pageSizeSelect', function () {
   
    $('#pageNum').val();
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SearchList();
});
$('body').on('change', '#pageSizeSelectoverview', function () {

    $('#pageNum').val();
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SearchListOverview();
});

$('body').on('click', '.news-overview-pagination a', function (e) {
    e.preventDefault();
    let page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    SearchListOverview(true);
});