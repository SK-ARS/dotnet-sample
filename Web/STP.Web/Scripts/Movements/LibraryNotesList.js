$(document).ready(function () {
    $('body').on('click', '.ImportNotes', function (e) { e.preventDefault(); ImportNotes(this); });
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

function ImportNotes(_this) {
    var LibraryNotesId = $(_this).attr("librarynotesid");
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
    var randomNumber = Math.random();
    var analysisId = $('#StructAnalysisID').val();
    var inboxId = $('#HdnInboxID').val();
    var id = $('#DOCUMENT_ID').val();
    


    var loggedInContactId = $('#NotescontactId').val();
    var IS_MOST_RECENT = $('#IS_MOST_RECENT').val();

    var NextNoLongerAffected = $('#NextNoLongerAffected').val();
    var NOTIFICATION_ID = $('#NOTIFICATION_ID').val();
    var rttype = $('#RouteName').val() != undefined ? $('#RouteName').val().replace(" ", "") : "";
    var routeOriginal = $('#RouteType').val() != undefined ? $('#RouteType').val().replace(" ", "_") : "";
    var WipNENCollab = 0;
    var nenId = $('#hNEN_Id').val();
    if (nenId != "" && nenId!=undefined)
        WipNENCollab = 1;
    if (nenId == "" || nenId == undefined)
        nenId = 0;
    startAnimation();

    $("#popupDialogue").load("../Movements/ViewCollaborationStatusAndNotes?random=" + randomNumber + "&Notificationid=" + NOTIFICATION_ID + "&documentid=" + id + "&InboxId=" + inboxId + "&analysisId=" + analysisId + "&email=" + encodeURIComponent($('#EMail').val().replace(/ /g, '+')) + "&esdalRef=" + encodeURIComponent($('#ESDAL_Reference').val()) + "&contactId=" + loggedInContactId + "&route=" + encodeURIComponent(rttype) + "&IS_MOST_RECENT=" + IS_MOST_RECENT + "&routeOriginal=" + encodeURIComponent(routeOriginal) + "&NextNoLongerAffected=" + NextNoLongerAffected + "&LibraryNotesId=" + LibraryNotesId + "&WipNENCollab=" + WipNENCollab + "&NEN_ID=" + nenId, function () {
        stopAnimation();
        $('body').removeClass('overflowhidden');
        $('body').css("overflow-y", "scroll");
        $('#viewCollabModal').modal({ keyboard: false, backdrop: 'static' });
        $("#viewCollabModal").modal('show');
        $("#overlay").css("display", "block");
        $("#popupDialogue").css("display", "block");
        $('#' + idStatusGlobal).prop('checked', true);
        $('#updateExternalNote').hide();
        $('#ValidAgreement').hide();
        $('#ValidLatest').hide();
        $('#ValidStatus').hide();
        $('#CollabValidStatus').hide();
    });

}

