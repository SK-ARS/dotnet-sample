    var pageSizeTemp  = $('#hf_pageSize').val(); 
    var pageNum = "@Model.PageNumber";
    $(document).ready(function () {

        fillPageSizeSelect();

        if (localStorage['page'] == document.Url)
            $(document).scrollTop(localStorage['scrollTop']);
        $("#General").css("display", "block");

        $(".ViewHistory").on('click', ViewHistory);
        $(".SetInternalCollaboration").on('click', SetInternalCollaboration);
        $(".ViewSORTHistoryFn").on('click', ViewSORTHistoryFn);
        $(".SetSORTInternalCollaboration").on('click', SetSORTInternalCollaborationFn);
        
        
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
                    $('#General').html(result);
                }
            });
            return false;
        }
    });

    var randomNumber = Math.random();
    function ViewSORTHistoryFn(data) {
        var DocumentId = data.currentTarget.attributes.DocumentId.value;
        ViewSORTHistory(DocumentId);
    }
    function SetSORTInternalCollaborationFn() {
        var DocumentId = data.currentTarget.attributes.DocumentId.value;
        var CollaborationId = data.currentTarget.attributes.CollaborationId.value;
        SetSORTInternalCollaboration(DocumentId, CollaborationId)
    }
    function ViewHistory(data) {
        var DocumentId = data.currentTarget.attributes.DocumentId.value;
        randomNumber = Math.random();
        var link = "../Notification/DisplayCollaborationHistoryList" + EncodedQueryString("DocumentId=" + DocumentId + "&randomNumber" + randomNumber);
        window.location.href = link;
    }

    function SetInternalCollaboration(data) {
        var DocumentID = data.currentTarget.attributes.DocumentID.value;
        var CollaborationNo = data.currentTarget.attributes.CollaborationNo.value;
        randomNumber = Math.random();
        var link = "../Notification/CreateInternalCollaboration";
        var paramList = {
            DOCUMENT_ID: DocumentID,
            COLLABORATION_NO: CollaborationNo
        }
        $.ajax({
            async: false,
            type: "POST",
            url: '@Url.Action("StoreInternalCollaboration", "Notification")',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: JSON.stringify(paramList),
            processdata: true,
            success: function (result) {
                if (result) {
                    paramList = null;
                    window.location.href = link;
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
        var pv  = $('#hf_PageView').val(); 

        var esdalRefNo = "@TempData["StatusCode"]";
        var sortCollab = "@SORTCollab";
        var notifCollab = "@NotifCollab";

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
if($('#hf_SORTCollab').val() ==  'False') {
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

