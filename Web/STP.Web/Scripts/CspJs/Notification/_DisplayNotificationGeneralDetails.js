    $(document).ready(function () {
        $("#CRNo").val('@Model.ContentReferenceNo');
        $("#NotifAnalysisId").val('@Model.AnalysisId');
        $("#ProjectId").val('@Model.ProjectId');
        $("#VersionId").val('@Model.VersionId');
        $("#ProjectStatus").val('@Model.ProjectStatus');
        $('#NotificationCode').val('@Model.ESDALReference');
        var string = $('#NotificationCode').val() + ' - '+ $("#MoveStat").val();
        $('#notif_code').text(string);
if($('#hf_ShowWarning').val() ==  'True')
        {
			$('#ShowWanring').show();
		}
		else
        {
            $('#ShowWanring').hide();
        }


        $("#PrintNotificationReport").on('click', PrintNotificationReport);
        $("#ShowHideDetails").on('click', ShowHideDetails);
        $("#Renotify").on('click', functionRenotify);
        $("#Clone").on('click', functionClone);

    });

    functionClone(data)
    {
        PreviousNotificationCode = data.currentTarget.attributes.NotificationCode.value;
        notificationId = data.currentTarget.attributes.notificationId.value;
        RevisionId = data.currentTarget.attributes.RevisionId.value;
        Renotify(PreviousNotificationCode, notificationId, RevisionId);
    }

    function functionRenotify(data) {
        PreviousNotificationCode = data.currentTarget.attributes.PreviousNotificationCode.value;
        notificationId = data.currentTarget.attributes.NotificationId.value;
        Renotify(PreviousNotificationCode, notificationId);

    }

    function PrintNotificationReport(data) {
        debugger;
        var NotificationId = data.currentTarget.attributes.NotificationId.value;
        var ContactId = data.currentTarget.attributes.ContactId.value;
        var link = "../Document/PrintIndexV";

        var link = "../Notification/HaulierNotificationDocument?notificationId=" + NotificationId + "&contactId=" + ContactId + "";
        window.open(link, '_blank');
    }
    function ShowHideDetails(data) {
        var target = data.currentTarget.attributes.target.value;
        switch (target) {
            case 6:
                //req-notes
                if (document.getElementById('dispensation-div').style.display !== "none") {
                    document.getElementById('dispensation-div').style.display = "none"
                    document.getElementById('chevlon-up-sog-icon6').style.display = "none"
                    document.getElementById('chevlon-down-sog-icon6').style.display = "block"
                }
                else {
                    document.getElementById('dispensation-div').style.display = "block"
                    document.getElementById('chevlon-up-sog-icon6').style.display = "block"
                    document.getElementById('chevlon-down-sog-icon6').style.display = "none"
                }
                break;
        }

    }
