var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
var MovLatestVer = $('#MovLatestVer').val();
var MovVersion = $('#versionno').val();
var AnalysisIdVal = $('#hf_AnalysisId').val();
var PlannrUserId = $('#PlannrUserId').length > 0 ? $('#PlannrUserId').val() : 0;

function Exclude(contactId, orgId, orgName) {

    if (((chk_status == 301002 || chk_status == 301005 || chk_status == 301008) && (sort_user_id != checker_id) && ((MovLatestVer == MovVersion) || MovLatestVer == 0))
        || ((chk_status == 301009 || chk_status == 301006) && sort_user_id != PlannrUserId)) {
        showErrorMessageIncludeExcludeNAP(chk_status);
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
                $("#YesIcon" + contactId).attr('style', 'cursor:pointer');
                $("#YesIcon" + contactId).attr("onclick", "Include(" + contactId + "," + orgId + ",'" + orgName + "')");

            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });

    }
}

function Include(contactId, orgId, orgName) {

    if (((chk_status == 301002 || chk_status == 301005 || chk_status == 301008) && (sort_user_id != checker_id) && ((MovLatestVer == MovVersion) || MovLatestVer == 0))
        || ((chk_status == 301009 || chk_status == 301006) && sort_user_id != PlannrUserId)) {
        showErrorMessageIncludeExcludeNAP(chk_status);
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
                $("#YesIcon" + contactId).attr("onclick", "Exclude(" + contactId + "," + orgId + ",'" + orgName + "')");

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

//Function to call dispensation
function ViewDispensationAffParties(Grantor_id, Grantor_name) {
    $('#hdnGrantor_Org').val(Grantor_name);
    $("#hdnDRN").val('');
    $("#hdnsummary").val('');
    $("#hdndescp").val('');
    $("#hdnfrmdate").val('');
    $("#hdnToDate").val('');
    $("#hdngrantby").val('');

    $("#hdngross").val('');
    $("#hdnaxle").val('');
    $("#hdnlength").val('');
    $("#hdnwidth").val('');
    $("#hdnheight").val('');

    var analysisId = $('#AnalId').val() ? $('#AnalId').val() : 0;
    var notifid = $('#Notificationid').val() ? $('#Notificationid').val() : 0;

    if (notifid == 0) {
        notifid = $('#hf_NotificationID').val();
    }

    $.ajax(
        {
            url: '../Notification/NotificationDispensationPopUp',
            type: 'POST',
            //async: false,
            data: { organisationId: Grantor_id, analysisId: analysisId, grantorName: Grantor_name.replace(/ /g, '%20'), notifid: notifid },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                SubStepFlag = 5.1;
                $('#div_dispensation').html(data);
                $('#div_address_parties').hide();
                $('#div_dispensation').find('div#filterDivDispensation').remove();
                var filters = $(data).find('div#filterDivDispensation');
                $(filters).insertAfter('#banner');
                NotificationDispensationPopUpInit(Grantor_id, Grantor_name);
            },
            error: function (xhr, textStatus, errorThrown) {
                stopAnimation();
            },
            complete: function () {
                stopAnimation();
            }
        });

    //$("#overlay").show();
    //$('.loading').show();
    //$("#dialogue").html('');


    //$("#dialogue").load("../Notification/NotificationDispensationPopUp?Org_Id=" + Grantor_id + "&hideLayout=" + true + "&analysisId=" + analysisId + "&Grantor_name=" + Grantor_name.replace(/ /g, '%20') + '&notifid=' + notifid, function (responseText, textStatus, req) {//+'&page='+1+'&pageSize='+10

    //    $('.loading').hide();
    //    $('#span-help').hide();
    //    if (textStatus == "error") {
    //        location.reload();
    //    }
    //});
    //removescroll();
    //resetdialogue();
    //$("#overlay").show();
    //$("#dialogue").show();
}

function showErrorMessageIncludeExcludeNAP(chk_status) {
    var message = "You are not allowed to perform this action!";
    if (chk_status == 301002 || chk_status == 301008 || chk_status == 301005) {
        message = "You are not allowed to perform this action during checking process";
    }
    showToastMessage({
        message: message,
        type: "error"
    })
}