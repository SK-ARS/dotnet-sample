var firstName;
var surName;
function CreteInternalCollaborationInit() {
    firstName = $('#hf_First_Name').val();
    surName = $('#hf_Sur_Name').val();
    $('#ValidStatus').hide();
    $('#updateInternalNote').hide();
    SetStatus();
    $('.modal-content').css('background', 'transparent');
    $('.CustModel').css('background', 'white');
    $('.modal-content').css('border', 'aliceblue');
    $('.modal-backdrop').modal({
        backdrop: 'static',
        keyboard: false
    });
    $('#exampleModalCenter2').modal({
        backdrop: 'static',
        keyboard: false
    });
}
$(document).ready(function () {
$('body').on('click','#CloseViewCollab', function(e) { 
  e.preventDefault();
  closeViewCollab(this);
}); 
$('body').on('click','#SaveNotes', function(e) { 
  e.preventDefault();
  UpdateStatus(this);
}); 
$('body').on('click','#cancel', function(e) { 
  e.preventDefault();
  loadPreviousLocation(this);
}); 
});
function SetStatus() {
    $("input[name=statusradiogroup][value=" + $('#STATUS').val() + "]").attr('checked', 'checked');
}
function loadPreviousLocation() {
    if ($('#hf_SORTSetCollab').val() != 'True') {
        var link = "../Notification/CollaborationStatusList";
        window.location.href = link;
    }
    else {
        closeViewCollab();
    }
}
function UpdateStatus() {
    $('#updateInternalNote').hide();
    $('#ValidStatus').hide();
    var status = $('input[name=statusradiogroup]:radio:checked').val();
    if ($('#STATUS').val() == status && $('#NOTES').val().trim() == $('#Description').val().trim()) {
        $('#updateInternalNote').show();
        return false;
    }

    if (typeof (status) === "undefined") {
        $('#ValidStatus').show();
        return false;
    }

    var status = $('input[name=statusradiogroup]:radio:checked').val();
    if ($('#hf_SORTSetCollab').val() == 'True') {
        $('#DOCUMENT_ID').val($('#hdnDocumentID').val());
    }
    //var note = $('input[name=radionotes]:radio:checked').val();
    //if (note == '2') {
    //    var NOTES = $('#Description').val();
    //}
    var paramList = {
        DOCUMENT_ID: $('#DOCUMENT_ID').val(),
        STATUS: status,
        FIRST_NAME: firstName,
        SUR_NAME: surName,
        NOTES: $('#Description').val()
    }
    $.ajax({
        async: false,
        type: "POST",
        url: '../Notification/ManageCollaborationInternal',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: JSON.stringify(paramList),
        processdata: true,
        success: function (result) {
            $("#overlay").hide();
            stopAnimation();
            $(".modal-backdrop").removeClass("show");
            $(".modal-backdrop").removeClass("modal-backdrop");
            $('.modal-content').css('background-color', '#fff');

            if ($('#hf_SORTSetCollab').val() == 'True') {
                $('#CollabHistoryDiv').modal('hide');
                ShowSuccessModalPopup("Internal collaboration updated successfully", "loadPreviousLocation");
            } else {
                $('#CollabHistoryDiv').modal('hide');
                ShowSuccessModalPopup("Internal collaboration updated successfully", "closeCollabSuccesspopup");

                //showWarningPopDialog('', 'Ok', '', 'ClosePOPCollab', '', 1, 'info');
            }
        },
        error: function () {
        },
        complete: function () {

        }
    });
}
function closeCollabSuccesspopup() {
    $('#SuccessPopupAction').modal('hide');
    DisplayCollabStatus();
}