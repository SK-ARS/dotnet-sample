var NEN_Notif_No = $('#hf_nenotifno').val();
var selectedVal;
var pageNum = 1;
$(document).ready(function () {
    
});

function NENAuditLogpopupInit() {
    NEN_Notif_No = $('#hf_nenotifno').val();
    showList();
}


function showList(sortOrderGlobal = 1, sortTypeGlobal = 1, pageSize, pageNum = 1) {
    startAnimation();

    $("#Config-body").load('../Logging/ListNENAuditPerNotification?page=' + pageNum + '&pageSize=' + pageSize + '&NEN_Notif_No=' + NEN_Notif_No + '&sortOrder=' + sortOrderGlobal + '&sortType=' + sortTypeGlobal,
        function () {
            stopAnimation();
        }
    );
}



