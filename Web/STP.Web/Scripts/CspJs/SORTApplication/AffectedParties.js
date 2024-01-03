    var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    var MovLatestVer = $('#MovLatestVer').val();
    var MovVersion = $('#versionno').val();
    function Exclude(contactId, orgId, orgName) {
        startAnimation();
        if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id) && ((MovLatestVer == MovVersion) || MovLatestVer == 0)) {
            ShowErrorPopup('You are not allowed to perform this action during checking process');
            stopAnimation();
        }
        else {
            $('#hidden_contactId_exclude').val(contactId);
            $('#hidden_orgId_exclude').val(orgId);
            $('#hidden_orgName_exclude').val(orgName);

            $.ajax({
            url: '@Url.Action("ExcludeAffectedParty", "SORTApplication")',
            type: 'GET',
            cache: false,
            async: false,
                data: { contactId: contactId, analysisId: @ViewBag.AnalysisId, organisationId: orgId, organisationName: orgName },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                $("#YesIcon" + contactId).attr('src', '../Content/assets/images/no-icon.svg');
                $("#YesIcon" + contactId).attr('title', 'Excluded');
                $("#YesIcon" + contactId).attr("onclick", "Include(" + contactId + "," + orgId + ",'" + orgName+"')");

            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });



            //$('#form_exclude').submit();
        }
    }

    function Include(contactId, orgId, orgName) {
        startAnimation();
        if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id) && ((MovLatestVer == MovVersion) || MovLatestVer == 0)) {
            ShowErrorPopup('You are not allowed to perform this action during checking process');
        }
        else {
            $('#hidden_contactId_include').val(contactId);
            $('#hidden_orgId_include').val(orgId);
            $('#hidden_orgName_include').val(orgName);

            $.ajax({
            url: '@Url.Action("IncludeAffectedParty", "SORTApplication")',
            type: 'GET',
            cache: false,
            async: false,
                data: { contactId: contactId, analysisId: @ViewBag.AnalysisId, organisationId: orgId, organisationName: orgName },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                $("#YesIcon" + contactId).attr('src', '../Content/assets/images/yes-icon.svg');
                $("#YesIcon" + contactId).attr('title', 'Included');
                $("#YesIcon"+contactId).attr("onclick", "Exclude(" + contactId + "," + orgId + ",'" + orgName + "')");

            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });



            //$('#form_include').submit();
        }
    }

    function LoadAffectedParties(result) {

        if (result) {
            var random = Math.random();
            var notificationId  = $('#hf_NotifId').val(); 
            var analysisId  = $('#hf_AnalysisId').val(); 

            //#region Keep this portion commented.
            ////var url = '../Notification/ManualAddedParties?NotifID=' + notificationId + '&analysisId=' + analysisId + '&isSORT=true' + '&random=' + random + '';
            ////$('#div_address_parties').load(url, function () {
            ////    stopAnimation();
            ////});
            //#endregion

            //above portion is commented to avoid entire affected parties page getting refereshed and thus if an already unchecked haulier contact reappears.
            $.post('/Notification/GetSortAffectedParties?analysisId=' + analysisId + '&isSORT=true' + '&random=' + random + '', function (data) {
                $('#ManuallyAddedContact').html(data);
                stopAnimation();
            });
        }
        else {
            stopAnimation();
            showWarningPopDialog('Process is not completed, Please try again.', 'Ok', '', 'WarningCancelBtn', 1, 'error');
        }
    }

