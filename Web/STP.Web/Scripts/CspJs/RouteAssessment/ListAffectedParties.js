
    var id = 0;
    var RoutePartId = 0;
    $(document).ready(function () {
        selectedmenu('Movements'); // for selected menu
    });
    var analysisId  = $('#hf_AnalysisId').val(); 

    var NotificationId  = $('#hf_NotifId').val(); 

    var sortSideCall  = $('#hf_SortSideCall').val(); 
    //#region
    function loadAffectedParties(analysisId) {
        if (sortSideCall != 'true') {
            $.ajax({
                url: '../RouteAssessment/ListAffectedParties',
                type: 'POST',
                async: false,
                data: { analysisID: analysisId, anal_type: 7 },

                beforeSend: function () {
                    startAnimation();
                },

                success: function (data) {

                },
                error: function (xhr, textStatus, errorThrown) {

                    location.reload();
                },
                complete: function () {
                    stopAnimation();
                }

            });
        }
        else {
            $.ajax({
                url: '../SORTApplication/AffectedParties',
                type: 'POST',
                async: false,
                data: { NotificationId: NotificationId, analysisId: analysisId },

                beforeSend: function () {
                    startAnimation();
                },

                success: function (data) {

                },
                error: function (xhr, textStatus, errorThrown) {

                    location.reload();
                },
                complete: function () {
                    stopAnimation();
                }

            });
        }
    }
    //#endregion

    //#region
    function DisplayContact(id) {
        removescroll();

        var randomNumber = Math.random();

        $("#dialogue").load("../Application/ViewContactDetails?ContactId=" + id + "&RandomNumber=" + randomNumber);
        $("#dialogue").show();
        $("#overlay").show();
    }
    //#endregion

