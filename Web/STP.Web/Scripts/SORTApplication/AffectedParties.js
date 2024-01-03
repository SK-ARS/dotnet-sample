var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
var MovLatestVer = $('#MovLatestVer').val();
var MovVersion = $('#versionno').val();
var AnalysisIdVal = $('#hf_AnalysisId').val();
var PlannrUserId = $('#PlannrUserId').length > 0 ? $('#PlannrUserId').val() : 0;

function Sort_AffectedPartiesInit() {
    chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    MovLatestVer = $('#MovLatestVer').val();
    MovVersion = $('#versionno').val();
    AnalysisIdVal = $('#hf_AnalysisId').val();
    PlannrUserId = $('#PlannrUserId').length > 0 ? $('#PlannrUserId').val() : 0;
    AffectedPartiesInit();
}


function Exclude(contactId, orgId, orgName) {
   
    if (((chk_status == 301002 || chk_status == 301005 || chk_status == 301008) && (sort_user_id != checker_id) && ((MovLatestVer == MovVersion) || MovLatestVer == 0))
        || ((chk_status == 301009 || chk_status == 301006) && sort_user_id != PlannrUserId)) {
        showErrorMessageIncludeExclude(chk_status);
    }
    else {
        $('#hidden_contactId_exclude').val(contactId);
        $('#hidden_orgId_exclude').val(orgId);
        $('#hidden_orgName_exclude').val(orgName);

        $.ajax({
            url: '../SORTApplication/ExcludeAffectedParty',
            type: 'GET',
            cache: false,
            //async: false,
            data: {
                contactId: contactId, analysisId: AnalysisIdVal, organisationId: orgId, organisationName: orgName
            },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                $("#YesIcon" + contactId).attr('src', '../Content/assets/images/no-icon.svg');
                $("#YesIcon" + contactId).attr('title', 'Excluded');
                $("#YesIcon" + contactId).removeClass('exclude');
                $("#YesIcon" + contactId).addClass('include');
                showToastMessage({ message: "Successfully excluded", type: "success" });
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
   
    if (((chk_status == 301002 || chk_status == 301005 || chk_status == 301008) && (sort_user_id != checker_id) && ((MovLatestVer == MovVersion) || MovLatestVer == 0))
        || ((chk_status == 301009 || chk_status == 301006) && sort_user_id != PlannrUserId)) {
        showErrorMessageIncludeExclude(chk_status);
    }
    else {
        $('#hidden_contactId_include').val(contactId);
        $('#hidden_orgId_include').val(orgId);
        $('#hidden_orgName_include').val(orgName);

        $.ajax({
            url: '../SORTApplication/IncludeAffectedParty',
            type: 'GET',
            cache: false,
            //async: false,
            data: {
                contactId: contactId, analysisId: AnalysisIdVal, organisationId: orgId, organisationName: orgName
            },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                $("#YesIcon" + contactId).attr('src', '../Content/assets/images/yes-icon.svg');
                $("#YesIcon" + contactId).attr('title', 'Included');
                $("#YesIcon" + contactId).removeClass('include');
                $("#YesIcon" + contactId).addClass('exclude');
                showToastMessage({ message: "Successfully included", type: "success" });
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
        var notificationId = $('#hf_NotifId').val();
        var analysisId = $('#hf_AnalysisId').val();

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

function showErrorMessageIncludeExclude(chk_status) {
    var message = "You are not allowed to perform this action!";
    if (chk_status == 301002 || chk_status == 301008 || chk_status == 301005) {
        message = "You are not allowed to perform this action during checking process";
    }
    showToastMessage({
        message: message,
        type: "error"
    })
}

