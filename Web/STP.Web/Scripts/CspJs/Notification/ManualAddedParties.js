
$(document).ready(function () {
    if ($('#hf_IsSortSide').val() == "SortSideCall") {
        SortaddMeAsAffectedByDeafult();
        document.getElementById("SortaddMeAsAffected").checked = true;
        var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
        var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
        var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
        var MovLatestVer = $('#MovLatestVer').val();
        var MovVersion = $('#versionno').val();
        if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id) && ((MovLatestVer == MovVersion) || MovLatestVer == 0)) {
            $('#addcontact').hide();
            /*$('#SortaddMeAsAffected').prop('disabled', true);*/
            $('#SortaddMeAsAffected').attr('readonly', true);
        }
    }
    else {
        if ($('#hf_AffectFlag').val() == 1) {
            //AddMeAsAffectedbyDefault();
            document.getElementById("addMeAsAffected").checked = true;
        }
    }
});

$('#addMeAsAffected').click(function () {
    if ($("#addMeAsAffected").is(":checked")) {
        AddMeAsAffected();
        document.getElementById("addMeAsAffected").checked = true;
    }
    else {
        //deleting if unchecked
        var contactName = $('#LoginUser').val();
        var orgName = $('#LoginOrg').val();
        DeleteAffectedContact(orgName, contactName);
    }
})

//region
$('#SortaddMeAsAffected').click(function () {
    var isReadOnly = $(this).attr("readonly") === undefined ? false : true;

    if (isReadOnly) {
        ShowErrorPopup('You are not allowed to perform this action during checking process');
        return false;
    }
    else {
        if ($("#SortaddMeAsAffected").is(":checked")) {
            SortaddMeAsAffected();
            document.getElementById("SortaddMeAsAffected").checked = true;
        }
        else {
            //deleting if unchecked the sort side haulier added to manually added contact list of affected parties.
            var contactName = $('#HaulName').val();
            var orgName = $('#HaulOrganisationName').val();
            //DeleteAffectedContact(orgName, contactName);
            Deleteallmannuallyaddedparties(orgName, contactName);
        }
    }
})

//#region
function AddMeAsAffected() {
    var analysisId = @ViewBag.AnalysisIdManual;
    var notifid = $('#NotificationId').val();
    $.ajax(
        {
            url: '../Contact/AffectedContactDetail',
            type: 'POST',
            //async: false,
            data: { ContactID: 0, NotifID: notifid, analysisId: analysisId, NotifyingContact: true },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                if (data.result.status != null) {
                    ShowErrorPopup('Contact already exist');
                }
                else {
                    loadPage(data.result.NotifID, data.result.analysisId);
                    document.getElementById("addMeAsAffected").checked = true;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                stopAnimation();
            },
            complete: function () {
                stopAnimation();
                $("#overlay").hide();
            }
        });

}
//#endregion

//region
function SortaddMeAsAffected() {
    var iscandidatert = $('#IsCandidateRT').val();
    var analysisId = 0;
    if (iscandidatert == true) {
        analysisId = $('#candAnalysisId').val();
    }
    else {
        analysisId = $('#AnalysisId').val();
    }

    var sortOrgId = $('#OrganisationId').val();
    var apprevisionid = $('#ApprevId').val();

    $.ajax({

        url: '../Contact/AffectedContactDetail',
        type: 'POST',
        //async: false,
        data: { ContactID: 0, analysisId: analysisId, NotifyingContact: false, fromSort: true, haulOrgID: sortOrgId, revisionId: apprevisionid },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {

            if (data.result.HaulFullName && data.result.HaulOrgName) {
                if (data.result.HaulFullName != null && data.result.HaulFullName != "" && data.result.HaulOrgName != null && data.result.HaulOrgName != "") {
                    $('#HaulName').val(data.result.HaulFullName);
                    $('#HaulOrganisationName').val(data.result.HaulOrgName);
                }
            }
            if (data.result.status != null) {
                showWarningPopDialog('Contact already exist', 'Ok', '', 'WarningCancelBtn', 'ReloadLocation', 1, 'info');
            }
            else {
                $.post('/Notification/GetSortAffectedParties?analysisId=' + analysisId, function (data) {
                    $('#ManuallyAddedContact').html(data);
                });
                //loadPage(data.result.NotifID, data.result.analysisId);
            }

        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            stopAnimation();
            //location.reload();
        },
        complete: function () {
            stopAnimation();
            //ReloadaffectedParties();
        }
    });

    //ClosePopUp();
}

function SortaddMeAsAffectedByDeafult() {
    var iscandidatert = $('#IsCandidateRT').val();
    var analysisId = 0;
    if (iscandidatert == true) {
        analysisId = $('#candAnalysisId').val();
    }
    else {
        analysisId = $('#AnalysisId').val();
    }

    var sortOrgId = $('#OrganisationId').val();
    var apprevisionid = $('#ApprevId').val();

    $.ajax({

        url: '../Contact/AffectedContactDetail',
        type: 'POST',
        async: false,
        data: { ContactID: 0, analysisId: analysisId, NotifyingContact: false, fromSort: true, haulOrgID: sortOrgId, revisionId: apprevisionid },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (data.result.HaulFullName && data.result.HaulOrgName) {
                if (data.result.HaulFullName != null && data.result.HaulFullName != "" && data.result.HaulOrgName != null && data.result.HaulOrgName != "") {
                    $('#HaulName').val(data.result.HaulFullName);
                    $('#HaulOrganisationName').val(data.result.HaulOrgName);
                }
            }
            $.post('/Notification/GetSortAffectedParties?analysisId=' + analysisId + "&isSORT=" + true, function (data) {
                $('#ManuallyAddedContact').html(data);
            });
        },
        error: function (xhr, textStatus, errorThrown) {
            stopAnimation();
        },
        complete: function () {
            stopAnimation();
        }
    });

    //ClosePopUp();
}
function ReloadaffectedParties() {
    de
    revisionId = $('#RevID').val();
    AnalysisId = $('#AnalysisId').val();
    anal_Type = 7;

    var randomNum = Math.random();
    //$('#generatebutton').hide();
    //uncomment for the calls,
    startAnimation();
    if (SORTflag != "true") {

        showWarningPopDialog('This functionality is not implemented', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');

    }
    else {
        var iscandidatert = $('#IsCandidateRT').val();
        var distributedMoveAnalyId = 0;

        //condition to add distributed movements analysis id in case its a candidate route and has a previous distributed movement
        if (iscandidatert == 'True') {
            revisionId = $('#revisionId').val();
            AnalysisId = $('#candAnalysisId').val();
            distributedMoveAnalyId = $('#DistributedMovAnalysisId').val();
        }
        //Notification id is not passed ! passed as 0 this is from sort side affected parties page needs to be called
        //$('#RouteAssesment').load('../SORTApplication/AffectedParties?analysisId=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&DistributedMovAnalysisId=' + distributedMoveAnalyId + '&random=' + randomNum, function () {
        //    stopAnimation();
        //    $('#SearchStructure').hide();
        //    $('#div_StructureGeneralDetails').hide();
        //});

        $.post('/Notification/GetSortAffectedParties?analysisId=' + AnalysisId, function (data) {
            $('#ManuallyAddedContact').html(data);
            stopAnimation();
            $('#SearchStructure').hide();
            $('#div_StructureGeneralDetails').hide();
        });

    }
}
//endregion


//#region
function DeleteAffectedContact(orgName, contactName) {
    var analysisId = $('#AnalId').val();
    if (analysisId == undefined)
        analysisId = $('#NotifAnalysisId').val();
    var notifid = $('#Notificationid').val();
    if (notifid == undefined)
        notifid = $('#NotificationId').val();
    var IsSortSide = $('#hf_IsSortSide').val();
    var tmpAnalysisId = 0;


    if (IsSortSide != 'SortSideCall') {

        $.ajax({
            url: '../Notification/DeleteXmlContact',
            type: 'POST',
            async: false,
            data: { OrgName: orgName, ContactName: contactName, analysisId: analysisId, notifId: notifid },

            beforeSend: function () {
                startAnimation();
            },

            success: function (data) {
                if (contactName == $('#LoginUser').val() && orgName == $('#LoginOrg').val()) {
                    document.getElementById("addMeAsAffected").checked = false;
                }
                loadPage(data.result.NotifID, data.result.analysisId);
                if (contactName != $('#LoginUser').val() && orgName != $('#LoginOrg').val()) {
                    document.getElementById("addMeAsAffected").checked = true;
                }
            },
            error: function (xhr, textStatus, errorThrown) {

                stopAnimation();

            },
            complete: function () {
                stopAnimation();
                $('.loading').hide();
                $("#overlay").hide();
            }

        });
    }
    else {
        analysisId = $('#hf_AnalysisIdManual').val();
        //Sort side call
        var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
        var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
        var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
        var MovLatestVer = $('#MovLatestVer').val();
        var MovVersion = $('#versionno').val();
        if ((chk_status == 301002 || chk_status == 301005 || chk_status == 301008) && (sort_user_id != checker_id) && ((MovLatestVer == MovVersion) || MovLatestVer == 0)) {
            ShowErrorPopup('You are not allowed to perform this action during checking process');
        }
        else {
            $.ajax({
                url: '../Notification/DeleteXmlContact',
                type: 'POST',
                //async: false,
                data: { OrgName: orgName, ContactName: contactName, analysisId: analysisId },

                beforeSend: function () {
                    startAnimation();
                },

                success: function (data) {
                    if (contactName == $('#HaulName').val() && orgName == $('#HaulOrganisationName').val()) {
                        document.getElementById("SortaddMeAsAffected").checked = false;
                    }
                    $.post('/Notification/GetSortAffectedParties?analysisId=' + analysisId, function (data) {
                        $('#ManuallyAddedContact').html(data);
                    });
                    //loadSortSideRAafterDelete(analysisId);
                },
                error: function (xhr, textStatus, errorThrown) {
                    stopAnimation();
                },
                complete: function () {
                    stopAnimation();
                    $('.loading').hide();
                    $("#overlay").hide();
                }
            });
        }
    }
}
//#endregion

//#region
function loadPage(notifId, analysisId) {

    $.ajax({
        url: '../Notification/ManualAddedParties',
        type: 'POST',
        async: false,
        data: { NotifID: notifId, analysisId: analysisId, flag: 1 },

        beforeSend: function () {
            startAnimation();
        },

        success: function (data) {
            $('#ManuallyAddedContact').html('');
            $('#notif_section').html('');
            $('#ManuallyAddedContact').html(data);
        },
        error: function (xhr, textStatus, errorThrown) {

        },
        complete: function () {
            stopAnimation();
        }
    });
}
//#endregion

//#region
function DisplayContact(id) {
    removescroll();

    var randomNumber = Math.random();
    $("#overlay").show();
    startAnimation();
    $("#dialogue").html('');
    //$("#dialogue").load("../Application/ViewContactDetails?ContactId=" + id + "&RandomNumber=" + randomNumber);
    //$("#dialogue").show();
    $("#dialogue").load('../Application/ViewContactDetails?ContactId=' + id + "&RandomNumber=" + randomNumber, function () {
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
        $('#contactDetails').modal('show');
        stopAnimation();
    });
}
//#endregion

//#region
function AddContactPopUp() {
    var isSort = true;
    var analysisId = $('#hf_AnalysisIdManual').val();
    if ($('#hf_IsSortSide').val() == 'SortSideCall')
        isSort = false;

    //$("#General").load('../Notification/PopUpAddressBook?NotifID=0' + '&analysisId=' + analysisId + '&fromSort=' + isSort);
    //$('#leftpanel').hide();
    //$('#route-assessment').hide();
    //$("#General").show();

    $.ajax
        ({
            type: "POST",
            url: "../Notification/PopUpAddressBook",
            data: { analysisId: analysisId, fromSort: isSort },
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                SubStepFlag = 5.1;
                var filters = $(page).find('div#filterDivAddressBook');
                $('#General').html(page);
                $('#General').find('div#filterDivAddressBook').remove();
                $('#leftpanel').hide();
                $('#route-assessment').hide();
                $(filters).insertAfter('#banner');
                $("#General").show();
                $("#backbutton").hide();
            },
            error: function (xhr, status, error) {

            },
            complete: function () {
                stopAnimation();
            }
        });

}
//#endregion

//List Affected party after deleting

function loadSortSideRAafterDelete(analysisId) {
    $.ajax
        ({
            type: "POST",
            url: "../SORTApplication/AffectedParties",
            data: { analysisID: analysisId, anal_type: 7 },
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {

                $('#RouteAssesment').html(page);

            },
            error: function (xhr, status, error) {

            },
            complete: function () {
                stopAnimation();
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
            }
        });
}

function Deleteallmannuallyaddedparties(orgName, contactName) {

    analysisId = $('#hf_AnalysisIdManual').val();
    //Sort side call
    $.ajax({
        url: '../Notification/DeleteXmlContact',
        type: 'POST',
        //async: false,
        data: { OrgName: orgName, ContactName: contactName, analysisId: analysisId },

        beforeSend: function () {
            startAnimation();
        },

        success: function (data) {
            //loadSortSideRAafterDelete(analysisId);
            if (contactName == $('#HaulName').val() && orgName == $('#HaulOrganisationName').val()) {
                document.getElementById("SortaddMeAsAffected").checked = false;
            }
            $.post('/Notification/GetSortAffectedParties?analysisId=' + analysisId, function (data) {
                $('#ManuallyAddedContact').html(data);
            });

        },
        error: function (xhr, textStatus, errorThrown) {
            stopAnimation();
        },
        complete: function () {
            stopAnimation();
            $('.loading').hide();
            $("#overlay").hide();
        }
    });
}
function AddMeAsAffectedbyDefault() {
    var analysisId = @ViewBag.AnalysisIdManual;
    var notifid = $('#NotificationId').val();
    $.ajax(
        {
            url: '../Contact/AffectedContactDetail',
            type: 'POST',
            async: false,
            data: { ContactID: 0, NotifID: notifid, analysisId: analysisId, NotifyingContact: true },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {

                loadPage(data.result.NotifID, data.result.analysisId);
                document.getElementById("addMeAsAffected").checked = true;
            },
            error: function (xhr, textStatus, errorThrown) {
                stopAnimation();
            },
            complete: function () {
                stopAnimation();
                $("#overlay").hide();
            }
        });
}

