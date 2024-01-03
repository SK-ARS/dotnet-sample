var PageNumber;
var StatusCode;
var SORTCollab;
var NotifCollab;
var pageSizeTemp;
var pageNum;
var randomNumber;
function CollaborationStatusInit() {
    fillPageSizeSelect();
    if (localStorage['page'] == document.Url)
        $(document).scrollTop(localStorage['scrollTop']);
    $("#General").css("display", "block");
    PageNumber = $('#hf_PageNumber').val();
    StatusCode = $('#hf_StatusCode').val();
    SORTCollab = $('#hf_SORTCollab').val();
    NotifCollab = $('#hf_NotifCollab').val();
    pageSizeTemp = $('#hf_pageSize').val();
    pageNum = PageNumber;
    randomNumber = Math.random();
    CreteInternalCollaborationInit();
}
$(document).ready(function () {
    $('body').on('click', '.ViewHistory', function (e) {
        e.preventDefault();
        ViewHistory(this);
    });
    $('body').on('click', '.SetInternalCollaboration', function (e) {
        e.preventDefault();
        SetInternalCollaboration(this);
    });
    $('body').on('click', '.ViewSORTHistoryFn', function (e) {
        e.preventDefault();
        ViewSORTHistoryFn(this);
    });
    $('body').on('click', '.ViewSORTHistory', function (e) {
        e.preventDefault();
        ViewSORTHistoryFn(this);
    });
    $('body').on('click', '.SetSORTInternalCollaboration', function (e) {
        e.preventDefault();
        SetSORTInternalCollaborationFn(this);
    });
    $('#reviewCausionPaginator').on('click', 'a', function (e) {
        if (this.href == '') {
            return false;
        }
        else {
            $.ajax({
                url: this.href,
                type: 'GET',
                cache: false,
                success: function (result) {
                    $('div#pageheader').html('');
                    $('#General').html(result);
                }
            });
            return false;
        }
    });
});
$('body').on('click','#reviewCausionPaginator a', function (e) {
        e.preventDefault();
        var url = this.href;
        var page = this.innerText;
        if (url == '') {
            return false;
        }
        else {
            $.ajax({
                url: url,
                type: 'GET',
                cache: false,
                data: { page:page },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (result) {
                   $('div#pageheader').html('');
                    $('#General').html(result);
                    stopAnimation();
                },
            });

        }
        
    });
   


function ViewSORTHistoryFn(_this) {
    var DocumentId = $(_this).attr("documentid");
    ViewSORTHistory(DocumentId);
}
function SetSORTInternalCollaborationFn(_this) {
    var DocumentId = $(_this).attr("documentid");
    var CollaborationId = $(_this).attr("collaborationid");
    SetSORTInternalCollaboration(DocumentId, CollaborationId)
}
function ViewHistory(_this) {
    var DocumentId = $(_this).attr("documentid");
    randomNumber = Math.random();
    var link = "../Notification/DisplayCollaborationHistoryList" + EncodedQueryString("DocumentId=" + DocumentId + "&randomNumber" + randomNumber);
    window.location.href = link;
}
function SetInternalCollaboration(_this) {
    var DocumentID = $(_this).attr("documentid");
    var CollaborationNo = $(_this).attr("collaborationid");
    randomNumber = Math.random();
    var link = "../Notification/StoreInternalCollaboration";
    var paramList = {
        DOCUMENT_ID: DocumentID,
        COLLABORATION_NO: CollaborationNo
    }
    $.ajax({
        async: false,
        type: "POST",
        url: link,
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: JSON.stringify(paramList),
        processdata: true,
        success: function (result) {
            if (result) {
                paramList = null;
                $("#overlay").show();
                removescroll();
                $("#dialogue").load('../Notification/CollabHistoryPopList?SORTSetCollab=' + true, function () {
                    $('#CollabHistoryDiv').modal({ keyboard: false, backdrop: 'static' });

                    $('#CollabHistoryDiv').modal('show')
                    $("#dialogue").css("display", "block");

                    stopAnimation();
                    $("#overlay").css("display", "block");
                });
            }
            else {
                alert('Error');
            }
        },
        error: function () {
        },
        complete: function () {
            $('.loading').hide();
        }
    });
}
function changePageSize(_this) {

    var pageSize = $(_this).val();

    var sURL = "../Notification/CollaborationStatusList";
    var pv = $('#hf_PageView').val();

    var esdalRefNo = StatusCode;
    var sortCollab = SORTCollab;
    var notifCollab = NotifCollab;

    $.ajax({
        url: sURL,
        type: 'GET',
        cache: false,
        async: false,
        data: { pageSize: pageSize, RefNo: esdalRefNo, SORTCollab: sortCollab, NotifCollab: notifCollab },
        beforeSend: function () {
            $("#overlay").show();
            $('.loading').show();
        },
        success: function (result) {
            if ($('#hf_SORTCollab').val() == 'False') {
                $('#div_coll_list').html($(result).find('#div_coll_list').html());
            } else {
                $('#tab_6').html($(result));
                removeHLinksTrans();
                PaginateGridTrans();
                fillPageSizeSelect();
            }
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            $("#overlay").hide();
            $('.loading').hide();
        }
    });
}
function fillPageSizeSelect() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageNum').val(pageNum);
    $('#pageSizeSelect').val(selectedVal);
}