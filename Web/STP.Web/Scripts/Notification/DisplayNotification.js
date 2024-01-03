//var dispCollapse = false;
//$(document).ready(function () {
//    var Helpdesk_redirect = $("#hf_Helpdesk_redirect").val();
//    if (Helpdesk_redirect == "true" || Helpdesk_redirect == "True") {
//        $("#haulier").css("display", "none");
//        $("#user-info-filter").css("display", "none");
//    }
//    $("#MoveStat").val($("#hf_VersionStatus").val());
//    DisplayGeneralDetails();
//});

//function DisplayGeneralDetails() {
//    SetTabStyle(1);
//    LoadContentForAjaxCalls("POST", '../Notification/DisplayGeneralDetails', { notificationId: $("#NotificationId").val() }, '#general_details');
//}

//function DisplayRouteVehicle() {
//    SetTabStyle(2);
//    LoadContentForAjaxCalls("POST", '../Notification/DisplayRouteVehicle', { notificationId: $("#NotificationId").val() }, '#route_vehicle_details');
//}

//function DisplayRouteAssessment() {
//    SetTabStyle(3);
//    LoadContentForAjaxCalls("POST", '../Application/RouteAnalysisPanel', { analysisId: $('#NotifAnalysisId').val(), contentRefNo: $('#CRNo').val() }, '#route_assessment_details', function () {
//        RouteAnalysisPanelInit();
//    });
//}

//function DisplayParties() {
//    SetTabStyle(7);
//    CloseSuccessModalPopup();
//    LoadContentForAjaxCalls("POST", '../Notification/DisplayNotifiedparties', { analysisId: $('#NotifAnalysisId').val(), notificationId: $("#NotificationId").val() }, '#notif_parties');
//    $('#overlay').hide();
//    $("#dialogue").hide();
//    $(".modal-backdrop").removeClass("show");
//    $(".modal-backdrop").removeClass("modal-backdrop");
//    removeHLinksParty('#notif_parties');
//    PaginateGridParty('#notif_parties');
//    fillPageSizeSelect();
//    CloseModalPopup();
//}

//function DiplayTransmissionStatus() {
//    SetTabStyle(4);
//    CloseSuccessModalPopup();
//    var tmf = { All: true, Delivered: true, Failed: true, Pending: true, Sent: true }
//    LoadContentForAjaxCalls("POST", '../Notification/TransmissionStatusList',
//        { PageView: true, TMF: tmf, Notification_Code: $('#NotificationCode').val(), showtrans: true, SortStatus: -1 }, '#trans_status_details');

//    $('#overlay').hide();
//    $("#dialogue").hide();
//    $(".modal-backdrop").removeClass("show");
//    $(".modal-backdrop").removeClass("modal-backdrop");
//    collab = false;
//    removeHLinksTrans('trans_status_details');
//    PaginateGridTrans('trans_status_details');
//    fillPageSizeSelect('trans_status_details');
//    CloseModalPopup();
//}
//function DiplayCollaboration() {
//    SetTabStyle(5);
//    // LoadContentForAjaxCalls("POST", '../Notification/DisplayCollaborationList', {notificationCode: $('#NotificationCode').val(), notificationId: $('#NotificationId').val() }, '#collaboration_details');
//    if ($('#hidden_acknowledged').val().toLowerCase() == 'false') {
//        if ($('#hidden_huallierNotes').val() != '') {
//            LoadContentForAjaxCalls("POST", '../Notification/ViewUnacknowledgedCollaboration', { notificationCode: $('#NotificationCode').val() }, '#collaboration_details');
//        }
//    }
//    else if ($('#hidden_acknowledged').val().toLowerCase() == 'true') {
//        var randomNumber = Math.random();
//        LoadContentForAjaxCalls("POST", '../Notification/DisplayCollaborationList', { notificationCode: $('#NotificationCode').val(), notificationId: $('#NotificationId').val(), random: randomNumber }, '#collaboration_details');
//    }
//}

//function DiplayNotifHistory() {
   
//    SetTabStyle(6);
//    LoadContentForAjaxCalls("POST", '../Notification/DisplayNotificationHistory', { notificationId: $('#NotificationId').val() }, '#notif_history_details');
//}
//function SetTabStyle(TabIndex) {
    
//    $("#tab1").removeClass('active-card');
//    $("#tab2").removeClass('active-card');
//    $("#tab3").removeClass('active-card');
//    $("#tab4").removeClass('active-card');
//    $("#tab5").removeClass('active-card');
//    $("#tab6").removeClass('active-card');
//    $("#tab7").removeClass('active-card');

//    $("#tab" + TabIndex).addClass('active-card');
//    $('.nav-link').removeClass('active');
//    $('#menu_container li:nth-child(2)').find('.nav-link').addClass('active');
//}

//function LoadContentForAjaxCalls(Type, Url, Params, ResLoadContnr, callBackFn) {
//    $.ajax({
//        type: Type,
//        url: Url,
//        data: Params,
//        async: false,
//        beforeSend: function () {
//            startAnimation();
//        },
//        success: function (response) {
//            $("#general_details").hide();
//            $("#route_vehicle_details").hide();
//            $('#route_assessment_details').hide();
//            $('#trans_status_details').hide();
//            $('#collaboration_details').hide();
//            $('#notif_history_details').hide();
//            $('#notif_parties').hide();

//            $(ResLoadContnr).show();
//            $(ResLoadContnr).html(response);
//            if (typeof callBackFn != 'undefined' && callBackFn != null) {
//                callBackFn();
//            }
//        },
//        error: function (result) {
//        },
//        complete: function () {
//            stopAnimation();
//        }
//    });
//}
