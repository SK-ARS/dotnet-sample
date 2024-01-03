    $(document).ready(function () {
        $(".SaveAcknowledgement").on('click', SaveAcknowledgement);       
    });

    function SaveAcknowledgement(data) {
        //  $('#form_save_unack').submit();
        var documentId = data.currentTarget.attributes.DocumentId.value;
        var collaborationNo = data.currentTarget.attributes.CollaborationNo.value;
        var esdalRefNo = data.currentTarget.attributes.EsdalRefNumber.value;

        $.ajax({
            type: "POST",
            url: '../Notification/SaveAcknowledgement',
            data: { DocID: documentId, ColNo: collaborationNo, esdalRefNumber: esdalRefNo },
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

