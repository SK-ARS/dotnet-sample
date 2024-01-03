var dispCollapse = false;
var versionstatus = $('#hf_versionstatus').val();
$(document).ready(function () {
    var Helpdesk_redirect = $('#hf_Helpdesk_redirect').val();
    if (Helpdesk_redirect == "true") {
        $("#haulier").css("display", "none");
        $("#user-info-filter").css("display", "none");
    }
    $("#MoveStat").val(versionstatus);
    if ($("#MoveStat").length > 0)
        DisplayGeneralDetails();

    $('body').on('click', '.btn-display-general-details', function (e) { e.preventDefault();DisplayGeneralDetails();});
    $('body').on('click', '.btn-display-route-vehicle', function (e) { e.preventDefault(); DisplayRouteVehicle();});
    $('body').on('click', '.btn-display-route-assessment', function (e) { e.preventDefault(); DisplayRouteAssessment();});
    $('body').on('click', '.btn-display-parties', function (e) { e.preventDefault(); DisplayParties();});
    $('body').on('click', '.btn-diplay-transmission-status', function (e) { e.preventDefault(); DiplayTransmissionStatus();});
    $('body').on('click', '.btn-display-collaboration', function (e) { e.preventDefault(); DiplayCollaboration();});
    $('body').on('click', '.btn-display-notif-history', function (e) { e.preventDefault(); DiplayNotifHistory();});
});



function DisplayGeneralDetails() {
    SetTabStyle(1);
    LoadContentForAjaxCalls("POST", '../Notification/DisplayGeneralDetails', { notificationId: $("#NotificationId").val(), isHistory: $("#Historical").val() }, '#general_details', function () {
        DisplayNotificationGeneralDetailsInit();
    });
}

function DisplayRouteVehicle() {
    SetTabStyle(2);
    LoadContentForAjaxCalls("POST", '../Notification/DisplayRouteVehicle', { notificationId: $("#NotificationId").val(), isHistory: $("#Historical").val() }, '#route_vehicle_details', function () {
        DisplayNotificationRouteVehicleInit();
    });
}

function DisplayRouteAssessment() {
    SetTabStyle(3);
    LoadContentForAjaxCalls("POST", '../Application/RouteAnalysisPanel', { analysisId: $('#NotifAnalysisId').val(), contentRefNo: $('#CRNo').val() }, '#route_assessment_details', function () {
        RouteAnalysisPanelInit();
    });
}

function DisplayParties() {
    SetTabStyle(7);
    CloseSuccessModalPopup();
    LoadContentForAjaxCalls("POST", '../Notification/DisplayNotifiedparties', { analysisId: $('#NotifAnalysisId').val(), notificationId: $("#NotificationId").val() }, '#notif_parties');
    $('#overlay').hide();
    $("#dialogue").hide();
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
    CloseModalPopup();
}

function DiplayTransmissionStatus() {
    SetTabStyle(4);
    CloseSuccessModalPopup();
    var tmf = { All: true, Delivered: true, Failed: true, Pending: true, Sent: true }
    LoadContentForAjaxCalls("POST", '../Notification/TransmissionStatusList',
        { PageView: true, TMF: tmf, Notification_Code: $('#NotificationCode').val(), showtrans: true, SortStatus: -1, historic: $("#Historical").val() }, '#trans_status_details');

    $('#overlay').hide();
    $("#dialogue").hide();
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
    collab = false;
    removeHLinksTrans('trans_status_details');
    PaginateGridTrans('trans_status_details');
    fillPageSizeSelect('trans_status_details');
    CloseModalPopup();
}
function DiplayCollaboration() {
    SetTabStyle(5);
    if ($('#hidden_acknowledged').val().toLowerCase() == 'false') {
        if ($('#hidden_huallierNotes').val() != '') {
            LoadContentForAjaxCalls("POST", '../Notification/ViewUnacknowledgedCollaboration', { notificationCode: $('#NotificationCode').val(), historic: $("#Historical").val() }, '#collaboration_details');
        }
    }
    else if ($('#hidden_acknowledged').val().toLowerCase() == 'true') {
        var randomNumber = Math.random();
        LoadContentForAjaxCalls("POST", '../Notification/DisplayCollaborationList', { notificationCode: $('#NotificationCode').val(), notificationId: $('#NotificationId').val(), random: randomNumber, historic: $("#Historical").val() }, '#collaboration_details');
    }
}

function DiplayNotifHistory(sortOrder = null, sortType = null, pageNum = 1, pageSize = 10) {

    SetTabStyle(6);
    LoadContentForAjaxCalls("POST", '../Notification/DisplayNotificationHistory', { notificationId: $('#NotificationId').val(), projectId: $('#ProjectId').val(), sortOrder: sortOrder, sortType: sortType, page: pageNum, historic: $("#Historical").val(), pageSize: pageSize }, '#notif_history_details');
    stopAnimation();
}
function SetTabStyle(TabIndex) {
    $("#tab1").removeClass('active-card');
    $("#tab2").removeClass('active-card');
    $("#tab3").removeClass('active-card');
    $("#tab4").removeClass('active-card');
    $("#tab5").removeClass('active-card');
    $("#tab6").removeClass('active-card');
    $("#tab7").removeClass('active-card');

    $("#tab" + TabIndex).addClass('active-card');
    $('.nav-link').removeClass('active');
    $('#menu_container li:nth-child(2)').find('.nav-link').addClass('active');
}
function AuditlogSort(event, param) {
    
    sortOrder = param;
    sortType = $(event).hasClass('sorting_asc') || $(event).find('.sorting').hasClass('sorting_asc') ?  1 : 0;
    var pageNum = $('#pageNum').val();
    var pageSizeVal = $('#pageSizeVal').val();
    DiplayNotifHistory(sortOrder, sortType, pageNum, pageSizeVal);
}
function LoadContentForAjaxCalls(Type, Url, Params, ResLoadContnr, callBackFn) {
    $.ajax({
        type: Type,
        url: Url,
        data: Params,
        //async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $("#general_details").hide();
            $("#route_vehicle_details").hide();
            $('#route_assessment_details').hide();
            $('#trans_status_details').hide();
            $('#collaboration_details').hide();
            $('#notif_history_details').hide();
            $('#notif_parties').hide();

            $(ResLoadContnr).show();
            $(ResLoadContnr).html(response);
            if (typeof callBackFn != 'undefined' && callBackFn != null) {
                callBackFn();
            }
        },
        error: function (result) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}

$('body').on('change', '#NotificationHistory #pageSizeSelect', function () {

    var pageSize = $(this).val();
    var pageNum = $('#pageNum').val();
    $('#pageSizeVal').val(pageSize);
    DiplayNotifHistory(1, 1, pageNum, pageSize);
});

//NotificationHistory pagination
$('body').on('click', '#NotificationHistory a', function (e) {

    if (this.href == '') {
        return false;
    }
    else {

        e.preventDefault();
        var page = getUrlParameterByName("page", this.href);
        $('#pageNum').val(page);
        var pageSize = $('#pageSizeVal').val();
        DiplayNotifHistory(1, 1, page, pageSize);

        //$.ajax({
        //    url: this.href,
        //    type: 'GET',
        //    cache: false,
        //    beforeSend: function () {
        //        startAnimation();
        //    },
        //    success: function (result) {
        //        $('#banner-container').find('div#filters').remove();
        //        document.getElementById("vehicles").style.filter = "unset";
        //        $("#notif_history_details").html(result);
        //        var filters = $('#notif_history_details').find('div#filters');
        //        $(filters).appendTo('#banner-container');
        //    },
        //    complete: function () {
        //        stopAnimation();
        //    }
        //});
        return false;
    }
});
