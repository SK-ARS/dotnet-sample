    $(document).ready(function () {
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

        $("#CloseViewCollab").on('click', closeViewCollab);
        $("#SaveNotes").on('click', UpdateStatus);
        $("#cancel").on('click', loadPreviousLocation);
    });

    function SetStatus() {
        $("input[name=statusradiogroup][value=" + $('#STATUS').val() + "]").attr('checked', 'checked');
    }

        function loadPreviousLocation() {
if($('#hf_SORTSetCollab').val() ==  'True') {
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
if($('#hf_SORTSetCollab').val() ==  'True') {
            $('#DOCUMENT_ID').val($('#hdnDocumentID').val());
        }
        //var note = $('input[name=radionotes]:radio:checked').val();
        //if (note == '2') {
        //    var NOTES = $('#Description').val();
        //}
        var paramList = {
            DOCUMENT_ID: $('#DOCUMENT_ID').val(),
            STATUS: status,
            FIRST_NAME: "@Model.FIRST_NAME",
            SUR_NAME: "@Model.SUR_NAME",
            NOTES: $('#Description').val()
        }
        $.ajax({
            async: false,
            type: "POST",
            url: '@Url.Action("ManageCollaborationInternal", "Notification")',
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

if($('#hf_SORTSetCollab').val() ==  'True') {
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
