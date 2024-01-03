    function SaveAcknowledgement(documentId, collaborationNo, esdalRefNo) {
        
    var randomNumber = Math.random();
    var result = LoadContentForAjaxCalls("POST", '../Notification/SaveAcknowledgement', { DocID: documentId, ColNo: collaborationNo, esdalRefNumber:esdalRefNo, random: randomNumber }, '#collaboration_details');
    CheckAcknowledged(result);
}
function CheckAcknowledged(result) {
    $('#hidden_acknowledged').val(result);
    DisplayCollaboration();
}

