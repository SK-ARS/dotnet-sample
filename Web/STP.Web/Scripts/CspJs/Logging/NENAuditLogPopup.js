    var selectedVal;
    var pageNum = 1;
    $(document).ready(function () {
        showList();
    });


    function showList(sortOrderGlobal = 1, sortTypeGlobal = 0, pageSize, pageNum = 1) {
        startAnimation();
        debugger
        $("#Config-body").load('../Logging/ListNENAuditPerNotification?page=' +pageNum + '&pageSize=' +pageSize + '&NEN_Notif_No=' + '@ViewBag.NEN_Notif_No' + '&sortOrder=' + $(' #SortOrderValue').val() + '&sortType=' + $('#SortTypeValue').val(),
            function () {
                stopAnimation();
            }
        );
    }



