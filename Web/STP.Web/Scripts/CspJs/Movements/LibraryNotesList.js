    $(document).ready(function () {
        $(".ImportNotes").on('click', ImportNotes);
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

    function ImportNotes(data) {
        var LibraryNotesId = data.currentTarget.attributes.LibraryNotesId.value;
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
          startAnimation();

        $("#popupDialogue").load("../Movements/ViewCollaborationStatusAndNotes?random=" + randomNumber + "&Notificationid=" + NOTIFICATION_ID + "&documentid=" + id + "&InboxId=" + inboxId + "&analysisId=" + analysisId + "&email=" + $('#EMail').val().replace(/ /g, '+') + "&esdalRef=" + $('#ESDAL_Reference').val() + "&contactId=" + loggedInContactId + "&route=" + rttype + "&IS_MOST_RECENT=" + IS_MOST_RECENT + "&routeOriginal=" + routeOriginal + "&NextNoLongerAffected=" + NextNoLongerAffected + "&LibraryNotesId=" + LibraryNotesId, function () {
            stopAnimation();
            $('body').removeClass('overflowhidden');
            $('body').css("overflow-y", "scroll");
            $('#viewCollabModal').modal({ keyboard: false, backdrop: 'static' });
            $("#viewCollabModal").modal('show');
            $("#overlay").css("display", "block");
            $("#popupDialogue").css("display", "block");
          });

    }

