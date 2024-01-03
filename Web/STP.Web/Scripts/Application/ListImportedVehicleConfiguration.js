var partid = 0;
var VehicleArr = [];
var RouteArr = [];
var RouteTypeArr = [];

var arr1;
var arr2;
var arr3;
var vehicleid = 0;
var routeid = 0;
var AppRevId = 0;

var IsCandidate = $('#hf_IsCandidate').val();
var IsNotif = $('#hf_IsNotif').val();
var pageflag = $('#hf_sessionPageflag').val();
var sessionRouteFlag = $('#hf_sessionRouteFlag').val();

function ListImportedVehicleConfigurationInit() {
    IsCandidate = $('#hf_IsCandidate').val();
    IsNotif = $('#hf_IsNotif').val();
    pageflag = $('#hf_sessionPageflag').val();
    sessionRouteFlag = $('#hf_sessionRouteFlag').val();
    if (IsCandidate == 'False') {
        $("#btnfinishcopy").hide();
        if ($('#hf_routecount').val() == 1 || $('#hf_routecount1').val() == 0) {
            $("#copy").hide();
        }
        else {
            $("#copy").show();
        }
    }
    var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    var sort_status = $('#SortStatus').val() ? $('#SortStatus').val() : 0;
    if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id)) {
        $('#copy').hide();
        $('#btn_edit_veh').hide();
        $('#btn_del_veh').hide();
        $('#btn_copy_veh').hide();
        $('.filter_button').hide();
    }
    if (chk_status == 301006) {
        if (sort_status == "CreateSO") {
            $('#copy').show();
            $('#btn_edit_veh').show();
            $('#btn_del_veh').show();
            $('#btn_copy_veh').show();
        }
        else {
            $('#copy').hide();
            $('#btn_edit_veh').hide();
            $('#btn_del_veh').hide();
            $('#btn_copy_veh').hide();
            $('.filter_button').hide();

        }
    }
}

$(document).ready(function () {
    $('body').on('click', '#mov-vehicle', function (e) {
        e.preventDefault();
        SelectPrevtMovementsVehicleImport(this);
    });
    $('body').on('click', '#btn-SelectCurrentMovementsVehicle', function (e) {
        e.preventDefault();
        SelectCurrentMovementsVehicle(this);
    });
    $('body').on('click', '#btn-SelectPrevtMovementsVehicle', function (e) {
        e.preventDefault();
        SelectPrevtMovementsVehicleImport(this);
    });
    $('body').on('click', '#copy', function (e) {
        e.preventDefault();
        copyroute(this);
    });
    $('body').on('click', '#btnfinishcopy', function (e) {
        e.preventDefault();
        Updateroutepart(this);
    });
    $('body').on('click', '#btnnavigatenext', function (e) {
        e.preventDefault();
        NavigateToRoute(this);
    });
    $('body').on('click', '#a-closeFilters', function (e) {
        e.preventDefault();
        closeFilters(this);
    });
    $('body').on('click', '#btn-ClearAdvancedSORT', function (e) {
        e.preventDefault();
        ClearAdvancedSORT(this);
    });
    
    $('body').on('click', '#btn-FillNormalFilter', function (e) {
        e.preventDefault();
        FillNormalFilter(this);
    });
    $('body').on('click', '#ViewVehicleDetails', function (e) {
        e.preventDefault();
        var vehicleId = $(this).data('vehicleid');
        ViewVehicleDetails(vehicleId);
    });
    $('body').on('click', '.AmendReg1', function (e) {
        e.preventDefault();
        var VehicleId = $(this).data('vehicleid');

        AmendReg(VehicleId);
    });
    $('body').on('click', '#img-CloneVehicle', function (e) {
        e.preventDefault();
        var VehicleId = $(this).data('vehicleid');
        var VehicleName = $(this).data('vehiclename');
        var Msg = "Are you sure want to copy '" + "" + "'" + VehicleName + "'" + "" + "' ?";
        ShowDialogWarningPop(Msg, 'NO', 'YES', 'WarningCancelBtn', 'CloneVehicle', 1, 'warning', VehicleId, VehicleName, $(this));
        //CloneVehicle(VehicleId, VehicleName, $(this));
    });
    $('body').on('change', '.clsroutepartidchange', function (e) {
        var routepartid = $(this).val();
        var vehicleid = this.id.split("RoutePartsId").pop()
        VehicleonChange(routepartid, vehicleid);

    });
});

function fillroutes() {
    AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    $.getJSON('../Application/GetRoutePartsModel', {
        apprevisionId: AppRevId
    })
        .done(function (data) {

            RouteParts.html('');

            var appenddata;
            $.each(data, function (routeID, routeName) {
                appenddata += "<option value = '" + value.routeID + " '>" + value.routeName + " </option>";
            });

            appenddata += "<option value = '" + data[0].routeID + " '>" + data[0].routeName + " </option>";
            $('#RouteParts').html(appenddata);




        })
        .fail(function (jqxhr, textStatus, error) {
            alert('fail');
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
}

function resetcopyroute() {

    $("#btnfinishcopy").hide();
    
    
    $('.editnmode').hide();
    $('.hiddenmode').show();
    $("#copy").show();
   
}
function copyroute() {
    $("#btnfinishcopy").show();
    
    $('.editnmode').show();
    $('.hiddenmode').hide();
    $("#copy").hide();

    
    $('.hiddenmode').each(function () {
        var name = $.trim($(this).html());

        $(this).parent().find('.drp_route_parts option').filter(function () {
            return ($.trim($(this).text()) == name);
        }).prop('selected', true);
    });

}

function CreateVehicle() {
    $('#leftpanel').show();
    var div = document.getElementById('VehicleSOApplication');

    if (div == null) {

        //call sovehicle view<a href="~/Views/Application/SoVehicle.cshtml">~/Views/Application/SoVehicle.cshtml</a>
        $("#leftpanel").load('../Application/SoVehicle');
    }

    $('#VehicleSOApplication').show();
    $('#FleetConfiguration').hide();
    $('#ShowSelectedFleetConfiguration').hide();
}

function Updateroutepart() {
    var VehicleArray = null;
    var RouteArray = null;
    var RouteTypeArray = null;

    var VR1Appl = $('#VR1Appl').val();


    for (var i = 0; i < VehicleArr.length; i++) {
        VehicleArray = VehicleArr + ',';
        RouteArray = RouteArr + ',';
        RouteTypeArray = RouteTypeArr + ',';
    }
    var dataArr = JSON.stringify({ VehicleArray: VehicleArray, RouteArray: RouteArray, RouteTypeArray: RouteTypeArray, arrlen: VehicleArr.length, VR1Appl: VR1Appl, Notif: $('#hf_IsNotif').val(), Iscand: $('#IsCandVersion').val() });
    $.ajax({
        url: '../Application/Updateroutepart',
        type: 'POST',
        cache: false,
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: dataArr,
        beforeSend: function (data) {
            startAnimation();
        },
        success: function (outputRes) {
            if (outputRes && outputRes.result == 1) {
                showToastMessage({
                    message: "Routes have been assigned to vehicles successfully.",
                    type: "success"
                })
            }
            //refresh list of imported vehicles n routes
            $('#tab_3').show();
            var revId = $('#ApplicationrevId').val();
            var contentno = $('#hf_ContentRefNo').val();

            if ($('#hf_IsCandidate').val() == 'True') {
                Show_CandidateRTVehicles();
                
            }
            else if (VR1Appl == "True") {
                listimportedVR1vehicles(revId);
            }
            if ($('#hf_IsNotif').val() == 'True') {
                listimportedNotifvehicles(contentno);
            }
            else {
                if ($('#hf_IsCandidate').val() != 'True') {
                    listimportedSOvehicles(revId);
                }
            }
        },
        error: function (err) {
            stopAnimation();
            console.error(err);
        },
        complete: function () {
            resetcopyroute();
            stopAnimation();
        }
    });

}

function listimportedSOvehicles(RevID, $this) {
    openContentLoader('html');
    $.ajax({
        url: '../Application/ListImportedVehicleConfiguration',
        type: 'POST',
        async: false,
        data: { apprevisionId: RevID },
        beforeSend: function (data) {
        },
        success: function (data) {
            var currentTab = (typeof $this == 'undefined') ? $('#tab_3'): $this.closest('.tab_content1');
            currentTab.html(data);
            //SOvalidationfun1();
            ListImportedVehicleConfigurationInit();
        },
        error: function (err) {
            closeContentLoader('html');
            console.error(err);
        },
        complete: function () {
            closeContentLoader('html');
        }
    });
}

function listimportedVR1vehicles(RevID, $this) {
    openContentLoader('html');
    var redDetails = $('#Reduceddetailed').val();
    var vr1contrefno = $('#VR1ContentRefNo').val();
    var vr1versionid = $('#VersionId').val();
    $.ajax({
        url: '../Application/ListImportedVehicleConfiguration',
        type: 'POST',
        async: false,
        data: { apprevisionId: RevID, VRAPP: true, ContentRefNo: vr1contrefno, verId: vr1versionid },
        beforeSend: function (data) {
            // startAnimation();
        },
        success: function (data) {
            var currentTab = (typeof $this == 'undefined') ? $('#tab_3') : $this.closest('.tab_content1');
            currentTab.html(data);

            VR1validationfun1(redDetails);
            ListImportedVehicleConfigurationInit();
        },
        error: function (err) {
            closeContentLoader('html');
            console.error(err);
        },
        complete: function () {
            closeContentLoader('html');
        }
    });
}

function listimportedNotifvehicles(contentno, $this) {
    openContentLoader('html');
    $.ajax({
        url: '../Application/ListImportedVehicleConfiguration',
        type: 'POST',
        async: false,
        data: { ContentRefNo: contentno, IsNotif: true },
        beforeSend: function (data) {
            // startAnimation();
        },
        success: function (data) {
            var currentTab = (typeof $this == 'undefined') ? $('#tab_2') : $this.closest('.tab_content1');
            currentTab.html(data);
            $('#tab_3').hide();
            NotifValidation();
            ListImportedVehicleConfigurationInit();
        },
        error: function (err) {
            closeContentLoader('html');
            console.error(err);
        },
        complete: function () {
            closeContentLoader('html');
        }
    });
}

function VehicleonChange(RoutePartID, VehicleID) {

    partid = $("#RoutePartsId" + VehicleID + " option:selected").val();
    var ddID = "RoutePartsIdType" + VehicleID;
    $("#" + ddID).val(partid);
    var routeType = $("#RoutePartsIdType" + VehicleID + " option:selected").text();


    arr1 = [VehicleID];
    arr2 = [partid];
    arr3 = [routeType];
    VehicleArr.push(arr1);
    RouteArr.push(arr2);
    RouteTypeArr.push(arr3);

}

function NavigateToRoute() {

    var status = $('#SortStatus').val();
    if (status == "ViewProj" || status == "CreateSO" || status == "MoveVer") {
        SORTShowSORoutePage();
        hidetabs();
        $('#4').show();

        $('#divMap1').show();
        $(".tab_content").each(function () {
            $(this).hide();
        });
        $('.t').addClass('nonactive');
        $('#tab_wrapper ul li').eq(4).removeClass('nonactive');
        var test = title + ' - ' + $('#tab_wrapper ul li').eq(4).find('.tab_centre').text();
        $('#pageheader').find('h3').html(test);
        CloneRoutesSort();
        $('#ChkNewConfiguration').attr('checked', true);
    } else {
        hidetabs();
        $('#tab_4').show();
        $('#divMap1').show();
        $(".tab_content").each(function () {
            $(this).hide();
        });
        $('.t').addClass('nonactive');
        $('#tab_wrapper ul li').eq(2).removeClass('nonactive');
        var test = title + ' - ' + $('#tab_wrapper ul li').eq(2).find('.tab_centre').text();
        $('#pageheader').find('h3').html(test);
        $('#RouteMap').html('');
        CloneRoutes();
        $("#hdnVRRouteTab").val("True");
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        if (ApplicationRevId == 0) {
            ApplicationRevId = $('#hf_ApprevisionId').val();
        }
        if (ApplicationRevId != 0) {
            $('#mapTitle').html('');
            var flag = pageflag;
            var link = '../Application/SoRoute?pageflag = "_pageflag"';
            $("#leftpanel").load(link.replace("_pageflag", flag), function () {
                $('#leftpanel_quickmenu').html('');
                $("#leftpanel").show();
                CheckSessionTimeOut();
            });
        }

        else {

            $("#leftpanel").hide();
        }
        addscroll();
        $('#ChkNewConfiguration').attr('checked', true);
    }
    if (sessionRouteFlag == 3)
        CloneRoutesNotify();
}

function AmendReg(id) {
    $('#AmendVehicleId').val(id);
    $("#dialogue").load('../VehicleConfig/PartialViewLayout', function () {
        $.ajax({
            url: '../VehicleConfig/RegistrationConfiguration',
            data: { vehicleId: id, RegBtn: false, isVR1: true, isAmend: true },
            type: 'GET',
            beforeSend: function () {
                startAnimation();
                $(".dyntitleConfig").html("Edit registration");
                removescroll();
                DisableBackButton();
            },
            success: function (result) {
                $("#Config-body").html(result);
            },

            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                stopAnimation();
                $("#dialogue").show();
                $("#overlay").show();

            }

        });
    });
}

function CloneVehicle(vehicleId, vehicleName, $this) {
    WarningCancelBtn();
    var VR1Appl = $('#VR1Appl').val();
    var revId = $('#hf_ApplRevId').val();
    var contentno = $('#hf_ContentRefNo').val();
    var iscandidate = $('#hf_IsCandidate').val();
    var notifId = $('#NotificatinId').val();
    openContentLoader('html');
    $.ajax({
        url: '../VehicleConfig/CopyVehicleFromList',
        type: 'POST',
        async: false,
        data: { vehicleId: vehicleId, ApplnRevId: 1, isNotif: false, isVR1: VR1Appl, ContentRefNo: contentno, IsCandidate: iscandidate, NotificationId: notifId },
        beforeSend: function (result) {
            
        },
        success: function (data) {
            closeContentLoader('html');
            if (data.success != 0) {
                showToastMessage({
                    message: "Vehicle has been successfully copied.",
                    type: "success"
                });
                if ($('#hf_IsCandidateModify').val() == 'True' || $('#hf_IsCandidate').val() == 'True') {
                    Show_CandidateRTVehicles();
                }
                else if (VR1Appl == "True") {
                    listimportedVR1vehicles(revId, $this);
                }
                else if ($('#hf_IsNotif').val() == 'True') {
                    listimportedNotifvehicles(contentno, $this);
                }
                else {
                    listimportedSOvehicles(revId, $this);

                }
            } else {
                showToastMessage({
                    message: "The vehicle is not copied. Please try again.",
                    type: "error"
                })
            }
        },
        error: function (err) {
            //location.reload();
            console.error('CloneVehicle---',err);
        },
        complete: function () {
            
        }
    });

}

function SelectCurrentMovements() {
    var _isvr1Candi = $('#VR1Applciation').val();
    ;
    $('#ViewRouetDetail').hide();
    $('#divViewRouetDetail').hide();
    $('#RoutePart').hide();
    $('#SelectCurrentMovementsVehicle').show();
    $('#previousMovementListDiv').html('');
    $('#previousMovementListDiv').hide();
    $("#IsCreateApplicationRoute").val("true");
    $('#divCurrentMovement').hide();
    var wstatus = 'ggg';
    $('#SelectCurrentMovementsVehicle').load('../SORTApplication/SORTAppCandidateRouteVersion?AnalysisId=' + analysis_id + '&Prj_Status=' + encodeURIComponent(wstatus) + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&projectid=' + projectid + '&IsCandPermision=' + 'true' + '&IsCurrentMovenet=true', {},
        function () {

            //ClosePopUp();
            $("#dialogue").html('');
            $("#dialogue").hide();
            $("#overlay").hide();
            addscroll();
            stopAnimation();
            LoadAppAndMoveVersions(hauliermnemonic, esdalref, projectid);
        });
    startAnimation();

}
var frmPrvMove = false;
function PrevMovement() {
    $("#SelectCurrentMovementsVehicle1").html('');
    $("#SelectCurrentMovementsVehicle2").html('');
    $('#tab_3').html("");
    $("#IsPrevMoveOpion").val("vehicleMoveOpion");
    $("#StruRelatedMove").val("No");
    $("#IsCreateApplicationRoute").val("true");
    $('#SelectCurrentMovementsVehicle').html('');
    //startAnimation();
    $('#leftpanel').hide();
    frmPrvMove = true;

    $('#tab_3').html('');

    var isVR1 = $('#vr1appln').val();
    if (isVR1 == 'True') {
        UsePreviousMovement();
    }
    else {
        $("#IsSOroute").val(false);
        $("#IsSOvehicle").val(true);
        if ($('#hf_IsSort').val() == 'True') {
            SelectPrevtMovementsVehicleImport();
        }
        else {
            UseMovement();
        }
    }
    $('#ShowSelectedFleetConfiguration').hide();
    stopAnimation();
}

function UseMovement() {
    // $('#VehicleSOApplication').hide();
    var OrgID = $('#OrgID').val() ? $('#OrgID').val() : 0;
    var VIsRoutePrevMoveOpion = false;

    if ($("#IsPrevMoveOpion").val() == "routeMoveOpion")
        VIsRoutePrevMoveOpion = true;
    $.ajax
        ({
            url: '../Movements/MovementList',
            data: { MovementListForSO: true, showrtveh: 1, OrgID: OrgID, IsSOvehicle: true, IsVehiclePrevMoveOpion: VIsRoutePrevMoveOpion },
            type: 'GET',
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                //$('#MovementVehConfiguration').show();
                //$('#MovementVehConfiguration').html('');
                //$('#MovementVehConfiguration').html($(page).find('#div_movement_list').html(), function () {
                //});
                //$('#tab_3').show();
                //$('#tab_3').html('');
                //$('#MovementVehConfiguration').appendTo('#tab_3');
                $('#tab_3').show();
                //$('#tab_3').html('');
                //$('#tab_3').html($(page).find('#divforsofilter').html(), function () {
                //});

                $('#tab_3').html($(page).find('#divforsofilter').html(), function () {
                });
                $('#tab_3').find("form").removeAttr('action', "");
                $('#tab_3').find("form").submit(function (event) {
                    AdvSearchforSOApplivation();
                    event.preventDefault();
                });
                removeHLinks();
                PaginateGridsomovement();
                //PaginateGridSO();
                fillPageSizeSelect();

            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
            }
        });
}
function UsePreviousMovement() {
    $.ajax({
        url: '../Movements/MovementList',
        data: { VR1route: true, showrtveh: 1 },
        type: 'GET',
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#tab_3').show();
            $('#tab_3').html($(page).find('#divforsofilter').html(), function () {
            });
            $('#tab_3').find("form").removeAttr('action', "");
            $('#tab_3').find("form").submit(function (event) {
                AdvSearchforSOApplivation();
                event.preventDefault();
            });
            //removeHLinks();
            //PaginateGridSO();
            fillPageSizeSelect();

        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
            PreviousMovePagination();
        }
    });
}



function SelectPrevtMovementsVehicleImport() {
    var url = '../SORTApplication/SORTInbox';
    $.ajax({
        url: url,
        type: 'GET',
        cache: false,
        data: { structID: null, pageSize: 10, IsPrevtMovementsVehicle: true },
        beforeSend: function () {
            startAnimation('Loading related movements...');
            if ($('#SearchPrevMoveVeh').val() == "No") {
                //ClearAdvancedDataSORT();
                //ResetDataSORT();
            }
        },
        success: function (result) {
            $('#tab_2').hide();
            $('#haulier_app_rev').hide();
            $('#App_Candidate_Ver').hide();
            $('#sortInboxTotalCount').hide();
            $('#App_Movment_Ver').hide();
            $('#generalDetailDiv').hide();
            $('#previousMovementListDiv').html('');
            $('#previousMovementListDiv').show();
            $('#banner-container').find('div#sortFilters').remove();
            $('div#sortFilters,#filter_SORT').remove();

            var mvmntlist = $(result).find('#sort-movement-table');
            $(mvmntlist).appendTo('#previousMovementListDiv');

            var importFromDiv = '#div_MoveList_advanceSearch';
            var div = '#divCurrentMovement';
            $(div).html($(result).find(importFromDiv).html(), function () {
                event.preventDefault();
            });
            $(div).find("form").removeAttr('action', "");
            $(div).find("form").submit(function (event) {
                event.preventDefault();
            });

             var filters = $(result).find('div#sortFilters');
            $(filters).find('form .filters:first').remove();
            $(filters).find('form .filters:first').remove();
            if ($('#IsSortUser').val() == 'True')
                $(filters).find('form .filters:last').remove();
            $(filters).insertAfter('#banner');
            $("#Mapfilterdiv").hide();
            $('.divViewMapFilter').remove();
        },
        complete: function () {
            if ($('#SearchPrevMoveVeh').val() == "Yes") {
                ShowAdvanced();
            }
            stopAnimation();
            scrolltotop();
        }
    });
}

function SelectCurrentMovementsVehicle() {
    $('#tab_2').hide();
    $('#generalDetailDiv').show();
    $('#generalDetailDiv').find($('#divbtn_prevmove2')).remove();
    $('#SelectCurrentMovementsVehicle').show();
    $("#IsPrevMoveOpion").val(false);
    $("#back_currentmovmnt").hide();
    $("#back_currentmovmnt_vehicle").show();
    if ($("#back_currentmovmnt_vehicle").length == 0) {
        $('#generalDetailDiv').append('<div class="button main-button mr-0 col-lg-2" id="back_currentmovmnt_vehicle"><div class="button main-button mr-0 col-lg-3 pt-5"><button class="btn btn-outline-primary btn-normal" style="position:absolute;width:15%;" id="showcandidate" role="button"  aria-pressed="true">BACK</button></div></div>');
    }
    startAnimation();
    var wstatus = '';
    $('#SelectCurrentMovementsVehicle').load('../SORTApplication/SORTAppCandidateRouteVersion?AnalysisId=' + analysis_id + '&Prj_Status=' + encodeURIComponent(wstatus) + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&projectid=' + projectid + '&IsCandPermision=' + 'true' + '&IsVehicleCurrentMovenet=true', {},
        function () {
            $("#dialogue").html('');
            $("#dialogue").hide();
            $("#overlay").hide();
            $("#previousMovementImportBack").hide();
            SORTAppCandidateRouteVersionInit();
            LoadAppAndMoveVersions(hauliermnemonic, esdalref, projectid);
            
            addscroll();
            stopAnimation();
        });
}

function CurreMovemenVehicleList(V_ReviosionId, VVRLIST_type) {
    var rtrevisionId = V_ReviosionId;
    var iscandlastversion = $('#IsCandVersion').val();
    var plannruserid = $('#PlannrUserId').val();
    var appstatuscode = $('#AppStatusCode').val();
    var movversionno = $('#versionno').val();
    var movdistributed = $('#IsMovDistributed').val();
    var sonumber = $('#SONumber').val();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var prjstatus = $('#Proj_Status').val();

    removescroll();
    //$("#overlay").show();
    //$('.loading').show();
    $('#div_ListSOMovements').html('');

    $.ajax({
        url: "../SORTApplication/CandiVersionVehicleList",
        type: 'post',
        data: { revisionid: rtrevisionId, CheckerId: _checkerid, CheckerStatus: _checkerstatus, IsCandLastVersion: iscandlastversion, planneruserId: plannruserid, appStatusCode: appstatuscode, SONumber: sonumber, VRLIST_type: VVRLIST_type, IsIsCreateApplication: false },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {

            //$('#SelectCurrentMovementsVehicle1').hide();
            //$('#SelectCurrentMovementsVehicle2').hide();
            //$('#SelectCurrentMovementsVehicle').hide();
            $('#generalDetailDiv').hide();
            $('#tab_3').html(data);
            $('#tab_3').show();

            //$("#overlay").show();
            //$("#dialogue").show();
            //$('.loading').hide();
        },
        error: function () {
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function SelectPrevitMovementsVehicle(VAnalysisId, VPrj_Status, Vhauliermnemonic, Vesdalref, Vprojectid) {
    $('#tab_3').html('');
    $('#SelectCurrentMovementsVehicle1').html('');
    $('#SelectCurrentMovementsVehicle2').html('');
    $("#PrevMove_projectid").val(Vprojectid);
    $("#PrevMove_hauliermnemonic").val(Vhauliermnemonic);
    $("#PrevMove_esdalref").val(Vesdalref);
    $("#IsPrevMoveOpion").val(true);
    $("#showcandidate").hide();
    $('#tab_2').hide();
    $('#SelectCurrentMovementsVehicle').show();

    //WarningCancelBtn();
    startAnimation();
    var wstatus = '';
    $('#SelectCurrentMovementsVehicle').html('');
    $('#SelectCurrentMovementsVehicle').load('../SORTApplication/SORTAppCandidateRouteVersion?AnalysisId=' + VAnalysisId + '&Prj_Status=' + VPrj_Status + '&hauliermnemonic=' + Vhauliermnemonic + '&esdalref=' + Vesdalref + '&projectid=' + Vprojectid + '&IsCandPermision=' + 'true' + '&IsVehicleCurrentMovenet=true', {},
        function () {
            //$("#dialogue").html('');
            //$("#dialogue").hide();
            $("#overlay").hide();
            $('#generalDetailDiv').show();
            $('#generalDetailDiv').find($('#divbtn_prevmove2')).remove();
            $('#previousMovementListDiv').html('');
            $('#previousMovementListDiv').hide();
            addscroll();
            stopAnimation();
            SORTAppCandidateRouteVersionInit();
            LoadAppAndMoveVersions(Vhauliermnemonic, Vesdalref, Vprojectid);
        });
}

//function remove href from pagination ul li
function removeHrefLinksVehicleConfig() {
    $('#previousMovementListDiv').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
}
function openFilters() {
    var filterWindowWidth = "600px";
    document.getElementById("filters").style.width = filterWindowWidth;
    document.getElementById("banner-container").style.filter = "brightness(0.5)";
    document.getElementById("banner-container").style.background = "white";

    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("filters").style.width = "200px";
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function closeFilters() {
    document.getElementById("filters").style.width = "0";
    document.getElementById("banner-container").style.filter = "unset";
    document.getElementById("banner-container").style.background = "linear-gradient(0deg, rgba(255, 255, 255, 1) 0%, rgba(201, 205, 229, 0.44) 100%)";
}

// showing filter-roads in side-nav
function viewSORTAdvHaulier(_this) {
    if (typeof _this == 'undefined') {
        if ($('#viewSORTAdvHaulier').is(':visible')) {
            $('#viewSORTAdvHaulier').css('display', "none");
            $('#chevlon-up-icon2').css('display', "none");
            $('#chevlon-down-icon2').css('display', "block");
        }
        else {
            $('#viewSORTAdvHaulier').css('display', "block");
            $('#chevlon-up-icon2').css('display', "block");
            $('#chevlon-down-icon2').css('display', "none");
        }
    } else {
        var parentElem = $(_this).closest('.sidenav');
        if (parentElem.find('#viewSORTAdvHaulier').is(':visible')) {//style.display !== "none"
            parentElem.find('#viewSORTAdvHaulier').css('display', "none");
            parentElem.find('#chevlon-up-icon2').css('display', "none");
            parentElem.find('#chevlon-down-icon2').css('display', "block");
        }
        else {
            parentElem.find('#viewSORTAdvHaulier').css('display', "block");
            parentElem.find('#chevlon-up-icon2').css('display', "block");
            parentElem.find('#chevlon-down-icon2').css('display', "none");
        }
    }
}

function OnFailure(result) {
    closeFilters();
    stopAnimation();
}
function OnBegin(result) {
    startAnimation();
}

function FillNormalFilter() {

    $('#SearchPrevMoveVeh').val("Yes");
    $('#viewSORTAdvHaulier').find('input:text').each(function () {
        var id = $(this).attr('id');
        var value = $(this).val();
        $('#div_normal_hidden').append('<input type="hidden" id="' + id + '" name="' + id + '" value="' + value + '"></hidden>');
    });

    $('#viewSORTAdvHaulier').find('input:checkbox').each(function () {
        var id = $(this).attr('id');
        var value = $(this).is(':checked');
        $('#div_normal_hidden').append('<input type="hidden" id="' + id + '" name="' + id + '" value="' + value + '"></hidden>');
    });

}
function ClearAdvancedSORT() {
    ClearAdvancedDataSORT();
    ResetDataSORT();
    //$("#frmFilterMoveInboxSORT").submit();

    return false;
}
function ClearAdvancedDataSORT() {
    var _advInboxFilter = $('#viewSORTAdvHaulier');
    _advInboxFilter.find('input:text').each(function () {
        $(this).val('');
    });

    _advInboxFilter.find('select').each(function () {
        $(this).val('0');
    });
}

function ResetDataSORT() {
    $.ajax({
        url: '../SORTApplication/ClearInboxAdvancedFilterSORT',
        type: 'POST',
        success: function (data) {
            //ClearAdvancedData();
        }
    });
}
function OnSuccess(result) {

    if (result == 1) {
        if ($('#SearchPrevMoveVeh').val() == "Yes") {
            closeFilters();
            SelectPrevtMovementsVehicleImport();
        }
        else { location.reload(); }
    }
}
