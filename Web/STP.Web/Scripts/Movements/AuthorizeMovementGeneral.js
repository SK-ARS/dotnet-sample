var SortMovementVal = $('#hf_SortMovement').val();
var contactIdVal = $('#hf_contactId').val();
var organisationIdVal = $('#hf_organisationId').val();
var NotificationidVal = $('#hf_Notificationid').val();
var routeTypeVal = $('#hf_routeType').val();
var RouteFlagVal = $('#hf_RouteFlag').val();
var Content_ref_noVal = $('#hf_Content_ref_no').val();
var sortFlag;
var IS_NEN = $('#HFNEN_ID').val() != null ? $('#HFNEN_ID').val() : 0;
var V_INBOX_ID = $('#HdnInboxID').val();
var vehicleInitialLoad = 0;
var ViewContactlist = 0;
var viewcontactdetailsInitialLoad = 0;
var pageNum = 1;
var pageSize = 10;
var Helpdesk_redirect;
$(document).ready(function () {
    sessionStorage.removeItem('checkboxStates');
    SortMovementVal = $('#hf_SortMovement').val();
    contactIdVal = $('#hf_contactId').val();
    organisationIdVal = $('#hf_organisationId').val();
    NotificationidVal = $('#hf_Notificationid').val();
    routeTypeVal = $('#hf_routeType').val();
    RouteFlagVal = $('#hf_RouteFlag').val();
    Content_ref_noVal = $('#hf_Content_ref_no').val();
    vehicleInitialLoad = 1;
    ViewContactlist = 1;
    viewcontactdetailsInitialLoad = 1;
    Helpdesk_redirect = $('#hf_Helpdesk_redirect').val();
    if (Helpdesk_redirect == "true") {
        $("#SOA").css("display", "none");
        $("#police").css("display", "none");
        $("#user-info-filter").css("display", "none");
    }
    if ($('#hfIsHistoric').val() == 1) {
        $(".ViewCollaboration").css("display", "none");
        $(".ViewCollaborationEdit").css("display", "none");
    }
    if ($("#userid").val() == 696002) {
        $("#ShowAllAffectStruct").val(true);
    }
    IS_NEN = $('#HFNEN_ID').val() != null ? $('#HFNEN_ID').val() : 0;
    V_INBOX_ID = $('#HdnInboxID').val();
    sortFlag = SortMovementVal != null && SortMovementVal != undefined && SortMovementVal != "False" && SortMovementVal != "false" ? true : false;
    $('#HFSortFlag').val(sortFlag);
    $('body').on('load', '#print-container', function (e) {
        window.print();
    });
    $('body').on('click', '.ViewCollaboration', function (e) {
        e.preventDefault();
        ViewCollaboration(this);
    });
    $('body').on('click', '.ViewCollaborationEdit', function (e) {
        e.preventDefault();
        ViewCollaboration(this);
    });
    $('body').on('click', '.ViewContactedPartiesByNotification', function (e) {
        e.preventDefault();
        ViewContactedPartiesByNotification(this);
    });
    $('body').on('click', '.ViewContactedPartiesByProposal', function (e) {
        e.preventDefault();
        ViewContactedPartiesByProposal(this);
    });
    $('body').on('click', '.ViewIndemnityConfirmation', function (e) {
        e.preventDefault();
        ViewIndemnityConfirmation(this);
    });
    $('body').on('click', '.ViewContactedParties', function (e) {
        e.preventDefault();
        ViewContactedParties(this);
    });
    $('body').on('click', '#printbutton', function (e) {
        e.preventDefault();
        printOptions(this);
    });
    $('body').on('click', '.PrintReport', function (e) {
        e.preventDefault();
        PrintReportAutherizeMovementGen(this);
    });
    $('body').on('click', '.PrintReducedReport', function (e) {
        e.preventDefault();
        PrintReducedReportPrintReducedReport(this);
    });
    $('body').on('click', '.viewVehicleSummary', function (e) {
        e.preventDefault();
        viewVehicleSummary(this);
    });
    $('body').on('click', '.vehicleSummary', function (e) {
        e.preventDefault();
        vehicleSummary(this);
    });
    $('body').on('click', '.routeOverview', function (e) {
        e.preventDefault();
        routeOverview(this);
    });
    $('body').on('click', '.routeDescription', function (e) {
        e.preventDefault();
        routeDescriptions(this);
    });
    $('body').on('click', '.routeRoads', function (e) {
        e.preventDefault();
        routeRoads(this);
    });
    $('body').on('click', '.affectedstructure', function (e) {
        e.preventDefault();
        affectedstructure(this);
    });
    $('body').on('click', '.drivingInstruction', function (e) {
        e.preventDefault();
        drivingInstruction(this);
    });
    $('body').on('click', '.notesFromHaulier', function (e) {
        e.preventDefault();
        notesFromHaulier(this);
    });
    $('body').on('click', '.relatedCom', function (e) {
        e.preventDefault();
        relatedCom(this);
    });
    $('body').on('click', '.RelatedCommunication', function (e) {
        e.preventDefault();
        RelatedCommunication(this);
    });
    $('body').on('click', '.notesOnEscort', function (e) {
        e.preventDefault();
        notesOnEscort(this);
    });
    $('body').on('click', '.internalNotes', function (e) {
        e.preventDefault();
        internalNotes(this);
    });
    $('body').on('click', '#SaveNotes', function (e) {
        e.preventDefault();
        SaveNotes(this);
    });
    $('body').on('click', '.ShowContactList', function (e) {
        e.preventDefault();
        ShowContactList(this);
    });
    $('body').on('click', '.predefinedCautions', function (e) {
        e.preventDefault();
        predefinedCautions(this);
    });
    $('body').on('click', '.reviewProcHighway', function (e) {
        e.preventDefault();
        reviewProcHighway(this);
    });
    $('body').on('click', '.showaudit', function (e) {
        e.preventDefault();
        showaudit(this);
    });
    $('body').on('click', '.openDispensationList', function (e) {
        e.preventDefault();
        openDispensationList(this);
    });
    $('body').on('click', '.closeAuthorizeMovFilters', function (e) {
        e.preventDefault();
        closeAuthorizeMovFilters(this);
    });
    $('body').on('click', '.SaveEscortNotes', function (e) {
        e.preventDefault();
        SaveEscortNotes(this);
    });
    $('body').on('change', '#toggleAllAffected', function (e) {
        ToggleAllAffected1();
    });
    $('body').on('click', '#span1-routeview', function (e) {
        e.preventDefault();
        var RouteID = $(this).data('routeid');
        var RouteType = $(this).data('routetype');
        var var1 = $(this).data('var1');
        RouteView(RouteID, RouteType, var1);
    });
    $('body').on('click', '#span2-routeview', function (e) {
        e.preventDefault();
        var RouteID = $(this).data('routeid');
        var RouteType = $(this).data('routetype');
        var var1 = $(this).data('var1');
        RouteView(RouteID, RouteType, var1);
    });
    $('body').on('click', '#span3-routeview', function (e) {
        e.preventDefault();
        var RouteID = $(this).data('routeid');
        var RouteType = $(this).data('routetype');
        var var1 = $(this).data('var1');
        RouteView(RouteID, RouteType, var1);
    });
    SelectMenu(2);
    $('body').on('click', '.toggle-collabsible-panel', function (e) {
        e.preventDefault();
        var targetElem = $(this).data('target');
        if ($('#' + targetElem).is(":visible")) {
            $('#' + targetElem).css("display", "none");
            $(this).find("#up-chevlon").css("display", "none");
            $(this).find("#down-chevlon").css("display", "block");
        }
        else {
            $('#' + targetElem).css("display", "block");
            $(this).find("#up-chevlon").css("display", "block");
            $(this).find("#down-chevlon").css("display", "none");
        }
    });
    if ($('#printoptions').hasClass('clicked')) {
        UnDisplayPrintOptions();
    }
    $('body').on('click', '#view-contact', function (e) {
        e.preventDefault();
        var ContactId = $(this).data('contactid');
        var flag = $(this).data('true');
        ViewContactDetails(ContactId, flag);
    });
    $("#movement-details").click(function () {
        UnDisplayPrintOptions();
    });
    $("#listitem").click(function () {
        UnDisplayPrintOptions();
    });
    $("#MarginTop").click(function () {
        UnDisplayPrintOptions();
    });
    $("#navbar").click(function () {
        UnDisplayPrintOptions();
    });
    $("#banner").click(function (event) {
        if (event.target.id == "banner-container" || event.target.id == "printButtonDiv" || event.target.id == "helpdeskDelegation" || event.target.id == "container-sub" || event.target.id == "movement-map") {
            UnDisplayPrintOptions();
        }
    });
    $("#toggleAllAffected").prop('checked', true);
    ToggleAllAffected1();
    $("#inputRouteDescriptionsNEN").hide();

    setInterval(function () {
        var analysisId = $("#StructAnalysisID").val();
        if (analysisId != 0) {
            DisplayAffectedStructure(analysisId,1)
        }
    }, 30000);

    $('body').on('click', '.NotesToHaulier', function (e) {
        e.preventDefault();
        var targetElem = 'NotesToHaulier';
        if ($('#' + targetElem).is(":visible")) {
            $('#' + targetElem).css("display", "none");
            $(this).find("#up-chevlon").css("display", "none");
            $(this).find("#down-chevlon").css("display", "block");
        }
        else {
            $('#' + targetElem).css("display", "block");
            $(this).find("#up-chevlon").css("display", "block");
            $(this).find("#down-chevlon").css("display", "none");
        }
    });
});
function UnDisplayPrintOptions() {
    if (document.getElementById('printOptions').style.display == "block") {
        document.getElementById('printOptions').style.display = "none"
        $("#MarginTop").css('margin-top', 0);
        $("#printOptions").css("z-index", "0");
    }
}
function ToggleAllAffected1() {
    if ($("#toggleAllAffected").prop('checked') == true) {
        $('#toggleAllAffected').val(true);
        document.getElementById('All').checked = true;
        document.getElementById('Affected').checked = false;
        $("#All").change();
        $("#Affected").change();

    }
    else if ($("#toggleAllAffected").prop('checked') == false) {
        $('#toggleAllAffected').val(false);
        document.getElementById('Affected').checked = true;
        document.getElementById('All').checked = false;
        $("#All").change();
        $("#Affected").change();
    }
}
function ViewIndemnityConfirmation(data) {
    var ID = $(data).attr("notificationid");
    var StartTime = $(data).attr("movestartdate");
    var EndTime = $(data).attr("moveenddate");
    startAnimation();
    var indemnity = {};
    indemnity.NotificationId = ID;
    indemnity.FirstMoveDate = StartTime;
    indemnity.LastMoveDate = EndTime;

    $("#dialogue").load("../Application/ViewIndemnityConfirmation", { indemnityConfirmation: indemnity }, function () {/*?NotificationId = " + ID + " & random=" + random + "*/
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
        $('#contactDetails').modal('show');
        IndemntityConfirmationInit();
    });
}
function IndemntityConfirmationInit() {
    stopAnimation();
    Resize_PopUp(540);
    $('body').on('click', '#IDcloseMp', function (e) {
        e.preventDefault();
        closeMp(this);
    });
    $('body').on('click', '#span-close', function (e) {
        e.preventDefault();
        $('#overlay').hide();
        addscroll(this);
        resetdialogue(this);
    });
    $('body').on('click', '#PrintPage', function (e) {
        e.preventDefault();
        var link = "../Movements/IndemnityConfirmation?notificationId=" + $('#NotificationId').val() + "";
        window.open(link, '_blank');
    });
    removescroll();
    $("#dialogue").show();
    $("#overlay").show();
}
function ViewCollaboration(_this) {
    var id = $(_this).attr("documentid");
    var inboxId = $(_this).attr("inboxid");
    var analysisId = $(_this).attr("analysisid");
    var randomNumber = Math.random();
    var nenId = $("#hNEN_Id").length > 0 ? $("#hNEN_Id").val() : $('#HFNEN_ID').val();
    var loggedInContactId = contactIdVal;
    var loggedInOrgId = organisationIdVal;
    var IS_MOST_RECENT = $('#IS_MOST_RECENT').val();

    //RM#3919 - add routeOriginal parameter;
    var NextNoLongerAffected = $('#hf_NextNoLongerAffected').val();
    startAnimation();

    $("#popupDialogue").load("../Movements/ViewCollaborationStatusAndNotes?random=" + randomNumber + "&Notificationid=" + NotificationidVal + "&documentid=" + id + "&InboxId=" + inboxId + "&analysisId=" + analysisId + "&email=" + encodeURIComponent($('#EMail').val().replace(/ /g, '+')) + "&esdalRef=" + encodeURIComponent($('#ESDAL_Reference').val()) + "&contactId=" + loggedInContactId + "&route=" + encodeURIComponent(routeTypeVal.replace(" ", "")) + "&IS_MOST_RECENT=" + IS_MOST_RECENT + "&routeOriginal=" + encodeURIComponent(routeTypeVal.replace(" ", "_")) + "" + "&NextNoLongerAffected=" + NextNoLongerAffected, function () {
        stopAnimation();
        $('#viewCollabModal').modal({ keyboard: false, backdrop: 'static' });
        $("#viewCollabModal").modal('show');
        $("#overlay").css("display", "block");
        $("#popupDialogue").css("display", "block");
        ViewCollaborationStatusAndNotesInit();
    });

}
function viewVehicleSummary() {
    if (document.getElementById('vehiclesummary').style.display !== "none") {
        document.getElementById('vehiclesummary').style.display = "none"
        document.getElementById('up-chevlon6').style.display = "none"
        document.getElementById('down-chevlon6').style.display = "block"
    }
    else {
        document.getElementById('vehiclesummary').style.display = "block"
        document.getElementById('up-chevlon6').style.display = "block"
        document.getElementById('down-chevlon6').style.display = "none"
    }
}
// view vehicle configuration summary
function vehicleSummary(data) {
    var id = $(data).attr("vehicleid");
    var classCode = $(data).attr("vehicleclassification");
    startAnimation();
    var vehicleId = Math.round(id);
    var IsLoaded = $("#Vehicle_" + vehicleId).val();
    if (IsLoaded == "true") {
        stopAnimation();
        if ($('#vehicleconfig_' + vehicleId).css('display') !== 'none') {
            $("#vehicleconfig_" + vehicleId).css("display", "none");
            $("#chevlon-up-icon_" + vehicleId).css("display", "none");
            $("#chevlon-down-icon_" + vehicleId).css("display", "block");
        }
        else {
            $("#vehicleconfig_" + vehicleId).css("display", "block");
            $("#chevlon-up-icon_" + vehicleId).css("display", "block");
            $("#chevlon-down-icon_" + vehicleId).css("display", "none");
        }
    }
    else {
        //stopAnimation();
        viewVehicleDetails(vehicleId, classCode);
        ViewConfigurationInit();
    }
}
function ShowContactList(_this) {
    var id = $(_this).attr("analysisid");
    if (ViewContactlist == 1) {
        ContactList(id);
    }
    else {
        if ($('#contact-list').css('display') !== 'none') {
            $("#contact-list").css("display", "none");
            $("#down-chevlon-icon2").css("display", "block");
            $("#up-chevlon-icon2").css("display", "none");
        }
        else {
            $("#contact-list").css("display", "block");
            $("#down-chevlon-icon2").css("display", "none");
            $("#up-chevlon-icon2").css("display", "block");
        }
    }
}
function ContactList(ID, sortOrder = 1, sortType = 0, pageNum = 1, pageSize = 10) {
    var random = Math.random();
    var notiId = $('#hf_Notificationid').val();

    var routeDetails = routeTypeVal;

    var notificationId = $('#NOTIFICATION_ID').val();
    var esdalRefNumber = $('#ESDAL_Reference').val();
    startAnimation();

    //RM#3919 - add toLowerCase()
    routeDetails = routeDetails.toLowerCase().replace("amendment to agreement", "amendment");
    routeDetails = routeDetails.toLowerCase().replace("no longer affected", "nolongeraffected");
    routeDetails = routeDetails.toLowerCase().replace("ne agreed notification", "ne notification");//Added For NEN project
    routeDetails = routeDetails.toLowerCase().replace("ne renotification", "ne renotification");//Added For NEN project
    routeDetails = routeDetails.toLowerCase().replace("ne agreed renotification", "ne renotification");//Added For NEN project


    $.ajax({
        url: '../Contact/AuthoriseMovementShowContactList',
        type: 'POST',
        cache: false,
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        data: {
            analysisID: ID, NotificationID: notificationId, ESDALRefNumber: esdalRefNumber, Type: routeDetails, random: random,
            sortOrder: sortOrder, sortType: sortType, page: pageNum, pageSize: pageSize
        },
        success: function (response) {
            stopAnimation();
            var result = $(response).find('div#contact-list').html();
            $('div#contact-list').html(result);
            ViewContactlist = 0;
            $("#contact-list").css("display", "block");
            $("#down-chevlon-icon2").css("display", "none");
            $("#up-chevlon-icon2").css("display", "block");
        }
    });
    removeHrefContactLinks();
    PaginateListContact();
}
function ContactSorting(event, param) {
    sortOrder = param;
    sortType = event.classList.contains('sorting_asc') ? 1 : 0;
    var hAnalysisId = $('#hAnalysisId').val();
    pageNum = pageNum;
    pageSize = pageSize;
    var pageSize = $('#pageSizeVal').val();
    var pageNum = $('#pageNum').val()
    ContactList(hAnalysisId, sortOrder, sortType, pageNum, pageSize);
}
// view vehicle configuration summary
function viewVehicleDetails(id, classCode) {
    //startAnimation();
    $.ajax({
        url: '../VehicleConfig/ViewConfigDetails',
        type: 'POST',
        cache: false,
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        data: {
            vehicleID: id, movementId: 0, isRoute: true, flag: "VR1", isPolice: true, isNotif: NotificationidVal != null && NotificationidVal != undefined ? true : false
        },
        success: function (response) {
            var result = $(response).find('div#vehicleconfig').html();
            $('div#vehicleconfig_' + id).html(result);
            vehicleInitialLoad = 0;
            $("#Vehicle_" + id).val("true");
            $("#vehicleconfig_" + id).css("display", "block");
            $("#chevlon-up-icon_" + id).css("display", "block");
            $("#chevlon-down-icon_" + id).css("display", "none");
            stopAnimation();
        }
    });
}
// View Contacted Parties By Notification
function ViewContactedPartiesByNotification(data) {
    var id = $(data).attr("notificationid");
    startAnimation();

    var isNEN = 0;
    if ($('#HFNEN_ID').val() != 0) { isNEN = 1; }
    if (viewcontactdetailsInitialLoad == 1) {
        $("#popupDialogue").load("../Application/GetHaulierContactDetails", { notificationNumber: id, isNEN: isNEN }, function () {
            stopAnimation();
            $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
            $("#popupDialogue").show();
            $("#overlay").show();
            $('#contactDetails').modal('show');
        });
    }
    else {
        if (document.getElementById('viewcontactdetails').style.display !== "none") {
            document.getElementById('viewcontactdetails').style.display = "none"
            document.getElementById('down-chevlon-icon2').style.display = "none"
            document.getElementById('up-chevlon-icon2').style.display = "block"
        }
        else {
            document.getElementById('viewcontactdetails').style.display = "block"
            document.getElementById('down-chevlon-icon2').style.display = "block"
            document.getElementById('up-chevlon-icon2').style.display = "none"
        }
    }
}
function ViewContactedPartiesByProposal(_this) {
    //removescroll();
    var versioId = $(_this).data("versionid");
    var revisionId = $(_this).data("revisionid");
    var random = Math.random();
    startAnimation();

    $("#dialogue").load("../Application/GetHaulierContactDetailsForProposal?versionId=" + versioId + "&revisionId=" + revisionId, function () {
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
        $("#dialogue").css("display", "block");
        $('#contactDetails').modal('show');
        stopAnimation();
        $("#overlay").css("display", "block");
    });


}
function ViewContactedParties(_this) {
    var ID = $(_this).attr("hacontactid");
    removescroll();
    var random = Math.random();
    $("#dialogue").load("../Application/ViewContactDetails?ContactId=" + ID + "&random=" + random, function () {
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
        $("#dialogue").css("display", "block");
        $('#contactDetails').modal('show');

        stopAnimation();
        $("#overlay").css("display", "block");
    });

}
function notesOnEscort() {
    if (document.getElementById('notesOnEscort').style.display !== "none") {
        document.getElementById('notesOnEscort').style.display = "none"
        document.getElementById('up-chevlon1').style.display = "none"
        document.getElementById('down-chevlon1').style.display = "block"
    }
    else {
        $('#updateEscortNotes').hide();
        document.getElementById('notesOnEscort').style.display = "block"
        document.getElementById('up-chevlon1').style.display = "block"
        document.getElementById('down-chevlon1').style.display = "none"
    }
}
function internalNotes() {
    if (document.getElementById('internalNotes').style.display !== "none") {
        document.getElementById('internalNotes').style.display = "none"
        document.getElementById('up-chevlon2').style.display = "none"
        document.getElementById('down-chevlon2').style.display = "block"
    }
    else {
        $('#updatenotes').hide();
        document.getElementById('internalNotes').style.display = "block"
        document.getElementById('up-chevlon2').style.display = "block"
        document.getElementById('down-chevlon2').style.display = "none"
    }
}
function notesFromHaulier() {
    if (document.getElementById('notesFromHaulier').style.display !== "none") {
        document.getElementById('notesFromHaulier').style.display = "none"
        document.getElementById('up-chevlon3').style.display = "none"
        document.getElementById('down-chevlon3').style.display = "block"
    }
    else {
        document.getElementById('notesFromHaulier').style.display = "block"
        document.getElementById('up-chevlon3').style.display = "block"
        document.getElementById('down-chevlon3').style.display = "none"
    }
    stopAnimation();
}
function predefinedCautions() {
    if (document.getElementById('predefinedCautions').style.display !== "none") {
        document.getElementById('predefinedCautions').style.display = "none"
        document.getElementById('up-chevlon4').style.display = "none"
        document.getElementById('down-chevlon4').style.display = "block"
    }
    else {
        document.getElementById('predefinedCautions').style.display = "block"
        document.getElementById('up-chevlon4').style.display = "block"
        document.getElementById('down-chevlon4').style.display = "none"
    }
}
function relatedCom() {

    if (document.getElementById('relatedCom').style.display !== "none") {
        document.getElementById('relatedCom').style.display = "none"
        document.getElementById('up-chevlon5').style.display = "none"
        document.getElementById('down-chevlon5').style.display = "block"
    }
    else {
        document.getElementById('relatedCom').style.display = "block"
        document.getElementById('up-chevlon5').style.display = "block"
        document.getElementById('down-chevlon5').style.display = "none"
    }
}
function SaveEscortNotes() {
    $('#validateEscortNotes').hide();
    $('#updateEscortNotes').hide();
    var inboxId = $('#hf_InboxId').val();
    var esdalRefNo = $('#ESDAL_Reference').val();
    if ($('#DescriptionOnEscort').val().trim() == '') {
        $('#validateEscortNotes').show();
        return false;
    }
    startAnimation();
    var paramList = {
        NotificationId: $('#NOTIFICATION_ID').val(),
        RevisionId: $('#REVISION_ID').val(),
        NotesOnEscort: $('#DescriptionOnEscort').val(),
        InboxId: inboxId,
        NotificationCode: esdalRefNo
    }

    $.ajax({
        async: false,
        type: "POST",
        url: '../Movements/ManageNotesOnEscort',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: JSON.stringify(paramList),
        processdata: true,
        success: function (result) {
            if (result == 'true') {
                $('#updatedEscortNotes').html('');
                $('#updatedEscortNotes').html($('#DescriptionOnEscort').val());
                $('#updateEscortNotes').show();
            }
            else if (result == 'expire') {
                RedirectWhenExpire();
            }
            else if (result == 'false') {
                RedirectWhenExpire();
            }
        },
        error: function () {
        },
        complete: function () {
            stopAnimation();
        }
    });

}
function SaveNotes() {

    $('#validateInternalNotes').hide();
    $('#updatenotes').hide();
    var inboxId = $('#hf_InboxId').val();
    var esdalRefNo = $('#ESDAL_Reference').val();
    if ($('#Description').val().trim() == '') {
        $('#validateInternalNotes').show();
        return false;
    }

    startAnimation();

    var paramList = {
        DocumentId: $('#DOCUMENT_ID').val(),
        RevisionId: $('#REVISION_ID').val(),
        InternalNotes: $('#Description').val(),
        InboxId: inboxId,
        NotificationCode: esdalRefNo
    }

    $.ajax({
        async: false,
        type: "POST",
        url: '../Movements/ManageInternalNotes',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: JSON.stringify(paramList),
        processdata: true,
        success: function (result) {

            if (result == 'true') {
                $('#updatenotes').show();
            }
            else if (result == 'expire') {
                RedirectWhenExpire();
            }
            else if (result == 'false') {
                RedirectWhenExpire();
            }
        },
        error: function () {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function ViewContactDetails(ContactId, isClosed) {
    var random = Math.random();
    startAnimation();
    $("#dialogue").load('../Application/ViewContactDetails?ContactId=' + ContactId + "&random=" + random, function () {
        stopAnimation();
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
        $("#dialogue").css("display", "block");
        $('#contactDetails').modal('show');
        stopAnimation();
        $("#overlay").show();
    });

}
function RelatedCommunication(data) {
    var NotificationId = $(data).attr("notificationid");
    var EsdalRefNumber = $(data).attr("notificationcode");
    var EncryptedRefNumber = $(data).attr("encryptesdalreference");
    startAnimation();
    var link = "";
    $.ajax({
        async: false,
        type: "GET",
        url: '../Movements/GetInboxDetails',
        dataType: "json",
        data: { esdalRefNumber: EsdalRefNumber },
        //contentType: "application/json; charset=utf-8",
        processdata: true,
        beforeSend: function () {

        },
        success: function (result) {
            var resultArray = result.split('|');

            var encryptInBoxId = resultArray[0];

            var encryptRoute = resultArray[1];
            var Status = resultArray[2];
            var NEN_ID = resultArray[3];
            var StatusName = resultArray[4];

            //check added for whether the notification is confirmed or not
            if (Status == "NE Notification" || Status == "NE Renotification") {
                link = "/NENNotification/NE_Notification?NEN_ID=" + NEN_ID + "&Notificationid=" + NotificationId + "&esdal_ref=" + EncryptedRefNumber + "&route=" + encryptRoute + "&inboxId=" + encryptInBoxId + "&inboxItemStat=" + StatusName + "";

            }
            else {
                link = "/Movements/AuthorizeMovementGeneral?Notificationid="+ NotificationId +"&esdal_ref=" + EncryptedRefNumber + "&route=" + encryptRoute + "&inboxId=" + encryptInBoxId + "&inboxItemStat=" + Status + "&FromInboxflag=" + 1 + "";
            }
        },
        error: function () {
        },
        complete: function () {
            stopAnimation();
        }
    });

    window.location.href = link;
    console.log(link);
}
function showaudit() {
    if (document.getElementById('showaudit').style.display !== "none") {
        document.getElementById('showaudit').style.display = "none"
        document.getElementById('up-chevlon8').style.display = "none"
        document.getElementById('down-chevlon8').style.display = "block"
    }
    else {
        document.getElementById('showaudit').style.display = "block"
        document.getElementById('up-chevlon8').style.display = "block"
        document.getElementById('down-chevlon8').style.display = "none"
        ShowAuditLog();
    }
}
function openDispensationList() {
    if (document.getElementById('dispensations').style.display !== "none") {
        document.getElementById('dispensations').style.display = "none"
        document.getElementById('up-chevlon3').style.display = "none"
        document.getElementById('down-chevlon3').style.display = "block"
    }
    else {
        document.getElementById('dispensations').style.display = "block"
        document.getElementById('up-chevlon3').style.display = "block"
        document.getElementById('down-chevlon3').style.display = "none"
    }
    stopAnimation();
}
function ShowAuditLog() {
    SelectNENAuditLog(pageNum, pageSize);
}
function SelectNENAuditLog(pageNum, pageSize) {
    var NEN_Notif = $('#ESDAL_Reference').val();
    startAnimation();
    $("#showaudit").load('../Logging/NENAuditLogPopup?pageNum=' + pageNum + '&pageSize=' + pageSize + '&NEN_Notif_No=' + NEN_Notif, {},
        function () {
            NENAuditLogpopupInit();
        });
}
function affectedstructure(_this) {
    var analysisId = $(_this).attr("analysisid");
    if (document.getElementById('affectedstructure').style.display !== "none") {
        document.getElementById('affectedstructure').style.display = "none"
        document.getElementById('up-chevlon10').style.display = "none"
        document.getElementById('down-chevlon10').style.display = "block"
    }
    else {
        document.getElementById('affectedstructure').style.display = "block"
        document.getElementById('up-chevlon10').style.display = "block"
        document.getElementById('down-chevlon10').style.display = "none"
        if (analysisId != 0) {
            DisplayAffectedStructure(analysisId)
        }
    }
}
function DisplayAffectedStructure(analysisId,flag=0) {
    var revisionId = $('#REVISION_ID').val();
    var sortFlag = SortMovementVal != null && SortMovementVal != undefined && SortMovementVal != "False" && SortMovementVal != "false" ? true : false;;
    var showall = $("#ShowAllAffectStruct").val();
    $("#Analysis_id").val(analysisId);
    var test = $("#showallstruct").val();
    var selectedRadio = null;
    if(flag==1)
        selectedRadio = $('input[type=radio][name=route_select]:checked').attr('value');
    if (test == 'on' && firsttime < 1) {

        $("#Firsttime").val("Firsttime");

        firsttime = firsttime + 2;
        var a = $("#StructAnalysisID").val();
        if ($("#showallstruct").is(":checked")) {
            $("#ShowAllAffectStruct").val(true);
            DisplayAffectedStructure(a);
        }
        else {
            $("#ShowAllAffectStruct").val(false);
            DisplayAffectedStructure(a);
        }
    }
    else {

        $("#Firsttime").val("secondTime");
        if (flag == 0)
            startAnimation();
        $("#Affected_Struct_Auth_Mov").show();
        $("#structcheckbox").show();
        //ShowAllStruct = true - hardcoded for demo. it will be removed after implementing filter
        $.ajax({
            type: "GET",
            url: "../RouteAssessment/ListAffectedStructures",
            //contentType: "application/json; charset=utf-8",
            data: { analysisID: analysisId, revisionId: revisionId, SORTflag: sortFlag, isAuthMove: true, ShowAllStruct: showall },
            datatype: "json",
            success: function (result) {
                $("#affectedstructure").html(result);
                $("#Affected_Structures").show();
                if (flag == 0) {
                    $("#StructureGenDet").hide();
                    $("#Driving_Instruction").hide();
                    $("#AffectedRoads").hide();// div related to affected roads
                    $("#GeneralDetails").hide();
                    $("#BackAfftStruct").hide();
                    stopAnimation();
                }
                ListAffectedStructuresInit(selectedRadio);
            },
            error: function () {
                location.reload();
            }
        });
    }
}
function printOptions() {
    buttonClicked = 1;
    if (document.getElementById('printOptions').style.display !== "none") {
        document.getElementById('printOptions').style.display = "none";
        $("#printOptions").removeClass('clicked');
        $("#MarginTop").css('margin-top', 0);
        $("#printOptions").css("z-index", "0");
    }
    else {
        document.getElementById('printOptions').style.display = "block"
        $("#printOptions").css("z-index", "1");
        $("#printOptions").addClass('clicked');
        var val = $("#DivHeight").val(); //by id
        if (val == 1) {
            $("#MarginTop").css('margin-top', -80);
        }
        else {
            $("#MarginTop").css('margin-top', -40);
            $("#printOptions").css('height', 40);
        }
    }
}
function ViewGPXRoute() {
    var routeId = $("#RouteId").val();
    var notifId = $("#NOTIFICATION_ID").val();
    var movementType = 207003;
    if ($("#gpxRoute").prop('checked') == true) {
        //New function to show GPX geometry using Geoserver
        ShowGpxRoute(notifId, routeId, movementType);
    }
    else if ($("#gpxRoute").prop('checked') == false) {
        //New function to hide GPX geometry using Geoserver
        HideGpxRoute();
    }
}
function RouteView(RouteId, RouteType, VIsSortPortal) {
    $("#GeneralDetails").hide();
    $("#ShowDetail").show();
    $("#map").empty();
    $("#leftpanel").empty();
    $('#leftpanel').empty();
    $("#RouteId").val(RouteId);
    $("#RouteType").val(RouteType);
    $('#Starting').text('')
    $('#Ending').text('')
    $('#Tabvia').empty();
    if ($("#RouteName").val().toLowerCase() == 'ne notification api' ||
        $("#RouteName").val().toLowerCase() == 'ne renotification api' ||
            $("#RouteName").val().toLowerCase() == 'ne agreed notification'||
            $("#RouteName").val().toLowerCase() == 'ne agreed renotification')
    {
        $("#showGPXRoute").show();
        $("#movementMapFilterIcon").addClass('mapFilterIconMargin');
        $("#inputRouteDescriptionsNEN").show();
    }
    var str = '';
    if (RouteType != "")
        str = RouteType;
    var value = ".move-map-" + RouteId;
    $(value).prop('checked', true);
    var HfRouteType = $('#HfRouteType').val();
    var hf_IsNen = $('#hf_IsNen').val();
    var IsAuthorizeMovementGeneral = $('#AuthorizeMovementGeneral').val();
    var historic = $('#hfIsHistoric').val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        //  async: false,
        url: '../Routes/GetPlannedRoute',
        data: { RouteID: RouteId, routeType: str, IsSortPortal: VIsSortPortal, IsNEN: hf_IsNen, IsPlanMovement: $("#hf_IsPlanMovmentGlobal").length > 0, IsCandidateView: IsCandidateRouteView(), IsAuthorizeMovementGeneral: IsAuthorizeMovementGeneral, IsHistoric: historic },
        beforeSend: function (xhr) {
            openContentLoader('#map');
        },
        success: function (result) {
            if (result.result != null) {
                var count = result.result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;
                for (var x = 0; x < result.result.routePathList.length; x++) {
                    result.result.routePathList[x].routePointList.sort((a, b) => a.routePointId - b.routePointId);
                }
                for (var i = 0; i < count; i++) {
                    if (result.result.routePathList[0].routePointList[i].pointGeom != null || result.result.routePathList[0].routePointList[i].linkId != null)
                        flag = 1;
                }
                if (flag == 1 || RouteFlagVal == 1 || RouteFlagVal == 3) {
                    $('#Tabvia').empty();
                    $('#trVia').hide();
                    $("#ShowDetail").show();
                    $("#RouteName").text(result.result.routePartDetails.routeName);
                    if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                        $("#RouteDesc").text(result.result.routePartDetails.routeDescr);
                    for (var i = 0; i < count; i++) {
                        if (result.result.routePathList[0].routePointList[i].pointType == 0)
                            $('#Starting').text(result.result.routePathList[0].routePointList[0].pointDescr);
                        else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                            $('#Ending').text(result.result.routePathList[0].routePointList[i].pointDescr);

                        else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                            Index = Index + 1;
                            strTr = "<div >" + Index + " . " + result.result.routePathList[0].routePointList[i].pointDescr + "</div>"
                            $('#Tabvia').append(strTr);
                            $('#trVia').show();
                            $('.via-point-container').show();
                        }
                    }
                    $("#map").empty();
                    $("#map").load('../Routes/A2BPlanning?routeID=0', function () {
                        
                        setRouteID(RouteId);
                        var listCountSeg = 0;

                        for (var i = 0; i < result.result.routePathList.Count; i++) {
                            listCountSeg = result.result.routePathList[i].routeSegmentList.Count;
                            if (listCountSeg > 0)
                                break;
                        }

                        if (listCountSeg == 0) {
                            if (result.result.routePathList[0].routeSegmentList != null)
                                listCountSeg = 1;
                        }
                        if (RouteType == "outline") {
                            loadmap('DISPLAYONLY');
                            showSketchedRoute(result.result);
                        }
                        else {
                            loadmap('DISPLAYONLY', result.result);
                        }
                        createContextMenu();
                    })
                }
                else {

                    $("#map").empty();
                    $("#ShowDetail").show();
                    $('#trStarting').show();
                    $("#RouteName").text(result.result.routePartDetails.routeName);
                    $('#Tabvia').empty();
                    $('#trVia').hide();
                    if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                        $("#RouteDesc").text(result.result.routePartDetails.routeDescr);
                    for (var i = 0; i < count; i++) {

                        if (result.result.routePathList[0].routePointList[i].pointType == 0)
                            $('#Starting').text(result.result.routePathList[0].routePointList[0].pointDescr);
                        else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                            $('#Ending').text(result.result.routePathList[0].routePointList[1].pointDescr);

                        else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                            Index = Index + 1;
                            strTr = "<div><div style='width:40px'>" + Index + "</div><div>" + result.result.routePathList[0].routePointList[i].pointDescr + "</div></div>"
                            $('#Tabvia').append(strTr);
                            $('#trVia').show();
                            $('.via-point-container').show();
                        }
                    }
                    $("#map").text('No visual representation of route available.');
                }
            }
            else {
                $("#RouteName").text('Route not available.');
                document.getElementById('ShowDetail').style.display = "none";
                document.getElementById('trHeaderDescription').style.display = "none";
                $("#map").empty();
                $("#map").load('../Routes/A2BPlanning?routeID=0', function () {
                    loadmap('DISPLAYONLY');
                    createContextMenu();
                });

                if ($('#Starting').val() != '') {
                    $('#trRoute').show();
                    $('#trStarting').show();
                    $('#trVia').show();
                    $('#trEnding').show();
                    $('#trRoutePart').show();
                }
                else {
                    document.getElementById('ShowDetail').style.display = "none";
                    $('#trRoute').hide();
                    $('#trStarting').hide();
                    $('#trVia').hide();
                    $('#trEnding').hide();
                    $('#trRoutePart').hide();
                }
            }
            stopAnimation();
        },
        complete: function () {
            stopAnimation();
            closeContentLoader('#map');
        },
        error: function () {

            $("#map").empty();
            $("#map").load('../Routes/A2BLeftPanel?routeID=0', function () {
                loadmap('DISPLAYONLY');

            });
        }
    });
}
function reviewProcHighway() {
    if (document.getElementById('reviewproc').style.display !== "none") {
        document.getElementById('reviewproc').style.display = "none"
        document.getElementById('up-chevlon11').style.display = "none"
        document.getElementById('down-chevlon11').style.display = "block"
    }
    else {
        document.getElementById('reviewproc').style.display = "block"
        document.getElementById('up-chevlon11').style.display = "block"
        document.getElementById('down-chevlon11').style.display = "none"
    }
}
function printDiv() {
    $('#divPrintReview').hide();

    var divToPrint = document.getElementById('reviewproc');

    var newWin = window.open('', 'Print-Window');

    newWin.document.write('<html><body id="print-container">' + divToPrint.innerHTML + '</body></html>');

    newWin.document.close();

    setTimeout(function () { newWin.close(); }, 10);
    $('#divPrintReview').Show();
}
function PrintReportAutherizeMovementGen(data) {
    var NotificationId = $(data).attr("notificationid");
    var ESDALREf = $(data).attr("esdalreference");
    var DocType = $(data).attr("doctype");
    var UserType = $(data).attr("usertype");
    var behalf = $(data).attr("onbehalfsoa");
    var DRN = $(data).attr("drn");
    var isNENVal = 0
    var contenRefNum = $('#hfContentRefNum').val();
        if ($('#hf_IsNen').val().toLowerCase() == "true") {
        isNENVal = 1;
    }
    else if ($("#RouteName").val().toLowerCase() == 'ne notification api' ||
        $("#RouteName").val().toLowerCase() == 'ne renotification api') {
        isNENVal = 2;
    }
    ESDALREf = escape(ESDALREf);
    if (DocType == 'agreed') {
        var link = "../Movements/PrintAgreedReport?esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&DocType=" + $('#RouteName').val() + "&VersionId=" + $('#hfVERSION_ID').val() + "";
        window.open(link, '_blank');
    }
    else if (DocType == 'amendment to agreement') {
        var link = "../Movements/PrintAmendmentToAgreementReport?esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&DocType=" + $('#RouteName').val() + "&VersionId=" + $('#hfVERSION_ID').val() + "";
        window.open(link, '_blank');
    }
    else if (DocType == 'proposed') {
        var link = "../Movements/PrintProposedReport?esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&DocType=" + $('#RouteName').val() + "";
        window.open(link, '_blank');
    }
    else if (DocType == 'reproposal') {
        var link = "../Movements/PrintReProposedReport?esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&DocType=" + $('#RouteName').val() + "&SORTflag=" + sortFlag + "";
        window.open(link, '_blank');
    }
    else if (DocType == 'no longer affected') {
        var link = "../Movements/PrintNoLongerAffectedReport?esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&DocType=" + $('#RouteName').val() + "";
        window.open(link, '_blank');
    }
    else {
        //var link = "../Movements/PrintReportPDF?notificationId=" + NotificationId + "&esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&ISNENVal=" + IS_NEN + "&NENInboxId=" + V_INBOX_ID + "";//added ISNENVal and NENInboxId param for NEN project
        var link = "../Movements/PrintReport?notificationId=" + NotificationId + "&esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&ISNENVal=" + isNENVal + "&NENInboxId=" + V_INBOX_ID + "&DocType=" + $('#RouteName').val() + "" + "&contentRefNum=" + contenRefNum;//added ISNENVal and NENInboxId param for NEN project
        window.open(link, '_blank');
    }
    if (document.getElementById('printOptions').style.display !== "none") {
        document.getElementById('printOptions').style.display = "none";
        $("#printOptions").removeClass('clicked');
        $("#MarginTop").css('margin-top', 0);
        $("#printOptions").css("z-index", "0");

    }
}
function PrintReducedReportPrintReducedReport(data) {
    var NotificationId = $(data).attr("notificationid");
    var ESDALREf = $(data).attr("esdalreference");
    var Classification = $(data).attr("doctype");
    var UserType = $(data).attr("usertype");
    var DRN = $(data).attr("drn");
    var isNENVal = 0;
    var contenRefNum = $('#hfContentRefNum').val();
    if ($('#hf_IsNen').val().toLowerCase() == "true") {
        isNENVal = 1;
    }
    else if ($("#RouteName").val().toLowerCase() == 'ne notification api' ||
        $("#RouteName").val().toLowerCase() == 'ne renotification api') {
        isNENVal = 2;
    }
    ESDALREf = escape(ESDALREf);

    var link = "../Movements/PrintReducedReport?notificationId=" + NotificationId + "&esdalRefno=" + ESDALREf + "&classification=" + Classification + "&userType=" + UserType + "&DRN=" + DRN + "&ISNENVal=" + isNENVal + "&NENInboxId=" + V_INBOX_ID + "&DocType=" + $('#RouteName').val() + "" + "&contentRefNum=" + contenRefNum;//added ISNENVal and NENInboxId param for NEN project
    window.open(link, '_blank');
    if (document.getElementById('printOptions').style.display !== "none") {
        document.getElementById('printOptions').style.display = "none";
        $("#printOptions").removeClass('clicked');
        $("#MarginTop").css('margin-top', 0);
        $("#printOptions").css("z-index", "0");

    }
}
function routeRoads(data) {
    var analysisId = $(data).attr("analysisid");

    if (document.getElementById('RoadsRoute').style.display !== "none") {
        document.getElementById('RoadsRoute').style.display = "none"
        document.getElementById('up-chevlon13').style.display = "none"
        document.getElementById('down-chevlon13').style.display = "block"
    }
    else {
        document.getElementById('RoadsRoute').style.display = "block"
        document.getElementById('up-chevlon13').style.display = "block"
        document.getElementById('down-chevlon13').style.display = "none"
        showRoadsOnRoute(analysisId);
    }
}
function routeOverview(data) {
    var analysisId = $(data).attr("analysisid");
    var revisionId = $(data).attr("revisionid");
    if (document.getElementById('routeDescription').style.display !== "none") {
        document.getElementById('routeDescription').style.display = "none"
        document.getElementById('up-chevlon12').style.display = "none"
        document.getElementById('down-chevlon12').style.display = "block"
    }
    else {
        document.getElementById('routeDescription').style.display = "block"
        document.getElementById('up-chevlon12').style.display = "block"
        document.getElementById('down-chevlon12').style.display = "none"
        if (analysisId != 0) {
            showRouteDescription(analysisId, revisionId);
        }
    }
}
function routeDescriptions(data) {
    var analysisId = $(data).attr("analysisid");
    var revisionId = $(data).attr("revisionid");
    if (document.getElementById('inputRouteDescriptions').style.display !== "none") {
        document.getElementById('inputRouteDescriptions').style.display = "none"
        document.getElementById('up-chevlon16').style.display = "none"
        document.getElementById('down-chevlon16').style.display = "block"
    }
    else {
        document.getElementById('inputRouteDescriptions').style.display = "block"
        document.getElementById('up-chevlon16').style.display = "block"
        document.getElementById('down-chevlon16').style.display = "none"
        if (analysisId != 0) {
            showInputRouteDescriptions(analysisId, revisionId);
        }
    }
}

function drivingInstruction(data) {
    var analysisId = $(data).attr("analysisid");
    if (document.getElementById('drivingInstruction').style.display !== "none") {
        document.getElementById('drivingInstruction').style.display = "none"
        document.getElementById('up-chevlon14').style.display = "none"
        document.getElementById('down-chevlon14').style.display = "block"
    }
    else {
        document.getElementById('drivingInstruction').style.display = "block"
        document.getElementById('up-chevlon14').style.display = "block"
        document.getElementById('down-chevlon14').style.display = "none"
        if (analysisId != 0) {
            ShowDrivingInstr(analysisId)
        }
    }
}
function showRouteDescription(analysisId, revisionId) {
    startAnimation();
    var AuthorizeFlagForDI = "true";
    var sortFlag = SortMovementVal != null && SortMovementVal != undefined && SortMovementVal != "False" && SortMovementVal != "false" ? true : false;
    routeDescription

    $("#routeDescription").load("../RouteAssessment/ListRouteDescription?analysisID=" + analysisId + '&anal_type=' + 2 + '&ContentRefNo=' + Content_ref_noVal + '&revisionId=' + revisionId + '&IsCandidate=' + false + '&AgreedSONotif=' + 0 + '&SORTFlag=' + sortFlag, function () {
        stopAnimation();
    });
}

function showInputRouteDescriptions(analysisId, revisionId) {
    startAnimation();
    var AuthorizeFlagForDI = "true";
    var sortFlag = SortMovementVal != null && SortMovementVal != undefined && SortMovementVal != "False" && SortMovementVal != "false" ? true : false;
    var isHistoric = $('#hfIsHistoric').val();
    var isNenViaPdf = ($('#hf_IsNenViaApi').val() === 'True') ? 0 : 1; // 0 for NEN via Api, 1 for NEN Via Pdf
    $("#inputRouteDescriptions").load("../Routes/ListInputRouteDescription?NotificationidVal=" + NotificationidVal + "&isNenViaPdf=" + parseInt(isNenViaPdf) + "&isHistoric=" + parseInt(isHistoric), function () {
        stopAnimation();
    });
}

function showRoadsOnRoute(analysisId) {
    startAnimation();
    var AuthorizeFlagForDI = "true";
    var sortFlag = SortMovementVal != null && SortMovementVal != undefined && SortMovementVal != "False" && SortMovementVal != "false" ? true : false;
    $("#RoadsRoute").load("../RouteAssessment/AffectedRoads?analysisID=" + analysisId + '&anal_type=' + 8 + '&notifId' + 0 + '&contentRefNo=' + '' + '&revisionId=' + 0 + '&IsCandidate=' + false + '&isAuthMove=' + true + '&versionId=' + 0 + '&IsVR1=' + false + '&FilterByOrgID' + 0 + '&SORTFlag=' + sortFlag, function () {
        stopAnimation();
        ListAfeectedRoadInit();
    });
}
function ShowDrivingInstr(analysisId) {
    startAnimation();
    var AuthorizeFlagForDI = "true";
    var sortFlag = SortMovementVal != null && SortMovementVal != undefined && SortMovementVal != "False" && SortMovementVal != "false" ? true : false;
    //startAnimation();
    $("#Affected_Struct_Auth_Mov").show();
    $("#drivingInstruction").load("../RouteAssessment/ListDrivingInstructions?analysisID=" + analysisId + '&anal_type=' + 1 + '&ContentRefNo=' + Content_ref_noVal + '&AuthorizeFlagForDI=' + AuthorizeFlagForDI + '&SORTflag=' + sortFlag, function () {
        stopAnimation();
    });
}
function PaginateListContact() {

    var cntrId = '#contact-list';
    $(cntrId).find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        var pageNum = $(this).html();
        AjaxPaginationforContactList(pageNum);
    });

    PaginateToLastPageContactList(cntrId);
    PaginateToFirstPageContactList(cntrId);
    PaginateToNextPageContactList(cntrId);
    PaginateToPrevPageContactList(cntrId);
}
//method to paginate to last page
function PaginateToLastPageContactList(ContainerId) {

    $(ContainerId).find('.PagedList-skipToLast').click(function () {
        var pageCount = $(ContainerId).find('#TotalPages').val();
        AjaxPaginationforContactList(pageCount);
    });
}
//method to paginate to first page
function PaginateToFirstPageContactList(ContainerId) {

    $(ContainerId).find('.PagedList-skipToFirst').click(function () {
        AjaxPaginationforContactList(1);
    });
}
//method to paginate to Next page
function PaginateToNextPageContactList(ContainerId) {

    $(ContainerId).find('.PagedList-skipToNext').click(function () {
        var thisPage = $(ContainerId).find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPaginationforContactList(nextPage);
    });
}
//method to paginate to Previous page
function PaginateToPrevPageContactList(ContainerId) {

    $(ContainerId).find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $(ContainerId).find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPaginationforContactList(prevPage);
    });
}
function AjaxPaginationforContactList(pageNum) {
    var random = Math.random();
    var notiId = $('#hf_Notificationid').val();
    var routeDetails = routeTypeVal;
    var notificationId = $('#NOTIFICATION_ID').val();
    var esdalRefNumber = $('#ESDAL_Reference').val();
    var ID = $('#hAnalysisId').val();

    routeDetails = routeDetails.toLowerCase().replace("amendment to agreement", "amendment");
    routeDetails = routeDetails.toLowerCase().replace("no longer affected", "nolongeraffected");
    routeDetails = routeDetails.toLowerCase().replace("ne agreed notification", "ne notification");//Added For NEN project
    routeDetails = routeDetails.toLowerCase().replace("ne renotification", "ne renotification");//Added For NEN project
    routeDetails = routeDetails.toLowerCase().replace("ne agreed renotification", "ne renotification");//Added For NEN project

    $.ajax({
        url: '../Contact/AuthoriseMovementShowContactList',
        type: 'POST',
        cache: false,
        async: false,
        data: { page: pageNum, analysisID: ID, NotificationID: notificationId, ESDALRefNumber: esdalRefNumber, Type: routeDetails, random: random },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            var result = $(response).find('div#contact-list').html();
            $('div#contact-list').html(result);
        },
        complete: function () {
            stopAnimation();
        }
    });
    removeHrefContactLinks();
    PaginateListContact();
}
function removeHrefContactLinks() {
    var div = 'div#contact-list';
    $(div).find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
}