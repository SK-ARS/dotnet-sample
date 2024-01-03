    $(document).ready(function () {
        $(".ViewNotifiedParties").on('click', ViewNotifiedParties);
    });

    function ViewNotifiedParties(data) {
        var ID = data.currentTarget.attributes.ContactId.value;
        removescroll();
        var random = Math.random();
        $("#dialogue").load("../Application/ViewContactDetails?ContactId=" + ID + "&random=" + random, function () {
            $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
            $("#dialogue").css("display", "block");
            $('#contactDetails').modal('show');
            stopAnimation();
            $("#overlay").css("display", "block");
        });
    }

    function removeHLinksParty(cntrId) {
        $(cntrId).find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    }
    function PaginateGridParty(cntrId) {
        $(cntrId).find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
            var pageNum = $(this).html();
            AjaxPaginationForParties(pageNum, cntrId);
        });
        PaginateToLastPagesParties(cntrId);
        PaginateToFirstPagesParties(cntrId);
        PaginateToNextPagesParties(cntrId);
        PaginateToPrevPagesParties(cntrId);
    }
    function PaginateToLastPagesParties(ContainerId) {
        $(ContainerId).find('.PagedList-skipToLast').click(function () {
            var pageNum = $(ContainerId).find('#TotalPages').val();
            AjaxPaginationForParties(pageNum, ContainerId);
        });
    }
    function PaginateToFirstPagesParties(ContainerId) {
        $(ContainerId).find('.PagedList-skipToFirst').click(function () {
            AjaxPaginationForParties(1, ContainerId);
        });
    }
    function PaginateToNextPagesParties(ContainerId) {
        $(ContainerId).find('.PagedList-skipToNext').click(function () {
            var thisPage = $(ContainerId).find('.active').find('a').html();
            var nextPage = parseInt(thisPage) + 1;
            AjaxPaginationForParties(nextPage, ContainerId);
        });
    }
    function PaginateToPrevPagesParties(ContainerId) {
        $(ContainerId).find('.PagedList-skipToPrevious').click(function () {
            var thisPage = $(ContainerId).find('.active').find('a').html();
            var prevPage = parseInt(thisPage) - 1;
            AjaxPaginationForParties(prevPage, ContainerId);
        });
    }
    function AjaxPaginationForParties(pageNum, cntrId) {
        var pageSize = $('#pageSizeVal').val();
        $.ajax({
            url: '../Notification/DisplayNotifiedparties',
            data: { page: pageNum, pageSize: pageSize, analysisId: $('#NotifAnalysisId').val(), notificationId: $("#NotificationId").val() },
            type: 'GET',
            async: false,
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                $(cntrId).html(page);
                $('#pageSizeVal').val(pageSize);
                $('#pageSizeSelect').val(pageSize);
                removeHLinksParty(cntrId);
                PaginateGridParty(cntrId);
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    function fillPageSizeSelectParty() {
        var selectedVal = $('#pageSizeVal').val();
        $('#pageSizeSelect').val(selectedVal);
    }
