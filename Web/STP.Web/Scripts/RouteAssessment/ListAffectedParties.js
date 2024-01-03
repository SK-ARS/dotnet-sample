var id;
var RoutePartId;
var hf_analysisId;
var hf_NotificationId;
var hf_sortSideCall;

function ListAffectedPartiesInit() {
    RoutePartId = 0;
    id = 0;
    hf_analysisId = $('#hf_AnalysisId').val();
    hf_NotificationId = $('#hf_NotifId').val();
    hf_sortSideCall = $('#hf_SortSideCall').val();
    selectedmenu('Movements');
    AffectedPartiesInit();
    $('#route-assessment').show();
    $('#back_btn').show();
    $('#confirm_btn').show();
}
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

function DisplayContact(id) {
    removescroll();
    var randomNumber = Math.random();
    $("#dialogue").load("../Application/ViewContactDetails?ContactId=" + id + "&RandomNumber=" + randomNumber);
    $("#dialogue").show();
    $("#overlay").show();
}

