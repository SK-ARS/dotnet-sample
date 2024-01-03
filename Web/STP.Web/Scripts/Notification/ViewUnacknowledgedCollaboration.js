$(document).ready(function () {
    $('body').on('click', '#SaveAcknowledgement', function (e) { e.preventDefault(); SaveAcknowledgement(this); });
});

function SaveAcknowledgement(_this) {
    //  $('#form_save_unack').submit();
    var documentId = $(_this).attr('documentid');
    var collaborationNo = $(_this).attr('collaborationno');
    var esdalRefNo = $(_this).attr('esdalrefnumber');
    var isHistoric = $("#Historical").length > 0 ? $("#Historical").val() : 0;
    $.ajax({
        type: "POST",
        url: '../Notification/SaveAcknowledgement',
        data: { DocID: documentId, ColNo: collaborationNo, esdalRefNumber: esdalRefNo, historic: isHistoric },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            CheckAcknowledged(response)
        },
        error: function (result) {
        },
        complete: function () {
            stopAnimation();
        }
    });

}

function CheckAcknowledged(result) {
    $('#hidden_acknowledged').val(result);
    DiplayCollaboration();
}

function SaveAcknowledgementCollab(documentId, collaborationNo, esdalRefNo) {

    var randomNumber = Math.random();
    var isHistoric = $("#Historical").length > 0 ? $("#Historical").val() : 0;
    var result = LoadContentForAjaxCalls("POST", '../Notification/SaveAcknowledgement', { DocID: documentId, ColNo: collaborationNo, esdalRefNumber: esdalRefNo, random: randomNumber, historic: isHistoric }, '#collaboration_details');
    CheckAcknowledged(result);
}
function CheckAcknowledgedCollab(result) {
    $('#hidden_acknowledged').val(result);
    DisplayCollaboration();
}



