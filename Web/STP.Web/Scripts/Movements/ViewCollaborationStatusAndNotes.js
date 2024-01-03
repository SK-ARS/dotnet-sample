
var getValue = 0;
var routeStatus = 0;
var mailedCollab = 0;
var userid = $("#hf_User_id").val();
var latestversion = $("#hf_LatestVersion").val();
var Notificationid = $("#hf_Notificationid").val();
var encryptNotiId = $("#hf_encryptNotiId").val();
var eRoute = $("#hf_encryptRoute").val();
var EnEsdalRef = $("#hf_EncryptEsdalRef").val();
var InBoxId = $("#hf_InBoxId").val();
var WipNENCollab = $("#hf_WipNENCollab").val();
var email = $("#hf_eMail").val();

function ViewCollaborationStatusAndNotesInit() {
    $("#importButton").show();
    getValue = 0;
    routeStatus = 0;
    mailedCollab = 0;
    userid = $("#hf_User_id").val();
    latestversion = $("#hf_LatestVersion").val();
    Notificationid = $("#hf_Notificationid").val();
    encryptNotiId = $("#hf_encryptNotiId").val();
    eRoute = $("#hf_encryptRoute").val();
    EnEsdalRef = $("#hf_EncryptEsdalRef").val();
    InBoxId = $("#hf_InBoxId").val();
    WipNENCollab = $("hf_WipNENCollab").val();
    email = $('#hf_eMail').val();
    $('#updateExternalNote').hide();
    $('#ValidAgreement').hide();
    $('#ValidLatest').hide();
    $('#ValidStatus').hide();
    $('#CollabValidStatus').hide();
    //set status
    if ($('#COL_STATUS').val() > 0) {
        var num = $('#COL_STATUS').val();
        num = Math.round(num * 100) / 100;
        $("input[name=statusradiogroup][value=" + num + "]").attr('checked', 'checked');
    }
    else { $("input[name=statusradiogroup][value=" + 313007 + "]").attr('checked', 'checked'); }

    if (parseInt($('#hf_User_id').val()) > 0) {
        $("#dropNEN").val(userid);
        getValue = $('#dropNEN').val();
    }
    else {
        $("#dropNEN").val('');
    }
    if ($("#Radio3").prop("checked")) {
        $('#tr_showscrutiny').show();
        routeStatus = 911009;
        //$('#SendMailBy').hide();
        //$('#div_emaildescp').hide();
    }
    if ($("#Radio1").prop("checked")) {
        $('#tr_showscrutiny').hide();
        routeStatus = 911007;
        //$('#SendMailBy').show();
        //$('#div_emaildescp').show();
    }
    if ($("#Radio2").prop("checked")) {
        $('#tr_showscrutiny').hide();
        routeStatus = 911008;
        //$('#SendMailBy').show();
        //$('#div_emaildescp').show();
    }
}

$(document).ready(function () {
    $('body').on('click', '#span-close', function () {
        $('#overlay').hide();
        addscroll();
        resetdialogue();
    });

    //----NEN portion-------
    $('body').on('change', '#dropNEN', function () {
        getValue = $(this).val();
    });
    
    $('body').on('click', '.view-collab-modal-save', function () {
        SendEmail(false);
        return false;
    });
    $('body').on('click', '.SendMailBy', function () {
        SendEmail(true);
        return false;
    });

    //$('body').on('click', '.view-collab-modal-save' , SendEmail);
    $('body').on('click', '.view-collab-modal-cancel, view-collab-modal-close', closeViewCollab);

    $('body').on('click', '.view-collab-modal-close', closeViewCollab);


    $('body').on('click', '#btn_importfromassessment', function (e) {
        var analysisId = $(this).data("analysisid");

        ImportAssessmentResult(analysisId);
        return false;
    });
    $('body').on('click', '#btnImportfrmLibrary', function (e) {
        var doc_id = $(this).data("doc_id");

        ImportFromLibrary(doc_id);
        return false;
    });
    //----------------------
    $('body').on('change', 'input[name=statusradiogroup]', function () {
        if ($(this).val() == 327003) {
            $('#tr_showscrutiny').show();
            if ($('#hf_User_id').val() != '') {
                if (parseInt($('#hf_User_id').val()) > 0 && parseInt($('#COL_STATUS').val()) != 327004)
                    $("#dropNEN").val(userid);
                else
                    $("#dropNEN").val('');
            }
            routeStatus = 911009;
        } else {
            $('#tr_showscrutiny').hide();
            if ($(this).val() == 327001) {
                routeStatus = 911007;
            } else {
                routeStatus = 911008;
            }
        }
    });
});

//function for closing popup
function closemodal() {
    $('#overlay').hide();
    addscroll();
    resetdialogue();
}
function UpdateStatus() {
    $('#updateExternalNote').hide();
    $('#ValidAgreement').hide();
    $('#ValidLatest').hide();
    $('#ValidStatus').hide();
    $('#CollabValidStatus').hide();
    var status = '';
    status = $('input[name=statusradiogroup]:radio:checked').val();
    //this part is commented by ajit for Removing constraint that forces SOA/,police to only respond to the latest version
    if (latestversion == "0") {

        var EsdalReference = $('#ESDAL_REF_NUMBER').val();
        if (EsdalReference.indexOf('#') > -1) {
            $('#ValidLatest').show();
        }
    }

    if ($('#Statushidden').val() == status && $('#DescriptionExternal').val().trim() == '' && $('#dropNEN').val() == $('#hdnAssignUser').val()) {
        $('#updateExternalNote').show();
        return false;
    }

    //For NEN processing collaboration---------
    if ($('#Statushidden').val() == status && $('#DescriptionExternal').val().trim() == '' && ($('#hNEN_RouteStatus').val() == '911001' || $('#hNEN_RouteStatus').val() == '911002' || $('#hNEN_RouteStatus').val() == '911003' || $('#hNEN_RouteStatus').val() == '911004' || $('#hNEN_RouteStatus').val() == '911005' || $('#hNEN_RouteStatus').val() == '911010' || $('#hNEN_RouteStatus').val() == '911011')) {
        $('#updateExternalNote').show();
        return false;
    }
    //-----------------------------------------
    if (typeof (status) === "undefined") {
        $('#ValidStatus').show();
        return false;
    }

    var to_User = '';
    if ($('#dropNEN').val() != "--Select user--" && $('#dropNEN').val() != '' && $('#dropNEN').val() != undefined) {
        var dropUserNEN = document.getElementById('dropNEN');
        to_User = dropUserNEN.options[dropUserNEN.selectedIndex].text;
    }
    //----------------------
    WipNENCollab = $("hf_WipNENCollab").length > 0 ? $("hf_WipNENCollab").val() : 0;
    var wipNENProcess = WipNENCollab != null ? WipNENCollab:0;
    var MailHaulier = $('#NoteToLibrary').is(":checked");

    var paramList = {
        DocumentId: $('#DOCUMENT_ID').val(),
        InboxId: $('#INBOX_ID').val(),
        STATUS: status,
        ContactId: $('#CONTACT_ID').val(),
        EsdalReference: $('#ESDAL_REF_NUMBER').val(),
        NOTES: $('#DescriptionExternal').val(),
        UserId: getValue,
        NOTIFICATION_ID: Notificationid,
        NenId: $("#hNEN_Id").length > 0 ? $("#hNEN_Id").val() : $('#HFNEN_ID').val(),
        RouteStatus: routeStatus,
        NenProcess: 0,
        WIPNENProcess: wipNENProcess,
        MailedCollab: mailedCollab,
        HaulierName: to_User,
        NenFlag: MailHaulier
    }
    startAnimation();

    $.ajax({
        //async: false,
        type: "POST",
        url: '../Movements/ManageMovementStatus',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: JSON.stringify(paramList),
        processdata: true,
        success: function (result) {
            stopAnimation();
            if (result == 'true') {
                ShowSuccessModalPopup("Status Updated", "CloseSuccessReload");
                if ($('#dropNEN').length > 0)
                    dropUserNEN.options[dropUserNEN.selectedIndex].text = to_User;
                $('#exampleModalCenter2').hide();
                $("#overlay").hide();
            }
            else {
                $('#CollabValidStatus').show();
                $("#overlay").show();
            }
        },
        error: function () {
            stopAnimation();
        },
        complete: function () {
            
            removescroll();
            $('.loading').hide();
        }
    });

}

function DisplayCollabStatus() {
   
    var pageNum = $('#hf_PageNumber').val() ? $('#hf_PageNumber').val() : 1;
    var pageSize = $('#pageSizeVal').val() ? $('#pageSizeVal').val() : 10;
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var versionno = $('#versionno').val();
    $("#overlay").show();
    $('.loading').show();
    var esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    var Notification_Code = $('#Notification_Code').val();
    $("#leftpanel").hide();
    $('#leftpanel_div').hide();
    $('#leftpanel_quickmenu').hide();
    collab = true;
    $.ajax({
        url: '../Notification/CollaborationStatusList',
        data: { page: pageNum, pageSize: pageSize, SORTCollab: true, RefNo: esdal_ref_no },
        type: 'GET',
        cache: false,
        async: false,
        beforesend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#General').html('');
            $('#General').html(page);
            $('#leftpanel').hide();
            removeHLinksTrans();
            PaginateGridTrans();
            fillPageSizeSelect();
            $("#overlay").hide();
            $('.loading').hide();
        },
        error: function () {
        },
        complete: function () {
            stopAnimation();

        }
    });

}

function CloseSuccessReload() {
    $("#popupDialogue").hide();
    CloseSuccessModalPopup();
    startAnimation();
    location.reload();
}

function loadPreviousLocation() {
    var encryptNotificationId = encryptNotiId;
    var encryptRoute = eRoute;

    var encryptEsdalRef = EnEsdalRef;
    var encryptInBoxId = InBoxId;

    var link = "../Movements/AuthorizeMovementGeneral?Notificationid=" + encryptNotificationId + "&esdal_ref=" + encryptEsdalRef + "&route=" + encryptRoute + "&inboxId=" + encryptInBoxId + "";
    //RM#3919 - start
    $('#pop-warning').hide();
    $('#span-close').click();
    $("#overlay").show();
    $('.loading').show();
    //RM#3919 - end
    window.open(link, '_self', false);

}
function RedirectWhenExpire() {
    window.location.href = "../Account/Login";
}

function Reload() {
    WarningCancelBtn();
    $("#overlay").hide();
}

function SendEmail(isMail) {
    if ($('#MailHaulier').is(":checked") || isMail) {
        var collaborationNote = $('#DescriptionExternal').val();

        collaborationNote = collaborationNote.replace(/#/gi, "%23");
        collaborationNote = collaborationNote.replace(/&/gi, "%26");
        collaborationNote = collaborationNote.replace(/ /gi, "%20");

        var restrictCollaborationNote = collaborationNote.substring(0, 1800);

        if (collaborationNote.length > 1800) {
            restrictCollaborationNote = restrictCollaborationNote + "...";
        }

        var emailAddress = $('#HaulierEmailAddress').val();
        if (emailAddress == "" || emailAddress == undefined)
            emailAddress = email;
        var subject = $('#hf_Subject').val();

        if (emailAddress != null && emailAddress != "") {
            window.location.href = "mailto:" + emailAddress + "?subject=" + subject + "&body=" + restrictCollaborationNote + "";
            mailedCollab = 3;
            UpdateStatus();
        }
    }
    else {
        UpdateStatus();
    }
}

function closeViewCollab() {
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
    $("#popupDialogue").hide();
    $('#viewCollabModal').html('');
    $('#viewCollabModal').modal('hide')
    $("#popupDialogue").css("display", "none");
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
    DisplayCollabStatus();
    $("#overlay").css("display", "none");
}
var idStatusGlobal;
function ImportFromLibrary(doc_id) {
    var radioStaus = $('input[name=statusradiogroup]:checked');
    idStatusGlobal = radioStaus.attr('id');
    randomNumber = Math.random();
    startAnimation();
    $("#overlay").show();
    removescroll();
    $("#dialogue").load('../Notification/CollabHistoryPopList?DocumentId=' + 1011210465 + "&randomNumber=" + randomNumber + "&SORTCollab=" + true, function () {
        $('#CollabHistoryDiv').modal({ keyboard: false, backdrop: 'static' });

        $('#CollabHistoryDiv').modal('show');
        stopAnimation();
        $("#dialogue").show();
        $("#popupDialogue").hide();
        $("#overlay").show();
    });

    //resetdialogue();
}

function SaveToLibrary() {
    var Notes = $("#DescriptionExternal").val();

    $.ajax({
        async: false,
        type: "GET",
        url: '../Movements/SaveToLibrary',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: { Notes: Notes, userSchema: "PORTAL" },

        processdata: true,
        success: function (result) {

            if (result == 'true') {
                ShowSuccessModalPopup("Note Saved To Library", "CloseSuccessReload");
            }
            else {
                ShowErrorPopup("Failed");

            }

        },
        error: function () {
        },
        complete: function () {
            removescroll();
            $('.loading').hide();
        }
    });
}

function ImportAssessmentResult(analysisId) {
    var desc = document.getElementById("DescriptionExternal").value;
    $("#DescriptionExternal").load("../RouteAssessment/GetAssessmentResult?analysisID=" + analysisId + "&userSchema=" + "PORTAL", function (data) {
        //$('#contactDetails').modal('show');
        if (data == "") {
            $('#assessmentWarning').text("Assessment results are not available");
        }
        else {
            if (desc == "") {
                document.getElementById("DescriptionExternal").value = data;
            }
            else {
                desc += "\n" + data;
                document.getElementById("DescriptionExternal").value = desc;
            }
        }


        stopAnimation();
        $("#overlay").show();
    });
    //$.ajax
    //    ({
    //        type: "POST",
    //        url: "../RouteAssessment/GetAssessmentResult",
    //        data: { analysisID: analysisId, userSchema: "PORTAL" },
    //        beforeSend: function () {
    //            //startAnimation();
    //        },
    //        success: function (data) {
    //            var desc = document.getElementById("DescriptionExternal").value;
    //            if (desc == "") {
    //                document.getElementById("DescriptionExternal").value = data;
    //            }
    //            else {
    //                desc += "\n" + data;
    //                document.getElementById("DescriptionExternal").value = desc;
    //            }

    //        },
    //        complete: function () {
    //            stopAnimation();
    //        }

    //    });

}
