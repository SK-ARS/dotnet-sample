var routeAnalysisAddRout = false;
var agreedRoute = false;
var configDetails = false;
var routeAnalysis = false;
var cntDetails = false;
var frmHaul = false;
var toHaul = false;
var distbtn = false;
var haulReq = false;
var contactedParties = false;
var hf_ApplicationRevId = $('#hf_ApplicationRevId').val();
var hf_ApprevisionId = $('#hf_ApprevisionId').val();
var hf_versionId = $('#hf_versionId').val();
var hf_routeType1 = $('#hf_routeType1').val();
var hf_revisionId = $('#hf_revisionId').val();
var hf_reduceddetailed = $('#hf_reduceddetailed').val();
var hf_VR1Applciation = $('#hf_VR1Applciation').val();
var hf_ApplicationStatus = $('#hf_ApplicationStatus').val();
var hf_hauliermnemonic = $('#hauliermnemonic').val();
var hf_esdalref = $('#hf_esdalref').val();
var hf_revisionno = $('#hf_revisionno').val();
var hf_versionno = $('#hf_versionno').val();
var hf_VR1Notify = $('#hf_VR1Notify').val();
var hf_pageflag = $('#hf_pageflag').val();
var hf_RouteFlag = $('#hf_RouteFlag').val();
var hf_routeID = $('#hf_routeID').val();
var hf_viewPageFlag = $('#hf_viewPageFlag').val();
var isHistoric = $("#Historical").val();

$("#sort-menu-list .card").click(function () {
    if ($(this).hasClass('no-active-class-required') == false) {
        $("#sort-menu-list .active-card").each(function () {
            $(this).removeClass('active-card');
        });
        $(this).addClass('active-card');
    }
});
$(document).ready(function () {
    $('body').on('click', '#popup', function (e) {
        e.preventDefault();
        displayPopupList(this);
    });
    $('body').on('click', '#btn-gotoRouteLibrary', function (e) {
        e.preventDefault();
        gotoRouteLibrary(this);
    });
    $('body').on('click', '#btn1-gotoRouteLibrary', function (e) {
        e.preventDefault();
        gotoRouteLibrary(this);
    });
    $('body').on('click', '#btn2-gotoRouteLibrary', function (e) {
        e.preventDefault();
        gotoRouteLibrary(this);
    });
    var Helpdesk_redirect = $('#hf_Helpdesk_redirect').val();
    if (Helpdesk_redirect == "true") {
        $("#haulier").css("display", "none");
        $("#user-info-filter").css("display", "none");
        $("#menu-buttons").hide();
    }
    $('#ApplicationrevId').val(hf_ApprevisionId);
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var V_IsNotifyFlag = $('#hdnIsNotifyFlag').val();
    if (hf_ApplicationStatus == 308001) {
        Edit_GeneralDetails();
    }
    else if (V_IsNotifyFlag == 1) {
        load_notificationHistory();
        $(".tab_content").each(function () {
            $(this).hide();
        });
        $('.t').addClass('nonactive');
        $('#tab_7').show();
        $('#tab_wrapper ul li[id=7]').removeClass('nonactive');
    }
    else {
        loadGeneralDetails();
    }
    var vr1 = $('#vr1appln').val();
    if (AppRevId == 0) {
        AppRevId = $('#hf_ApplicationRevId').val();
    }
    if (AppRevId != 0 && hf_ApplicationStatus == 308001) {

        if (vr1 == 'True') {
            VR1validationfun1(hf_reduceddetailed);
        }
        else {
            SOvalidationfun1();
        }
    }
    TabClick();

    $('body').on('click', '.span-close', function (e) { e.preventDefault(); addscroll(this); });
    //-------Below click events for tab click feature , copied from CustomContraol.cs file
    $('body').on('click', '.btn-edit_generaldetails', function (e) { e.preventDefault(); Edit_GeneralDetails(); });
    $('body').on('click', '.btn-listvr1veh', function (e) { e.preventDefault(); listvr1veh(); });
    $('body').on('click', '.btn-listsoveh', function (e) { e.preventDefault(); listsoveh(); });
    $('body').on('click', '.btn-cloneroutes', function (e) { e.preventDefault(); CloneRoutes(); });
    $('body').on('click', '.btn-applrouteassessment', function (e) { e.preventDefault(); ApplRouteAssessment(); });
    $('body').on('click', '.btn-supplementaryinfo', function (e) { e.preventDefault(); SupplementaryInfo(); });
    $('body').on('click', '.btn-loadgeneraldetails', function (e) { e.preventDefault(); loadGeneralDetails(); });
    $('body').on('click', '.btn-remove_routeanalysisaddroutepart', function (e) { e.preventDefault(); remove_routeanalysisAddRoutePart(); });
    $('body').on('click', '.btn-load_applvehicle', function (e) { e.preventDefault(); load_applVehicle(); });
    $('body').on('click', '.btn-load_routeanalysis', function (e) { e.preventDefault(); load_routeAnalysis(); });
    $('body').on('click', '.btn-load_affectedparties', function (e) { e.preventDefault(); load_affectedParties(); });
    $('body').on('click', '.btn-showcontactedparties', function (e) { e.preventDefault(); ShowContactedParties(); });
    $('body').on('click', '.btn-showvehicleconfigdetail', function (e) { e.preventDefault(); ShowVehicleConfigDetail(); });
    $('body').on('click', '#btn-Importrouteinnotif', function (e) {
        e.preventDefault();
        var RouteId = $(this).data('routeid');
        var RouteName = $(this).data('routename');
        Importrouteinnotif(RouteId, RouteName);
    });
});
function gotoRouteLibrary() {
    $("#RoutePart").show();
    $("#ShowDetail").hide();
    $("#RouteMap").html('');
    routelistfrommovement();
}
function NotImp() {
    alert("Not Implemented.");
};
//for loading route parts n vehicle config
function load_applVehicle() {
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    if (AppRevId == 0) {
        AppRevId = $('#hf_ApprevisionId').val();
    }
    $('#leftpanel').hide();
    $('#leftpanel_quickmenu').hide();
    var vr1app = $('#vr1appln').val();
    var vr1contref = $('#VR1ContentRefNo').val();
    if (vr1app == 'True') {
        $('#vr1Route').show();
        if (hf_ApplicationStatus == 308002 || hf_ApplicationStatus == 308003) {
            $('#vr1Route').load('../Application/HaulierApplRouteParts', { RevisionId: AppRevId, VersionId: hf_versionId, SubmitVR1: true, VR1ContentRefNo: vr1contref }, function () {
                CheckSessionTimeOut();
            });
        }
        else {
            $('#vr1Route').load('../Application/HaulierApplRouteParts', { RevisionId: AppRevId }, function () {
                CheckSessionTimeOut();
            });
        }
    }
    else {
        $('#SOHaulierApplDetails').html('');
        $('#tab_3').load('../Application/RouteConfig', { versionId: hf_versionId }, function () {
            CheckSessionTimeOut();
            RouteVehicleInit();
        });
    }
}
function getQuery(key) {
    var temp = location.search.match(new RegExp(key + "=(.*?)($|\&)", "i"));
    if (!temp) return
    else
        return temp[1];
}
function showroutepart() {
    $('#show_route_part').show();
}
function showvehicleconfig() {
    $('#VehicleConfig').show();
}
function remove_routeanalysis() {
    $('#leftpanel').hide();
    $('#leftpanel_quickmenu').hide();
}
function load_affectedParties() {
    $('#leftpanel').hide();
    $('#leftpanel_quickmenu').hide();
}
function remove_routeanalysisAddRoutePart() {
    var ProjectID = $('#projid').val();
    var rev_Id = $('#ApplicationRevId').val() ? $('#ApplicationRevId').val() : $('#ApplicationRevisionId').val();
    var Pageflag = $('#Pageflag').val();
    var sortentered = $('#EnteredBySort').val();
    var PlannerId = $('#PlannrUserId').val();
    var versionId = $('#hf_versionId').val();
    isHistoric = $('#Historical').length > 0 ? $("#Historical").val() : 0;
    startAnimation()
    $("#tab_3").html('');
    $("#leftpanel").html('');
    $('#SOHaulierApplDetails').load('../Application/ApplicationDetails', { appRevisionId: rev_Id, appVersionId: versionId, historic: isHistoric },
        function () {
            $('#SOHaulierApplDetails').show();
            $("#leftpanel").html('');
            $("#leftpanel").hide();
            if ($('#ViewFlag').val() == 0) {
                $('#leftpanel').load('../SORTApplication/SORTLeftPanel?Display=ApplSumm&pageflag=' + Pageflag + '&PlannerId=' + PlannerId + '&Project_ID=' + ProjectID + '&Rev_ID=' + rev_Id + '&Enter_BY_SORT=' + sortentered, function () {
                    $("#leftpanel").show();
                    ApplSummaryLeftPanelInit();
                });
            }
            stopAnimation()
            CheckSessionTimeOut();
            ApplicationRouteListInit();
        });
}
function AgreedRoute_AddRoutePart() {
    $('#leftpanel').hide();
    $('#leftpanel_quickmenu').hide();
    var vr1app = $('#vr1appln').val();
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var versionId = $('#hf_versionId').val();

    //added by poonam (13.8.14)
    var vr1contrefno = $('#VR1ContentRefNo').val();
    //-----------
    $('#submittedRoute').val(1);
    $.ajax({
        url: '../Application/AgreedRoutes',
        type: 'POST',
        data: { VersionId: versionId, revisionid: AppRevId, VR1: vr1app, ContentRefNo: vr1contrefno },
        beforeSend: function () {
            startAnimation();
        },
        success: function (html) {
            $('#SOHaulierApplDetails').html('');
            $('#RoutePart').html(html);
            CheckSessionTimeOut();
            $('#divsovalidation').hide();
        },
        error: function () {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function routepartlist(AppRevId, verstatus) {
    var hdnVRRouteTab = $("#hdnVRRouteTab").val();
    var vr1app = $('#vr1appln').val();
    var vr1RefNo = $('#aesdal_' + AppRevId).data('name');
    if (vr1RefNo != "" && vr1RefNo != undefined && vr1RefNo != null) {
        $('#PrevMovESDALRefNum').val(vr1RefNo);// storing previous movment Esdal ref number for #5697
    }
    if (hdnVRRouteTab == "True") {
        $('#tab_4').show();
        $('#RoutePart').html('');
        var url = '../Application/HaulierApplRouteParts';
        url += '?RevisionId=' + AppRevId + "&approute=" + true;
        $("#RoutePart").load(url, function () {
            CheckSessionTimeOut();
        });

        $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);
    }
    else {
        $('#tab_3').html('');
        var url = '../Application/HaulierApplRouteParts';
        url += '?RevisionId=' + AppRevId + "&soapp=" + true + "&vr1app=" + vr1app + "&VersionStatus=" + verstatus;

        $("#tab_3").load(url, function () {
            CheckSessionTimeOut();
        });
        $("#ChkNewroute1,#ChkNewroute2,#ChkNewroute3").attr("checked", false);
    }
}
function ImportRoute(PartID, RouteName, RouteType) {
    var typr = $('#hf_routeType1').val();
    ImportAppRouteForSOApp(PartID, RouteName, RouteType);
}
//Importing Route For SO App
function ImportAppRouteForSOApp(PartID, RouteName, RouteType) {
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    console.log(PartID);
    $.ajax({
        url: '../Application/SaveRouteInAppParts',
        type: 'POST',
        cache: false,
        data: { routepartId: PartID, AppRevId: AppRevId, routeType: RouteType },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (result != 0) {
                $('#leftpanel').html('');
                showWarningPopDialog('"' + RouteName + '" route imported for this application', 'Ok', '', 'SelectedRouteFromLibraryForVR1', '', 1, 'info');
            }
        },
        error: function (e) {
            //location.reload();
            console.error('error---', e);
        },
        complete: function () {
            stopAnimation();
        }
    });
}
//Importing Route For VR1 App------SP is not ready
function ImportAppRouteForVR1App(PartID) {
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    console.log(PartID);
    $.ajax({
        url: '../Application/SaveRouteInAppParts',
        type: 'GET',
        cache: false,
        data: { routepartId: PartID, AppRevId: AppRevId, VR1route: true },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $('#leftpanel').html('');
            SelectedRouteFromLibraryForVR1();
        },
        error: function (e) {
            //location.reload();
            console.error('error---', e);
        },
        complete: function () {
            stopAnimation();
        }
    });
}
//by ajit need to complete
function Importapproute(partid, routeId) {

    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    $.ajax({
        url: '../Application/AppRoute_MovementList',
        type: 'POST',
        datatype: 'json',
        async: false,
        data: { routeId: routeId, apprevisionId: ApplicationRevId, routepartid: partid },
        success: function (page) {
            startAnimation();
            $('#tab_4').html('');
            var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            var url = '../Application/ListImportedRouteFromLibrary';
            url += '?apprevisionId=' + ApplicationRevId;
            $("#tab_4").load(url, function () {
                ListImportedRouteFromLibraryInit();
                CheckSessionTimeOut();
            });
        },
        error: function (e) {
            //location.reload();
            console.error('error---', e);
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function load_notificationHistory() {
    $('#leftpanel').hide();
    $('#leftpanel_quickmenu').hide();
    var versionId = $('#VersionId').val();
    $('#tab_7').load('../Application/NotificationHistory', { versionId: hf_versionId }, function () {
        CheckSessionTimeOut();
    });
}
function load_routeAnalysis() {

    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var analysisId = $('#analysisId').val() ? $('#analysisId').val() : 0;
    var vr1contrefno = $('#VR1ContentRefNo').val();

    $.ajax({
        url: '../Application/RouteAnalysisPanel',
        type: 'POST',
        data: { analysisId: analysisId, RivisionId: ApplicationRevId, contentRefNo: vr1contrefno },
        beforeSend: function () {
            startAnimation();
        },
        success: function (html) {

            $('#leftpanel').html(html);
            routeAnalysis = true;
            stopAnimation();
            CheckSessionTimeOut();
            $('li#RBAffectedParty').remove();
            $('li#RBAffectedRoad').remove();
            $('#leftpanel').show();
            RouteAnalysisPanelInit();
        },
        error: function (e) {
            //location.reload();
            console.error('error---', e);
        },
        complete: function () {
        }
    });

}
function listsoveh() {
    if ($("#TextChangeFlagSO").val() == 'False' || $("#TextChangeFlagSO").val() == 'false') {
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        if (ApplicationRevId == 0) {
            ApplicationRevId = $('#hf_ApprevisionId').val();
        }

        if (ApplicationRevId != 0) {
            SOvalidationfun1();

            $('#hidden_apprevisionId').val(ApplicationRevId);
            $('#form_list_vehicle').submit();
        }
    }
}
function listvr1veh() {
    $('#tab_3').html('');
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;


    if (ApplicationRevId != 0) {

        VR1validationfun1(hf_reduceddetailed);

        $('#leftpanel').html('');
        $("#leftpanel").show();
        $("#leftpanel").load('../Application/SoVehicle', new { VR1App: true }, function () { CheckSessionTimeOut(); });

        var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        var routePartId = $('#RoutePartId').val();
        if (routePartId == null || routePartId == 0) {
            routePartId = routePartIdVR1;
        }
        var isVR1 = $('#vr1appln').val();
        var isNotify = $('#ISNotif').val();
        //added by poonam (13.8.14)
        var vr1contrefno = $('#VR1ContentRefNo').val();
        var vr1versionid = $('#VersionId').val();
        //-----------
        $('#hidden_apprevisionId').val(AppRevId);
        $('#hidden_routepartId').val(routePartId);
        $('#hidden_VRAPP').val(isVR1);
        $('#hidden_versionId').val(vr1versionid);
        $('#hidden_vr1content').val(vr1contrefno);
        $('#form_list_vehicle').submit();
    }
}
function importapp(partid, vehid, configName, vehtype) {


    var isVR1 = $('#vr1appln').val();
    var PrevMovESDALRefNum = $('#PrevMovESDALRefNum').val();
    var SOVersionID = $('#RevisionID').val(); //Previous Movement version id
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    $.ajax({
        url: '../Application/AppVehicle_MovementList',
        type: 'POST',
        datatype: 'json',
        async: false,
        data: { vehicleId: vehid, apprevisionId: ApplicationRevId, routepartid: partid, IsVR1: isVR1, VehicleType: vehtype, PrevMovEsdalRefNum: PrevMovESDALRefNum, SOVersionId: SOVersionID },
        success: function (result) {
            startAnimation();

            $('#tab_3').html('');
            var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;

            showPopUpDialog('Configuration "' + configName + '" imported successfully', 'Ok', '', 'CloseModelPop', '', 1, 'info');
            $('#hidden_apprevisionId').val(ApplicationRevId);
            $('#hidden_routepartId').val();
            $('#hidden_VRAPP').val();
            SOvalidationfun1();

            $('#form_list_vehicle').submit();
        },
        error: function (e) {
            //location.reload();
            console.error('error---', e);
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function SupplementaryInfo() {

    $("#leftpanel").hide();
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var url = '../Application/VR1EditSupplementaryDetails';
    url += '?apprevisionId=' + ApplicationRevId;
    $("#tab_6").load(url, function () {
        CheckSessionTimeOut();
    });
}
function ShowVehicleConfigDetail() {
    load_notificationHistory()
}
function unhook(id, routeName) {
    $("#mapTitle").html('');
    $('#tab_4').show();
    $('#RoutePart').html('');
    $('#RoutePart').show();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        async: false,
        url: '../Routes/GetPlannedRoute',
        data: { RouteID: id, IsPlanMovement: $("#hf_IsPlanMovmentGlobal").length > 0, IsCandidateView: IsCandidateRouteView() },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            for (var x = 0; x < result.result.routePathList.length; x++) {
                result.result.routePathList[x].routePointList.sort((a, b) => a.routePointId - b.routePointId);
            }
            $("#RoutePart").load('../Routes/A2BPlanning', { routeID: id }, {}, function () {
                CheckSessionTimeOut();
                $("#mapTitle").append('<h4 style="color:#E30040;">' + routeName + '</h4> <br/>');
                loadmap('DISPLAYONLY', result.result);
                $('#leftpanel').html('');
                $('#RoutePart').append('<button class="btn_reg_back next btngrad btnrds btnbdr" id="btn-Backtoroute"  type="button" data-icon="" aria-hidden="true">Cancel</button>');
            });
            stopAnimation();
        }
    });
}
function LoadLeftPanel() {
    var id = $('#RouteID').val();

    $.ajax({
        url: '../Routes/A2BLeftPanel',
        type: 'POST',
        data: { routeID: hf_routeID, val: hf_viewPageFlag },
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#leftpanel').html('');
            $('#leftpanel').html(page);
            CheckSessionTimeOut();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function ShowContactedParties() {
    $('#leftpanel_quickmenu').hide();
    $('#leftpanel').hide();
    var analysisId = $('#analysisId').val() ? $('#analysisId').val() : 0;

    if (!contactedParties) {
        $.ajax({
            url: '../Contact/ContactedPartiesList',
            type: 'POST',
            data: { analysisID: analysisId },
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {

                $('#tab_8').html(page);
                $("#tab_8").show();
                CheckSessionTimeOut();
                ContactedPartiesInit();
            },
            error: function (err, ex, xhr) {

                showWarningPopDialog('An error occured. Please try again.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
            },
            complete: function () {
                stopAnimation();
                contactedParties = true;
            }
        });
    }
}
function ApplRouteAssessment() {

    //The following changes were added for vr1 related route assessment.
    var contentRefNo = $('#VR1ContentRefNo').val();
    var versionId = $('#VersionId').val();
    var statusVR1 = $('#vr1appln').val();

    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var analysisId = $('#analysisId').val() ? $('#analysisId').val() : 0;
    if (ApplicationRevId != 0) {
        $.ajax({
            url: '../Application/RouteAnalysisPanel',
            type: 'POST',
            data: { versionId: versionId, analysisId: analysisId, RivisionId: ApplicationRevId, ContentRefNo: contentRefNo },
            beforeSend: function () {
                startAnimation();
            },
            success: function (html) {
                $('#leftpanel').show();
                $('#leftpanel').html(html);
                routeAnalysis = true;
                stopAnimation();
                CheckSessionTimeOut();
                RouteAnalysisPanelInit();
            },
            error: function (e) {
                //location.reload();
                console.error('error---', e);
            },
            complete: function () {
            }
        });
    }
}
//load edit generale details
function Edit_GeneralDetails() {

    $('#leftpanel').hide();

    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var ReducedDet = $('#Reduceddetailed').val();

    if (ApplicationRevId == 0) {
        ApplicationRevId = $('#hf_revisionId').val();
        ReducedDet = $('#hf_reduceddetailed').val();
    }


    if (hf_VR1Applciation == 'True') {
        $("#URLKEY").val("VR1");
        $('#DeleteVR1App').show();
        $.ajax({
            url: '../Application/ApplyVR1GeneralDetails',
            type: 'POST',
            data: { reduceddetailed: ReducedDet, applicationrevid: ApplicationRevId },
            beforeSend: function () {
                startAnimation();
            },
            success: function (html) {
                $('#tab_1').show();
                $('#SOGeneralDetails').html(html);
                CheckSessionTimeOut();

            },
            error: function (e) {
                //location.reload();
                console.error('error---', e);
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    else {
        $("#URLKEY").val("SO");
        $('#DeleteSOApp').show();
        $.ajax({
            url: '../Application/CreateGeneralApplication',
            type: 'POST',
            data: { hauliermnemonic: hf_hauliermnemonic, esdalref: hf_esdalref, revisionno: hf_revisionno, versionno: hf_versionno, applicationrevid: ApplicationRevId },
            beforeSend: function () {
                startAnimation();
            },
            success: function (html) {
                $('#tab_1').show();
                $('#SOGeneralDetails').html(html);
                CheckSessionTimeOut();
            },
            error: function (e) {
                //location.reload();
                console.error('error---', e);
            },
            complete: function () {
                stopAnimation();
            }
        });
    }

}
//load display general details
function loadGeneralDetails() {
    isHistoric = $('#Historical').length > 0 ? $("#Historical").val() : 0;
    if ($('#hf_VR1Applciation').val() == 'True') {
        $("#URLKEY").val("VR1");
        $.ajax({
            url: '../Application/VR1GeneralDetails',
            type: 'POST',
            data: { reduceddetailed: hf_reduceddetailed, applicationrevid: hf_revisionId, VR1Notify: hf_VR1Notify, versionid: hf_versionId, isHistory: isHistoric },
            beforeSend: function () {
            },
            success: function (html) {
                $('#tab_1').show();
                $('#menu_container li:nth-child(2)').find('.nav-link').addClass('active');
                $('#SOGeneralDetails').html(html);
                $("#leftpanel").hide();
                VR1GeneralDetailInit();
            },
            error: function (e) {
                console.error('error---', e);
            },
            complete: function () {
            }
        });
    }
    else {
        $("#URLKEY").val("SO");
        $.ajax({
            url: '../Application/SOGeneralDetails',
            type: 'POST',
            data: { hauliermnemonic: hf_hauliermnemonic, esdalref: hf_esdalref, revisionno: hf_revisionno, versionno: hf_versionno, revisionId: hf_revisionId, versionId: hf_versionId, isHistory: isHistoric },
            beforeSend: function () {
                startAnimation();
            },
            success: function (html) {
                $('#tab_1').show();
                $('#menu_container li:nth-child(2)').find('.nav-link').addClass('active');
                $('#SOGeneralDetails').html(html);
                CheckSessionTimeOut();
                SoGeneralDetailInit();
            },
            error: function (e) {
                console.error('error---', e);
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
}
function LoadResult(data) {
    $('#tab_3').html(data);
    CheckSessionTimeOut();
}
function LoadVROneRoute(revId) {
    $('#RevisionId').val(revId);
    $('#form_vrRoute').submit();
}
function Validation() {
    // for validation message
    if (hf_ApplicationStatus == 308001 || hf_ApplicationStatus == 0) {
        $(".t").bind("click", function () {
            if (AppRevId != 0) {
                if (hf_VR1Applciation == "False") {
                    SOvalidationfun1();
                }
                else {
                    VR1validationfun1();
                }
            }
        });
    }

    if (hf_ApplicationStatus == 308001 || hf_ApplicationStatus == 0) {
        if (AppRevId != 0) {
            if (hf_VR1Applciation == "False") {
                SOvalidationfun1();
            }
            else {
                VR1validationfun1();
            }
        }
    }
}
function TabClick() {
    $(".t").bind("click", function () {
        var id = $(this).attr("id");
        if (id == 3) {
            if (hf_ApplicationStatus == 308001 || hf_ApplicationStatus == 0) {
                var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
                if (ApplicationRevId == 0) {
                    ApplicationRevId = $('#hf_ApprevisionId').val();
                }
                if (ApplicationRevId != 0) {
                    $("#leftpanel").load('../Application/SoVehicle', new { VR1App: hf_VR1Applciation }, function () {
                        $('#leftpanel_quickmenu').html('');
                        $("#leftpanel").show();
                        CheckSessionTimeOut();
                    });
                }
                $("#hdnVRRouteTab").val("False");
            }
            else {
                $("#leftpanel").hide();
            }
        }
        if (id == 4) {
            $('#RouteMap').html('');
            CloneRoutes();
            $("#hdnVRRouteTab").val("True");
            if (hf_ApplicationStatus == 308001 || hf_ApplicationStatus == 0) {
                var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
                if (ApplicationRevId == 0) {
                    ApplicationRevId = $('#hf_ApprevisionId').val();
                }
                if (ApplicationRevId != 0) {
                    $('#mapTitle').html('');
                    var flag = hf_pageflag;
                    $("#leftpanel").load('../Application/SoRoute', new { pageflag: flag }, function () {
                        $('#leftpanel_quickmenu').html('');
                        $("#leftpanel").show();
                        CheckSessionTimeOut();
                    });
                }
            }
            else {
                $("#leftpanel").hide();
            }
            addscroll();
        }
    });
}
function displaySoRouteDescMapforNotification(RouteId, RouteType) {

    var hdnVRRouteTab = $("#hdnVRRouteTab").val();
    if (hdnVRRouteTab == "True") {
        $("#RoutePart").hide();
        $("#ShowDetail").show();
        $("#RouteMap").html('');
        var str = '';
        var HfRouteType = $('#HfRouteType').val();
        if (RouteType != "") {
            str = RouteType;
            HfRouteType = RouteType;
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            async: false,
            url: '../Routes/GetPlannedRoute',
            data: { RouteID: RouteId, routeType: str, IsPlanMovement: $("#hf_IsPlanMovmentGlobal").length > 0, IsCandidateView: IsCandidateRouteView() },
            beforeSend: function (xhr) {
                startAnimation();
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
                    if (flag == 1 || hf_RouteFlag == 1 || hf_RouteFlag == 3) {
                        $('#Tabvia').html('');
                        $("#ShowDetail").show();
                        $("#div_Route").hide();

                        $("#RouteName").html(result.result.routePartDetails.routeName);
                        if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                            $("#RouteDesc").html(result.result.routePartDetails.routeDescr);
                        for (var i = 0; i < count; i++) {
                            if (result.result.routePathList[0].routePointList[i].pointType == 0)
                                $('#Starting').html(result.result.routePathList[0].routePointList[0].pointDescr);
                            else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                                $('#Ending').html(result.result.routePathList[0].routePointList[1].pointDescr);

                            else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                                Index = Index + 1;
                                strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                                $('#Tabvia').append(strTr);
                            }
                        }
                        $("#RouteMap").html('');
                        $("#RouteMap").load('../Routes/A2BPlanning', "Routes", { routeID: 0 }, function () {
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
                            if (listCountSeg == 0) {
                                loadmap('DISPLAYONLY');
                                showSketchedRoute(result.result);
                            }
                            else {
                                loadmap('DISPLAYONLY', result.result);
                            }
                        })
                    }
                    else {

                        $("#RouteMap").html('');
                        $("#ShowDetail").show();
                        $("#div_Route").hide();

                        $("#RouteName").html(result.result.routePartDetails.routeName);
                        $('#Tabvia').html('');
                        if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                            $("#RouteDesc").html(result.result.routePartDetails.routeDescr);
                        for (var i = 0; i < count; i++) {

                            if (result.result.routePathList[0].routePointList[i].pointType == 0)
                                $('#Starting').html(result.result.routePathList[0].routePointList[0].pointDescr);
                            else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                                $('#Ending').html(result.result.routePathList[0].routePointList[1].pointDescr);

                            else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                                Index = Index + 1;
                                strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                                $('#Tabvia').append(strTr);
                            }
                        }
                        $("#RouteMap").html('No visual representation of route available.');
                    }
                }
                else
                    $("#RouteName").html('Route not available.');
                if ($('#Starting').html() == '') {
                    $('#trRoute').hide();
                    $('#trStarting').hide();
                    $('#trVia').hide();
                    $('#trEnding').hide();
                }
                else {
                    $('#trRoute').show();
                    $('#trStarting').show();
                    $('#trVia').show();
                    $('#trEnding').show();
                }
                if ($("#RouteDesc").html() != "") {
                    $('#trHeaderDescription').show();
                    $('#trdesc').show();
                }
                else {
                    $('#trHeaderDescription').hide();
                    $('#trdesc').hide();
                }


                stopAnimation();
            }
        });

        $("#tdBtn").html('');

        $("#tdBtn").html('<button class="btn_reg_back next btngrad btnrds btnbdr"  aria-hidden="true" data-icon="" type="button" id="popup" >Back</button>  <button id="btn-Importrouteinnotif" data-RouteId="' + RouteId + '" data-RouteName="' + $("#RouteName").html() + '"  class="next btngrad btnrds btnbdr"  aria-hidden="true" data-icon="&#xe0f4;" >Import</button>');

        $("#TDbtnBackToRouteList").html('<button class="btn_reg_back next btngrad btnrds btnbdr"  aria-hidden="true" data-icon="" type="button" id="btn1-gotoRouteLibrary" >Back</button> ')

        $("#TDbtnBackToRouteList").html('<button class="btn_reg_back next btngrad btnrds btnbdr"  aria-hidden="true" data-icon="" type="button" id="btn2-gotoRouteLibrary" >Back</button>  <button id="importinapp" data-rid="' + RouteId + '" data-type="' + HfRouteType + '"  class="next btngrad btnrds btnbdr"  aria-hidden="true" data-icon="&#xe0f4;" >Import</button>')


    }
}
function Importrouteinnotif(LibraryrouteId, routename) {
    var P_CONTENT_REF = $('#CRNo').val() ? $('#CRNo').val() : 0;
    $.ajax({
        url: '../Application/SaveRouteInRouteParts',
        type: 'POST',
        async: false,
        cache: false,
        data: { routepartId: LibraryrouteId, routetype: 'planned', AppRevId: 0, CONTENT_REF: P_CONTENT_REF },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            var msg = '"' + routename + '" ' + "route imported for this application.";
            showWarningPopDialog(msg, 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
            CloneRoutes();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
    $('#mapTitle').html('');
}
function ListVehicles(result) {
    $('#tab_3').html('');
    $('#tab_3').html(result);
    $('#leftpanel').html('');
    $("#leftpanel").html('');
    $("#leftpanel").show();
    $("#leftpanel").load('../Application/SoVehicle');
    $("#ChkNewroute1,#ChkNewroute2,#ChkNewroute3").attr("checked", false);
}
function setTittle() {

    if (hf_RouteFlag == 2) { $('#pageheader').html(''); $('#pageheader').html('<h3> Apply for SO  - Route</h3>'); }
    else if ((hf_RouteFlag == 1) || (hf_RouteFlag == 3)) {
        $('#pageheader').html(''); $('#pageheader').html('<h3> Apply for VR-1 - Route</h3>');
    }
}