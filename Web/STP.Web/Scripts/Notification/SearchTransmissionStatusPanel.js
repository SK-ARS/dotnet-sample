$(document).ready(function () {
    checkboxselection();
});
function checkboxselection() {
    // change child checkbox as per header check box selection
    $('#All').change(function () {
        $(".check-box").prop('checked', $(this).prop('checked'));
    });

    // change header checkbox as per child check box selection
    $('.check-box').change(function () {
        if ($(this).is(':checked')) {
            if ($('.check-box:checked').length == 4) {
                $("#All").prop('checked', true);
            }
        }
        else {
            $("#All").prop('checked', false);
        }
    });
}
function SearchTransmission() {

    var SORT_APP = $('#SORTApplication').val() ? $('#SORTApplication').val() : 0;
    if (SORT_APP == "True") {
        var showtrans = true;
    }
    var SortApp_Status = $('#SortAppStatus').val() ? $('#SortAppStatus').val() : 0;
    $("#overlay").show();
    $('.loading').show();
    var randomNumber = Math.random();
    var url = '../Notification/TransmissionStatusList';
    $.ajax(
        {
            type: "post",
            data: $("#SearchPanelForm").serialize() + "&showtrans=" + showtrans + "&SortStatus=" + SortApp_Status,
            url: url,
            success: function (data) {
                // data contains your partial view
                if (SORT_APP == "True") {
                    $('#tab_7').html(data);
                    removeHLinksTrans();
                    PaginateGridTrans();
                    fillPageSizeSelect();
                } else {
                    $('#tab_5').html(data);
                }
                closeFilters_trans();
            },
            complete: function () {
                stopAnimation();
            }

        });
}
//function remove href from pagination ul li
function removeHLinksTrans() {
    $('#tab_7').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
}
function fillPageSizeSelect() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
}
//function Pagination
function PaginateGridTrans() {
    //method to paginate through page numbers
    $('#tab_7').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        //var pageCount = $('#TotalPages').val();
        var pageNum = $(this).html();
        // CheckSessionTimeOut();
        AjaxPaginationTrans(pageNum);
    });

    PaginateToLastPageTrans();
    PaginateToFirstPageTrans();
    PaginateToNextPageTrans();
    PaginateToPrevPageTrans();
}
//method to paginate to last page
function PaginateToLastPageTrans() {

    $('#tab_7').find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#TotalPages').val();
        // CheckSessionTimeOut();
        AjaxPaginationTrans(pageCount);
    });
}
//method to paginate to first page
function PaginateToFirstPageTrans() {

    $('#tab_7').find('.PagedList-skipToFirst').click(function () {
        AjaxPaginationTrans(1);
        // CheckSessionTimeOut();
    });
}
//method to paginate to Next page
function PaginateToNextPageTrans() {

    $('#tab_7').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#tab_7').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        // CheckSessionTimeOut();
        AjaxPaginationTrans(nextPage);
    });
}
//method to paginate to Previous page
function PaginateToPrevPageTrans() {
    $('#tab_7').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#tab_7').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        // CheckSessionTimeOut();
        AjaxPaginationTrans(prevPage);
    });
}
//function Ajax call fro pagination
function AjaxPaginationTrans(pageNum) {
    var selectedVal = $('#pageSizeVal').val();
    var pageSize = selectedVal;
    var SortApp_Status = $('#SortAppStatus').val() ? $('#SortAppStatus').val() : 0;
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var versionno = $('#versionno').val();
    var esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    var page_view = null;
    if ($('#All').is(':checked')) {
        page_view = true;
    }
    //var tmf = {All: true, Delivered: true, Failed: true, Pending: true, Sent: true }
    sURL = 'Notification/TransmissionStatusList';
    $.ajax({
        url: '../' + sURL,
        data: { page: pageNum, pageSize: pageSize, PageView: page_view, showtrans: true, SortStatus: SortApp_Status },
        type: 'GET',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#tab_7').html(page);
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            removeHLinksTrans();
            PaginateGridTrans();
            fillPageSizeSelect();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function ClearTransmission() {

    var _Filter = $('#filter_SORT');
    ClearTransStatus();
    _Filter.find('input:checkbox').attr('checked', 'checked');
    //_Filter.find('input:checkbox').prop("checked", true);
}
function ClearTransStatus() {

    //var status = $('#SortStatus').val();Notification_Code
    //var Version_Status = $('#versionStatus').val();
    var doc_status = 0;
    //switch (Version_Status) {
    //    case "305002"://proposed
    //        doc_status = 1;
    //        break;
    //    case "305003"://reproposed
    //        doc_status = 2;
    //        break;
    //    case "305004"://agreed
    //        doc_status = 3;
    //        break;
    //    case "305005"://agreed revised
    //        doc_status = 4;
    //        break;
    //    case "305006"://agreed recleared
    //        doc_status = 5;
    //        break;
    //}
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    //$("#leftpanel").html('');
    //$("#leftpanel").hide();
    //$('#leftpanel_div').hide();
    //$('#leftpanel_quickmenu').hide();
    //$("#General").html('');
    //$("#General").hide();
    var tmf = { All: true, Delivered: true, Failed: true, Pending: true, Sent: true }
    collab = false;
    if (esdal_ref_no != 0) {
        $.ajax({
            url: '../Notification/TransmissionStatusList',
            data: { page: null, pageSize: null, PageView: true, TMF: tmf, Notification_Code: esdal_ref_no, showtrans: false, SortStatus: 0 },
            type: 'GET',
            cache: false,
            async: false,
            beforesend: function () {
                startAnimation()
            },
            success: function (page) {

                $('#tab_7').html(page);
                removeHLinksTrans();
                PaginateGridTrans();
                fillPageSizeSelect();
                CheckSessionTimeOut();
                closeFilters_trans();
            },
            error: function (page) {
            },
            complete: function () {
                stopAnimation()
            }
        });
    } else {
        $('#tab_7').html('<span class="error" style="text-align:center;"><h4 style="color:#414193;margin: 0px 0;font-size: 1.0em;">There is no transmission status for this application. </h4> </span><br/>');
    }

}