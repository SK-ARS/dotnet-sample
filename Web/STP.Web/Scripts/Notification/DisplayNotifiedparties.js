$(document).ready(function () {
    $('body').on('click', '.ViewNotifiedParties', function (e) { e.preventDefault(); ViewNotifiedParties(this); });

    $('body').on('click', '#Notifiedparties .pagination-container-new a', function (e) {
        e.preventDefault();
        if (this.href == '') {
            return false;
        }
        else {
            var page = getUrlParameterByName("page", this.href);
            $('#pageNum').val(page);
            AjaxPaginationForParties(page, '#notif_parties');
        }
    });

    $('body').on('change', '#Notifiedparties  #pageSizeSelect', function () {
        $('#pageNum').val(1);
        $('#pageSizeVal').val($(this).val());
        AjaxPaginationForParties(1, '#notif_parties');
    });

});

function ViewNotifiedParties(_this) {
    var ID = $(_this).attr('contactid');
    removescroll();
    var random = Math.random();
    startAnimation();
    $("#dialogue").load("../Application/ViewContactDetails?ContactId=" + ID + "&random=" + random, function () {
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
        $("#dialogue").css("display", "block");
        $('#contactDetails').modal('show');
        stopAnimation();
        $("#overlay").css("display", "block");
    });
}

function AjaxPaginationForParties(pageNum, cntrId) {
    var pageSize = $('#pageSizeVal').val();
    $.ajax({
        url: '../Notification/DisplayNotifiedparties',
        data: { page: pageNum, pageSize: pageSize, analysisId: $('#NotifAnalysisId').val(), notificationId: $("#NotificationId").val() },
        type: 'GET',
        //async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $(cntrId).html(page);
            //$('#pageSizeVal').val(pageSize);
            //$('#pageSizeSelect').val(pageSize);
            //removeHLinksParty(cntrId);
            //PaginateGridParty(cntrId);
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}

