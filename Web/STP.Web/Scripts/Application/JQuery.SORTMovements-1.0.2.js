function SORTShowSOVehiclePage() {
    $("#SelectCurrentMovementsVehicle1").html('');
    $("#SelectCurrentMovementsVehicle2").html('');
    $('#SelectCurrentMovementsVehicle1').html('');
    $('#SelectCurrentMovementsVehicle2').html('');
    $('#App_Candidate_Ver').html('');
    $('#StruRelatedMov_viewDetails').html('');
    AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var VR1Applciation = $('#VR1Applciation').val();

    //This code is to set adv search param to null
    var _advFilter = $('#div_so_advanced_search');
    _advFilter.find('input:text').each(function () {
        $(this).val('');
    });
    _advFilter.find('input:radio').eq(0).prop('checked', true);
    _advFilter.find('input:checkbox').attr('checked', false);

    _advFilter.find('input:checkbox').closest('tr').find('input:text').attr('disabled', 'disabled');

    $.ajax({
        url: '../Movements/ClearSOAdvancedFilter',
        type: 'POST',
        success: function (data) {
        }
    });
    //

    if (AppRevId != 0) {
        $(".tab_content1").each(function () {
            $(this).hide();
        });
        $("li[id=3]").addClass('t');

        $('#pageheader').find('h3').text(title + ' - ' + $("li[id=3]").find('.tab_centre').text());
        if (VR1Applciation == "False") {
            if (status == "ViewProj" || status == "MoveVer" || status == "Revisions") {
            } else {
                ShowSOVehiclePage();
            }
        } else {
            if (status == "ViewProj" || status == "MoveVer" || status == "Revisions") {
                $("#wraper_leftpanel_content").hide();
            }
        }
        if (status == "CreateVR1") {
            $(".tab_content1").each(function () {
                $(this).hide();
            });
            if ($("#SupplInfo input").length > 0) {
                $("#SupplInfo").show();
            }
            else {
                ShowVR1SupplInfo();
            }
        }
        if (AppEdit == "True") {
            $('#validationSucced').show();
        }
        scrolltotop();
    }

}
$(document).ready(function () {

    $('body').on('click', '#Mov_Agree', function (e) {
        e.preventDefault();
        Mov_Agree_funt_chck(this);
    });
    $('body').on('click', '#Mov_Unagree', function (e) {
        e.preventDefault();
        Mov_Unagree_funt(this);
    });
    $('body').on('click', '#Mov_Withdraw', function (e) {
        e.preventDefault();
        Mov_Withdraw_funt(this);
    });
    $('body').on('click', '#btndistributemovement', function (e) {
        e.preventDefault();
        DistributeMovement(this);
    });
    $('body').on('click', '#btn-dismov', function (e) {
        e.preventDefault();
        DistributeMovement(this);
    });
    $('body').on('click', '.btn-approve-vr1', function (e) {
        e.preventDefault();
        VR1approval(this);
    });
    $('body').on('click', '.btn-generate-vr1-number', function (e) {
        e.preventDefault();
        GenerateVR1Number(this);
    });
    $('body').on('click', '.btn-generate-vr1-document', function (e) {
        e.preventDefault();
        GenerateVR1Document(this);
    });
    //-------Below click events for tab click feature , copied from CustomContraol.cs file
    $('body').on('click', '.btn-display-proj-view', function (e) {  DisplayProjView(); });
    $('body').on('click', '.btn-display-general', function (e) {  DisplayGeneral(); });
    $('body').on('click', '.btn-display-history', function (e) {  DisplayHistory(); });
    $('body').on('click', '.btn-display-appl-summary', function (e) {  DisplayApplSummary(); });
    $('body').on('click', '.btn-display-vehroute', function (e) {  DisplayVehroute(); });
    $('body').on('click', '.btn-list-route-assessment', function (e) {  ListRouteAssessment(); });
    $('body').on('click', '.btn-show-sortvr1-vehicle', function (e) {  ShowSORTVR1Vehicle(); });
    $('body').on('click', '.btn-show-sortvr1-route', function (e) {  ShowSORTVR1Route(); });
    $('body').on('click', '.btn-display-collab-status', function (e) { e.preventDefault(); DisplayCollabStatus(); });
    $('body').on('click', '.btn-display-trans-status', function (e) {  DisplayTransStatus(); });
    $('body').on('click', '.btn-to-haulier', function (e) {  toHaulier(); });
    $('body').on('click', '.btn-show-so-vehicle-page', function (e) {  ShowSOVehiclePage(); });
    $('body').on('click', '.btn-showsoroutepage', function (e) {  ShowSORoutePage(); });
    $('body').on('click', '.btn-showsoroutepage', function (e) {  ShowRouteAssessment(); });
    $('body').on('click', '.btn-showcheckdiff', function (e) {  ShowCheckDiff(); });
    $('body').on('click', '.btn-displayspecorder', function (e) {  DisplaySpecOrder(); });
    $('body').on('click', '.btn-displayhistory', function (e) { DisplayHistory(); });
    $('body').on('click', '.btn-displayprojview', function (e) {  DisplayProjView(); });
    $('body').on('click', '.btn-sortshowsovehiclepage', function (e) {  SORTShowSOVehiclePage(); });
    $('body').on('click', '.btn-sortshowsoroutepage', function (e) {  SORTShowSORoutePage(); });

    // Below methods not found in any js files or xslt files. Need to verify
    $('body').on('click', '.btn-showstructrelatedmovements', function (e) {  showStructRelatedMovements(); });
    $('body').on('click', '.btn-load_routeAnalysisSORT', function (e) {  load_routeAnalysisSORT(); });
    $('body').on('click', '.btn-load_routeanalysissort', function (e) {  load_routeAnalysisSORT(); });
    $('body').on('click', '.btn-show_candidatertvehicles', function (e) {  Show_CandidateRTVehicles(); });
    $('body').on('click', '.btn-bindrouteparts', function (e) {  BindRouteParts(); });
    $('body').on('click', '.btn-viewprojectgeneraldetails', function (e) {  ViewProjectGeneralDetails(); });
    $('body').on('click', '#span1-ViewVehicleSORT', function (e) {
        
        var VehicleId = $(this).data('vehicleid');
        ViewVehicleSORT(VehicleId);
    });
    $('body').on('click', '#btn-sp', function (e) {
        btnsp_click(this);
    });
});
function SORTShowSORoutePage() {
    $('#SelectCurrentMovements1').hide();
    $('#SelectCurrentMovements2').hide();
    $('#SelectCurrentMovementsVehicle1').html('');
    $('#SelectCurrentMovementsVehicle2').html('');
    $('#App_Candidate_Ver').html('');
    $('#StruRelatedMov_viewDetails').html('');
    AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var VR1Applciation = $('#VR1Applciation').val();

    //This code is to set adv search param to null
    var _advFilter = $('#div_so_advanced_search');
    _advFilter.find('input:text').each(function () {
        $(this).val('');
    });
    _advFilter.find('input:radio').eq(0).prop('checked', true);
    _advFilter.find('input:checkbox').attr('checked', false);

    _advFilter.find('input:checkbox').closest('tr').find('input:text').attr('disabled', 'disabled');

    $.ajax({
        url: '../Movements/ClearSOAdvancedFilter',
        type: 'POST',
        success: function (data) {
            //ClearAdvancedData();
        }
    });
    //
    if (AppRevId != 0) {
        $(".tab_content1").each(function () {
            $(this).hide();
        });
        $("li[id=4]").addClass('t');
        $('#pageheader').find('h3').text(title + ' - ' + $("li[id=4]").find('.tab_centre').text());
        $("#hdnVRRouteTab").val("True");
        ShowSORoutePage();
        if (AppEdit == "True") {
            $('#validationSucced').show();
        }
        CheckSessionTimeOut();
        scrolltotop();
        SOvalidationfun1();
    }
}
function CloseOrgSavePopup() {
    WarningCancelBtn();
    $("#btnNextSORTGeneralSave").show();
    ViewGeneralTabAuto();

}
function ShowVR1GeneralPage() {
    $("#leftpanel").hide();
    $('#leftpanel_div').hide();
    $('#leftpanel_quickmenu').hide()
    startAnimation()
    $('#General').load('../SORTApplication/SORTCreateVR1General', { ProjectID: projectid },
        function () {
            $('#General').show();
            if (projectid == "0") {
                FillHaulierDetailsVr1();
            }
            if ($('#validationSucced').text() == "This application is ready to be submitted") {
                $('#VR1Submit').show();
            }
            stopAnimation()
            CheckSessionTimeOut();
        });

}
function ShowVR1SupplInfo() {
    $("#leftpanel").hide();
    $('#leftpanel_div').hide();
    $('#leftpanel_quickmenu').hide()
    startAnimation()
    $('#SupplInfo').load('../Application/VR1EditSupplementaryDetails', { apprevisionId: revisionId },
        function () {
            $('#SupplInfo').show();
            stopAnimation()
            CheckSessionTimeOut();
        });
}
function ShowSOHaulierOrgPage() {
    var sortApp = "";
    var mode = "";
    var showHaulCnt = false;
    if (status == "CreateSO" && revisionId != 0) {
        mode = "Edit";
        sortApp = "SORTSO";
        if (cloneapprevid != 0) {
            showHaulCnt = true;
        }
    }
    if (status == "CreateVR1" && revisionId != 0) {
        mode = "Edit";
        sortApp = "SORTSO";
    }
    if (OrgID == null || OrgID == 0) {
        OrgID = "";
        mode = "SORTSO";
    }

    $("#overlay").show();
    $('.loading').show();
    $('#HaulOrg').load('../Organisation/CreateOrganisation', { mode: mode, orgID: OrgID, sortApp: sortApp, RevisionId: revisionId, SortStatus: status, showHaulCnt: showHaulCnt },
        function () {
            $('#HaulOrg').show();
            if (revisionId == 0) {
                $("#leftpanel").html('');
                $("#leftpanel").load('../SORTApplication/SORTLeftPanel', { Display: "HaulOrg", pageflag: 2 },
                    function () {
                        $('#leftpanel_quickmenu').html('');
                        $("#leftpanel").show();
                        ApplSummaryLeftPanelInit();
                    });
            } else {
                $("#leftpanel").html('');
            }
            $(".form .body").css("border-right", "0px");
            $(".form .body").css("border-left", "0px");
            $(".form .body").css("border-bottom", "0px");
            $(".form .body").css("background", "none");
            $(".form .body").css("border-bottom-right-radius", "0px");
            $(".form .body").css("border-bottom-left-radius", "0px");
            $(".form .body").css("background", "none");
            $(".form").css("background", "none");

            $('#HaulOrg').find('#Head_headgrid').hide();

            $("#overlay").hide();
            $('.loading').hide();
            CheckSessionTimeOut();
        });
}
function ShowSOGeneralPage() {
    var OrgID = $('#OrganisationId').val();
    $("#leftpanel").hide();
    $('#leftpanel_div').hide();
    $('#leftpanel_quickmenu').hide()
    startAnimation()
    $('#General').load('../SORTApplication/SORTSOGeneral', { saveFlagSOGeneral: false, RevisionId: revisionId, Organisation_ID: OrgID },
        function () {
            $('#General').show();
            stopAnimation()
            CheckSessionTimeOut();
        });
}
function ShowSOVehiclePage() {
    LoadSelectedFleetConfigurationSort();
    scrolltotop();
}
function LoadSelectedFleetConfigurationSort() {
    var status1 = "";
    var AppRevId = $('#ApprevId').val() ? $('#ApprevId').val() : 0;
    var status = $('#SortStatus').val();
    if (AppRevId == 0) {
        AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    }
    var VR1Applciation = $('#VR1Applciation').val();

    var routePartId = 0;
    var isVR1 = false;
    if (status == null || status == "" || status == "CreateSO") {
        status1 = "";
    } else if (OrgID != 0 && status == "ViewProj") {
        status1 = status;
    } else if (VR1Applciation == 'True' || VR1Applciation == 'true') {
        isVR1 = true;
        status1 = status;
    } else if (VR1Applciation != 'True' || VR1Applciation != 'true') {
        versionId = 0;
    }
    $.ajax({
        url: "../Application/ListImportedVehicleConfiguration",
        type: 'GET',
        async: false,
        cache: false,
        //contentType: 'application/json; charset=utf-8',
        data: { apprevisionId: AppRevId, routepartId: routePartId, VRAPP: isVR1, status: status1 },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            $('#tab_3').html('');
            $('#tab_3').html(data);
            $('#tab_3').show();

            if (status == "ViewProj" || status == "MoveVer") {
                if ($('#ModelCntVeh').val() == 0) {
                    $('#tab_3').find('#h4_blank_header_Veh').html('');
                    $('#tab_3').find('#h4_blank_header_Veh').append('<h4 style="color:#414193;margin: 0px 0;font-size: 1.0em;">There are no vehicle configurations for this application. </h4> <br/>');
                } else {
                    $('#tab_3').find('#header').html('');
                    $('#tab_3').find('#header').append('<h4 style="color:#414193;margin: 0px 0;font-size: 1.0em;">Listed below, are the vehicle configurations defined for this application. </h4> <br/>');
                }
                $("#leftpanel").html('');
                $('#tab_3').find('#table_veh_detail').find("button").hide();
                $('#tab_3').find('#copy').hide();
            }
            else {
                $('#leftpanel_quickmenu').html('');
                $("#leftpanel").html('');

                $("#leftpanel").load('../Application/SoVehicle', function () {
                    $("#leftpanel").show();
                });
                $("#ChkNewroute1,#ChkNewroute2,#ChkNewroute3").attr("checked", false);
            }
            ListImportedVehicleConfigurationInit();
        },
        complete: function () {
            stopAnimation();
            CheckSessionTimeOut();
        },
        error: function () {
            location.reload();
        }
    });
}
function ShowSORoutePage() {
    CloneRoutesSort();
    scrolltotop();
}
function CloneRoutesSort() {
    $('#pageheader').html(''); $('#pageheader').html('<h3> Apply for SO  - Route</h3>');
    var AppRevId = $('#ApprevId').val() ? $('#ApprevId').val() : 0;
    if (AppRevId == 0) {
        AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    }
    var vr1 = $('#vr1appln').val();
    $.ajax({
        url: "../Application/ListImportedRouteFromLibrary",
        type: 'post',
        async: false,
        //contentType: 'application/json; charset=utf-8',
        data: { apprevisionId: AppRevId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {

            $('#tab_4').show();
            $('#RoutePart').html('');

            $('#RoutePart').show();
            $('#RoutePart').html(data);
            if (status == "ViewProj" || status == "MoveVer") {
                $("#leftpanel").html('');
                $('#tab_4').find('#tblroutelist').find("button").hide();
                $('#tab_4').find('#sort').hide();

                if ($('#ModelCnt').val() == 0) {
                    $('#ShowList').find('#h4_blank_header').html('');
                    $('#ShowList').find('#h4_blank_header').append('<h4 style="color:#414193;margin: 0px 0;font-size: 1.0em;">There are no route parts for this application.  </h4> <br/>');
                } else {
                    $('#ShowList').find('#header').html('');
                    $('#ShowList').find('#header').append('<h4 style="color:#414193;margin: 0px 0;font-size: 1.0em;">Listed below, are the routes defined for this application. </h4> <br/>');
                }

            } else {
                $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);


                startAnimation();

                $("#leftpanel").html('');
                $("#leftpanel").load('../Application/SoRoute', {}, function () {
                    $("#leftpanel").show();
                    stopAnimation();
                });
                ListImportedRouteFromLibraryInit();
                CheckSessionTimeOut();
            }
        },
        complete: function () {
        },
        error: function () {
        }
    });
}
function ShowSORTVR1Vehicle() {
    var AppRevId = $('#ApprevId').val() ? $('#ApprevId').val() : 0;
    var Content_Ref = $('#VR1ContentRef').val() ? $('#VR1ContentRef').val() : "";
    var versionId = $('#versionId').val();
    var status = $('#SortStatus').val();
    var isVR1 = true;
    $('#leftpanel').hide();
    $('#leftpanel_quickmenu').hide();
    $("#overlay").show();
    $('.loading').show();
    var vr1app = $('#VR1Applciation').val();
    if (vr1app == 'True' || vr1app == 'true') {
        $('#tab_3').show();
        startAnimation();
        $('#tab_3').load('../Application/HaulierApplRouteParts', { RevisionId: AppRevId, VersionId: versionId, SubmitVR1: true, Type: "Vehicle" }, function () {
            $('#Cand_RouteVehicles').show();
            $("#overlay").hide();
            $('.loading').hide();
            CheckSessionTimeOut();
            HaulierApplVehiclePartsInit();
            stopAnimation();
        });
    }
}
function ShowSORTVR1Route() {
    $('#leftpanel').hide();
    $('#leftpanel_quickmenu').hide();
    //if (!agreedRoute) {
    var AppRevId = $('#ApprevId').val() ? $('#ApprevId').val() : 0;
    if (AppRevId == 0) {
        AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    }
    var Content_Ref = $('#VR1ContentRef').val() ? $('#VR1ContentRef').val() : "";
    var versionId = $('#versionId').val();
    var VR1Applciation = $('#VR1Applciation').val();
    //-----------

    $.ajax({
        url: '../Application/AgreedRoutes',
        type: 'POST',
        data: { VersionId: versionId, revisionid: AppRevId, VR1: VR1Applciation, ContentRefNo: Content_Ref },
        beforeSend: function () {
            startAnimation();
        },
        success: function (html) {
            $('#tab_4').show();
            $('#tab_4').html(html);
            //$('#RoutePart').html(html);
            CheckSessionTimeOut();
            Sort_AgreedRouteInit();
        },
        error: function () {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function close_alert() {
    $("#dialogue").html('');
    $("#dialogue").hide();
    $("#overlay").hide();
    addscroll();
    resetdialogue();
    $('#leftpanel').find($("input:radio").attr("checked", false));
}
function DisplayGeneral() {

    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var VR1Applciation = $('#VR1Applciation').val();
    var sortentered = $('#EnteredBySort').val();
    if (VR1Applciation == "True") {
        var PlannerId = $('#PlannrUserId').val();
        var revision_id = $('#arev_Id').val();
        var Enter_BY_SORT = $('#EnterBySort').val();
        var Lat_revno = $('#arev_no').val();
        $("#leftpanel").html('');
        $("#leftpanel").hide();
        $('#leftpanel_div').hide();
        $('#leftpanel_quickmenu').hide()
        startAnimation()
        $('#General').load('../Application/VR1GeneralDetails', { applicationrevid: revisionId, hideflag: true },
            function () {

                $("#leftpanel").html('');
                $('#leftpanel').load('../SORTApplication/SORTLeftPanel?Display=VR1Approval' + '&pageflag=' + Pageflag + '&PlannerId=' + PlannerId + '&Project_ID=' + projectid + '&Rev_ID=' + revision_id + '&Latest_Rev_No=' + Lat_revno + '&VR1App=' + VR1Applciation + '&Enter_BY_SORT=' + Enter_BY_SORT, function () {
                    $("#leftpanel").show();
                    ApplSummaryLeftPanelInit();
                });
                $('#General').show();
                $('#General').find('#revise').hide();
                $('#General').find('#notify').hide();
                $('#General').find('#clone').hide();
                $('#General').find('#tbl_foldermgmt').hide();
                stopAnimation()
                CheckSessionTimeOut();
            });

    } else {
        var revision_no = $('#arev_no').val();
        var revision_id = $('#arev_Id').val();
        var ver_no = $('#ver_no').val();
        var PlannerId = $('#PlannrUserId').val();
        $("#leftpanel").html('');
        $("#leftpanel").hide();
        $('#leftpanel_div').hide();
        $('#leftpanel_quickmenu').hide()
        $("#leftpanel").html('');

        startAnimation()
        $('#General').load('../Application/SOHaulierApplicationDetails', { hauliermnemonic: hauliermnemonic, esdalref: esdalref, revisionno: revision_no, versionno: ver_no, hideflag: true, RevisionID: revision_id },
            function () {
                $('#General').show();
                $("#leftpanel").html('');
                if ($('#ViewFlag').val() == 0) {
                    $('#leftpanel').load('../SORTApplication/SORTLeftPanel?Display=ApplSumm&pageflag=' + Pageflag + '&PlannerId=' + PlannerId + '&Project_ID=' + projectid + '&Rev_ID=' + revision_id + '&Enter_BY_SORT=' + sortentered, function () {
                        $("#leftpanel").show();
                        ApplSummaryLeftPanelInit();
                    });
                }
                stopAnimation()
                CheckSessionTimeOut();
            });
    }

}
//DisplayVehroute
function DisplayVehroute() {

    if (status == "MoveVer") {
        var analysis_id = $('#analysis_id').val();
        var versionId = $('#versionId').val();
        $('#tab_3').html('');
        startAnimation()
        $('#tab_3').load('../Application/RouteConfig', { versionId: versionId }, function () {
            $('#tab_3').show();
            $('#ShowDetail').hide();
            stopAnimation()
            CheckSessionTimeOut();
            RouteVehicleInit();
        });
        //$('#tab_3').load('../Application/HaulierApplRouteParts', { VersionId: versionId, SORTVehRoute: true },//, SORTVehRoute: true
        //    function () {
        //        $('#tab_3').show();
        //        stopAnimation()
        //        CheckSessionTimeOut();
        //        HaulierAppRoutePartInit();
        //    });
    }
    else {
        var revision_id = $('#arev_Id').val();
        var app_status = $('#Application_Status').val();
        $('#tab_3').html('');
        startAnimation();
        $('#tab_3').load('../Application/RouteConfig', { versionId: hf_versionId }, function () {
            $('#tab_3').show();
            $('#ShowDetail').hide();
            stopAnimation()
            CheckSessionTimeOut();
            RouteVehicleInit();
        });
        //$('#tab_3').load('../Application/HaulierApplRouteParts', { RevisionId: revision_id, IsSort: true, RouteVehFlag: true },//, SORTVehRoute: true
        //    function () {
        //        $('#tab_3').show();
        //        $('#ShowDetail').hide();
        //        stopAnimation()
        //        CheckSessionTimeOut();
        //        HaulierAppRoutePartInit();
        //    });
    }
}

function DisplayProjView() {
    WarningCancelBtn();
    CloseInfoPopup();
    CloseInfoPopup('InfoPopup');
    $('#SelectCurrentMovements1').hide();
    $('#SelectCurrentMovements2').hide();
    $('#route').hide();
    $('#back_btn_Rt').hide();
    $('#SelectCurrentMovementsVehicle1').html('');
    $('#SelectCurrentMovementsVehicle2').html('');
    $('#App_Candidate_Ver').html('');
    $('#StruRelatedMov_viewDetails').html('');

    var sort_status = $('#SortStatus').val();
    var apprevisionid = $('#ApprevId').val();
    if (sort_status == "CreateSO") {
        $.post('/SORTApplication/CheckVehicleForApplication?PROJ_ID=' + projectid + '&Rev_Id=' + apprevisionid, function (data) {
            if (data == true) {
                ViewProjectOverview();
            }
            else {
                stopAnimation();
                showWarningPopDialog('Submit the edited application before proceeding', 'Ok', '', 'showGeneral', '', 1, 'info');
            }
        });
    }
    else
        ViewProjectOverview();

}
function showGeneral() {
    WarningCancelBtn();
    ShowSOGeneralPage();
    $(".tab_content1").each(function () {
        $(this).hide();
    });
    $("li[id=2]").addClass('t');
    $('#pageheader').find('h3').text(title + ' - ' + $("li[id=2]").find('.tab_centre').text());
}
function ViewProjectOverview() {
    var VR1Applciation = $('#VR1Applciation').val();
    var Owner = $('#Owner').val();
    var Checker = $('#Checker').val();
    var revisionId = $('#revisionId').val();
    var apprevisionid = $('#ApprevId').val();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var prjstatus = $('#Proj_Status').val();
    $("#leftpanel").hide();
    $('#leftpanel_div').hide();
    $('#leftpanel_quickmenu').hide()
    Owner = Owner.replace(/ /g, '%20');
    Checker = Checker.replace(/ /g, '%20');

    var versionno = $('#versionno').val();

    $('#HaulOrg').load('../SORTApplication/SORTProjectOverview?hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&Rev_ID=' + apprevisionid + '&Checker=' + Checker + '&OwnerName=' + Owner + '&ProjectId=' + projectid + '&VR1App=' + VR1Applciation, {},
        function () {
            startAnimation();
            $('#SupplInfo').hide();
            if (VR1Applciation == "true") {
                $('#HaulOrg').show();
                $('#HaulOrg').find('#Candidate_route_version').hide();

                $('#HaulOrg').find('#Special_order').hide();

            } else {
                $('#HaulOrg').show();
            }
            $('.t').each(function () {
                $(this).hide();
            });
            $('.t').eq(0).show();
            if ($('#hauliermnemonic').val() != "") {
                $('#9').show();
            }
            var prjheader = "";
            var mainheader = "";
            if (VR1Applciation == "True" || VR1Applciation=="true")
                if (hauliermnemonic != "" && esdalref != 0) {
                    mainheader = hauliermnemonic + "/" + esdalref;
                    prjheader = " - VR1 - " + prjstatus;
                } else {
                    prjheader = "Apply for VR-1";
                }
            else
                if (hauliermnemonic != "" && esdalref != 0) {
                    mainheader = hauliermnemonic + "/" + esdalref;
                    prjheader = " - SO - " + prjstatus;
                } else {
                    prjheader = "Apply for SO";
                }
            $('.mainhead').html('');
            $('.mainhead').append(mainheader);
            $('.sorthead').html('');
            $('.sorthead').append(prjheader);
            $('.sortsubhead').html('');
            //stopAnimation();
            CheckSessionTimeOut();
            SORTProjectOverviewInit();
        });
}
function DisplayApplSummary() {
    var VR1Applciation = $('#VR1Applciation').val();
    var OrgID = $('#OrganisationId').val();
    var projectid = $('#projectid').val();
    var btn_show = false;
    var revisionId = $('#revisionId').val();
    if (MovLatestVer == versionno) {
        btn_show = true;
    }
    if (VR1Applciation == "True") {
        var status = $('#SortStatus').val();
        $("#leftpanel").hide();
        $('#leftpanel_div').hide();
        $('#leftpanel_quickmenu').hide()
        startAnimation()
        $('#General').load('../SORTApplication/SORTApplicationSummary', { Status: status, RevisionId: revisionId, versionId: versionId, Project_id: projectid, Mov_btn_show: btn_show, VR1App: VR1Applciation, Organisation_ID: OrgID },
            function () {
                $('#General').show();
                stopAnimation()
                SortAppSummaryInit();
            });
    }
    else {
        var status = $('#SortStatus').val();
        $("#leftpanel").hide();
        $('#leftpanel_div').hide();
        $('#leftpanel_quickmenu').hide();
        startAnimation();
        $('#General').load('../SORTApplication/SORTApplicationSummary', { Status: status, RevisionId: revisionId, versionId: versionId, Project_id: projectid, Mov_btn_show: btn_show, Organisation_ID: OrgID },
            function () {
                $('#General').show();
                stopAnimation()
                CheckSessionTimeOut();
                SortAppSummaryInit();
            });
    }
}
function ViewProjectOverviewtemp() {

    var VR1Applciation = $('#VR1Applciation').val();
    var Owner = $('#Owner').val();
    var Checker = $('#Checker').val();
    var revisionId = $('#revisionId').val();
    var apprevisionid = $('#ApprevId').val();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var prjstatus = $('#Proj_Status').val();
    $("#leftpanel").hide();
    $('#leftpanel_div').hide();
    $('#leftpanel_quickmenu').hide()
    Owner = Owner.replace(/ /g, '%20');
    Checker = Checker.replace(/ /g, '%20');

    startAnimation();
    $('#HaulOrg').load('../SORTApplication/SORTProjectOverview?hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&Rev_ID=' + apprevisionid + '&Checker=' + Checker + '&OwnerName=' + Owner + '&ProjectId=' + projectid + '&VR1App=' + VR1Applciation, {},
        function () {
            $('#SupplInfo').hide();
            if (VR1Applciation == "True") {
                $('#HaulOrg').show();
                $('#HaulOrg').find('#Candidate_route_version').hide();
                $('#HaulOrg').find('#Special_order').hide();
            } else {
                $('#HaulOrg').show();
            }
            $('.t').each(function () {
                $(this).hide();
            });
            $('.t').eq(0).show();
            if ($('#hauliermnemonic').val() != "") {
                $('#9').show();
            }
            var prjheader = "";
            if (VR1Applciation == "True")
                if (hauliermnemonic != "" && esdalref != "") {
                    prjheader = hauliermnemonic + "/" + esdalref + " - VR1 - " + prjstatus;
                } else {
                    prjheader = "Apply for VR-1";
                }
            else
                if (hauliermnemonic != "" && esdalref != "") {
                    prjheader = hauliermnemonic + "/" + esdalref + " - SO - " + prjstatus;
                } else {
                    prjheader = "Apply for SO";
                }
            $('div#pageheader').html('');
            $('div#pageheader').append('<h3>' + prjheader + '</h3>');
            stopAnimation();
            CheckSessionTimeOut();
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
          /*  $('div#pageheader').html('');*/
            $('#General').html('');
            $('#General').html(page);
            $('#leftpanel').hide();
            removeHLinksTrans();
            PaginateGridTrans();
            fillPageSizeSelect();
            CollaborationStatusInit();
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
function CreteInternalCollaborationInit() {
    firstName = $('#hf_First_Name').val();
    surName = $('#hf_Sur_Name').val();
    $('#ValidStatus').hide();
    $('#updateInternalNote').hide();
    SetStatus();
    $('.modal-content').css('background', 'transparent');
    $('.CustModel').css('background', 'white');
    $('.modal-content').css('border', 'aliceblue');
    $('.modal-backdrop').modal({
        backdrop: 'static',
        keyboard: false
    });
    $('#exampleModalCenter2').modal({
        backdrop: 'static',
        keyboard: false
    });
}
function CollaborationStatusInit() {
    fillPageSizeSelect();
    if (localStorage['page'] == document.Url)
        $(document).scrollTop(localStorage['scrollTop']);
    $("#General").css("display", "block");
    PageNumber = $('#hf_PageNumber').val();
    StatusCode = $('#hf_StatusCode').val();
    SORTCollab = $('#hf_SORTCollab').val();
    NotifCollab = $('#hf_NotifCollab').val();
    pageSizeTemp = $('#hf_pageSize').val();
    pageNum = PageNumber;
    randomNumber = Math.random();
    CreteInternalCollaborationInit();
}
function ViewSORTHistory(doc_id) {
    randomNumber = Math.random();
    startAnimation();
    $("#overlay").show();
    removescroll();
    $("#dialogue").load('../Notification/CollabHistoryPopList?DocumentId=' + doc_id + "&randomNumber=" + randomNumber + "&SORTCollab=" + true, function () {
        $('#CollabHistoryDiv').modal({ keyboard: false, backdrop: 'static' });

        $('#CollabHistoryDiv').modal('show')
        $("#dialogue").css("display", "block");

        stopAnimation();
        $("#overlay").css("display", "block");
    });
}
function SetSORTInternalCollaboration(doc_id, coll_id) {
    randomNumber = Math.random();
    $('#hdnDocumentID').val(doc_id);
    var paramList = {
        DOCUMENT_ID: doc_id,
        COLLABORATION_NO: coll_id
    }
    $.ajax({
        async: false,
        type: "POST",
        url: '../Notification/StoreInternalCollaboration',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: JSON.stringify(paramList),
        processdata: true,
        success: function (result) {
            if (result) {
                paramList = null;
                $("#overlay").show();
                removescroll();
                $("#dialogue").load('../Notification/CollabHistoryPopList?SORTSetCollab=' + true, function () {
                    $('#CollabHistoryDiv').modal({ keyboard: false, backdrop: 'static' });

                    $('#CollabHistoryDiv').modal('show')
                    $("#dialogue").css("display", "block");

                    stopAnimation();
                    $("#overlay").css("display", "block");
                });
            }
        },
    });
}
function DisplayTransStatus() {
    CloseSuccessModalPopup();
    var Version_Status = $('#versionStatus').val();
    var doc_status = 0;
    switch (Version_Status) {
        case "305002"://proposed
            doc_status = 1;
            break;
        case "305003"://reproposed
            doc_status = 2;
            break;
        case "305004"://agreed
            doc_status = 3;
            break;
        case "305005"://agreed revised
            doc_status = 4;
            break;
        case "305006"://agreed recleared
            doc_status = 5;
            break;
    }
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    $("#leftpanel").hide();
    $('#leftpanel_div').hide();
    $('#leftpanel_quickmenu').hide();
    $("#General").hide();
    var tmf = { All: true, Delivered: true, Failed: true, Pending: true, Sent: true }
    collab = false;
    if (esdal_ref_no != 0) {
        $.ajax({
            url: '../Notification/TransmissionStatusList',
            data: { page: 1, pageSize: 10, PageView: true, TMF: tmf, Notification_Code: esdal_ref_no, showtrans: true, SortStatus: doc_status },
            type: 'GET',
            cache: false,
            async: false,
            beforesend: function () {
                startAnimation()
            },
            success: function (page) {

                $('#tab_7').html(page);
                removeHLinksTrans();
                PaginateGridTrans();
                fillPageSizeSelect();
                CheckSessionTimeOut();
            },
            complete: function () {
                stopAnimation()
            }
        });
    } else {
        $('#tab_7').html('<span class="error" style="text-align:center;"><h4 style="color:#414193;margin: 0px 0;font-size: 1.0em;">There is no transmission status for this application. </h4> </span><br/>');
    }
}
function btn_ViewProposal() {
    //showWarningDialog('Functionality is not implemented for this version.', 'Ok', '', WarningCancelBtn, '', 1, 'info');AuthorizeMovementGeneral
}
function DisplayHistory(sortOrder, sortType, pageNum,page) {
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref_no = $('#esdalref').val();
    var esdal_hist = hauliermnemonic + '/' + esdalref;
    var versionno = $('#versionno').val();
    var projId = $('#projectid').val();
    startAnimation()
    $('#SupplInfo').load('../SORTApplication/SORTMovementHistory', { page: pageNum,pageSize:page, Haul_num: hauliermnemonic, esdalref: esdalref_no, versionno: versionno, projId: projId, sortOrder: sortOrder, sortType: sortType },
        function () {
            $('#SupplInfo').show();
            $('#HaulOrg').hide();
            stopAnimation();
            SORTMovementHistoryInit();
        });
}
function ShowCheckDiff() {
    $("#leftpanel").hide();
    $('#leftpanel_div').hide();
    $('#leftpanel_quickmenu').hide()
    startAnimation()
    $('#General').load('../SORTApplication/SORTCheckDifference', {},
        function () {
            $('#General').show();
            stopAnimation()
        });

}
function DisplaySpecOrder() {
    $("#leftpanel").hide();
    $('#leftpanel_div').hide();
    $('#leftpanel_quickmenu').hide()
    $.ajax({
        url: "../SORTApplication/SORTSpecialOrder",
        type: "GET",
        onbegin: function () {
            startAnimation();
        },
        success: function (page) {
            $('#General').html(page);
        },
        complete: function () {
            $('#General').show();
            stopAnimation();
        }
    });
}
function toHaulier() {
    $("#leftpanel").hide();
    $('#leftpanel_div').hide();
    $('#leftpanel_quickmenu').hide();
    $('#tab_8').html('');
    var version_Id = $("#versionId").val();
    startAnimation();
    $('#tab_8').load('../SORTApplication/HaulierNotes?VersionId=' + version_Id, function () {
        $('#tab_8').show();
        stopAnimation();
        HaulierNotesInit();
    });
}
function ShowRouteAssessment() {
    $('#leftpanel_div').hide();
    $('#leftpanel_quickmenu').hide()
    startAnimation()
    $("#leftpanel").html('');
    $('#leftpanel').load('../SORTApplication/SORTLeftPanel', { Display: "RouteAss" },
        function () {
            $("#leftpanel").show();
            stopAnimation();
            ApplSummaryLeftPanelInit();
        });

}
function ListRouteAssessment() {
    $("#General").html('');
    $("#General").hide();

    var _palnnerId = $('#PlannrUserId').val();
    var _appstatuscode = $('#AppStatusCode').val();
    var _checkerStatus = $('#CheckerStatus').val();
    var _isvr1 = $('#VR1Applciation').val();
    var analysis_id = $('#analysis_id').val();
    var revisionId = $('#revisionId').val();
    var status = $('#SortStatus').val();
    var VR1Applciation = $('#VR1Applciation').val();


    $('#leftpanel_div').load('../Application/RouteAnalysisPanel', { analysisId: analysis_id, RivisionId: revisionId, SORTflag: true, IsVr1: VR1Applciation, planneruserId: _palnnerId, appStatusCode: _appstatuscode, CheckerStatus: _checkerStatus, SORTStatus: status }, function () {
        $("#leftpanel_div").show();
        $('#leftpanel').show();
        $("#overlay").hide();
        $('.loading').hide();
        $('#Organisation_ID').val(0);
        RouteAnalysisPanelInit();
    });
}
function ViewVehicleSORT(id) {
    var VR1Applciation = $('#VR1Applciation').val();
    var flag;
    if (VR1Applciation == "True") {
        flag = "VR1";
    }
    else {
        flag = "SOApp";
    }
    $("#dialogue").html('');
    $("#dialogue").load('../VehicleConfig/PartialViewLayout', function () {
        DynamicTitle('View configuration');
        $.ajax({
            url: '../VehicleConfig/ViewConfiguration',
            type: 'GET',
            cache: false,
            async: false,
            data: { vehicleID: id, isRoute: true, movementId: movementId, flag: flag },
            beforeSend: function () {
                startAnimation();
                $("#overlay").show();
                $("#dialogue").hide();
                $('.loading').show();
            },
            success: function (result) {
                $('#Config-body').html(result);
                $('#Config-body').find('h4').hide();
            },
            error: function (xhr, textStatus, errorThrown) {
                location.reload();
            },
            complete: function () {
                stopAnimation();
                $("#dialogue").show();
                $("#overlay").show();
                $('.loading').hide();
                removescroll();
            }
        });
    });
}
//View candidate route vehicles.
function ViewCandidateVehicleSORT(id) {
    var flag = "VR1";
    $("#dialogue").html('');
    $("#dialogue").load('../VehicleConfig/PartialViewLayout', function () {
        DynamicTitle('View configuration');
        $.ajax({
            url: '../VehicleConfig/ViewConfiguration',
            type: 'GET',
            cache: false,
            async: false,
            data: { vehicleID: id, isRoute: true, movementId: movementId, flag: flag },
            beforeSend: function () {
                startAnimation();
                $("#overlay").show();
                $("#dialogue").hide();
                $('.loading').show();
            },
            success: function (result) {
                $('#Config-body').html(result);
                $('#Config-body').find('h4').hide();
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                stopAnimation();
                Resize_PopUp(930);
                $("#dialogue").show();
                $("#overlay").show();
                $('.loading').hide();
                removescroll();
            }
        });

    });
}
function ClosePopUp() {
    $('#overlay').hide();
    $('#dialogue').hide();
    addscroll();
}
//function remove href from pagination ul li
function removeHLinksTrans(divId) {
    if (collab == true) {
        $('#tab_6').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    } else {
        if ($('#tab_7').length > 0) {
            $('#tab_7').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
        }
        else {
            $('#trans_status_details').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
        }
    }

}
//function Pagination
function PaginateGridTrans() {
    //method to paginate through page numbers
    if (collab == true) {
        $('#tab_6').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
            //var pageCount = $('#TotalPages').val();
            var pageNum = $(this).html();
            // CheckSessionTimeOut();
            AjaxPaginationTrans(pageNum);
        });
    } else {
        var divId = '#tab_7';
        if ($('#tab_7').length == 0) {
            divId = '#trans_status_details';
        }
        $(divId).find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
            //var pageCount = $('#TotalPages').val();
            var pageNum = $(this).html();
            // CheckSessionTimeOut();
            AjaxPaginationTrans(pageNum);
        });
    }
    PaginateToLastPageTrans();
    PaginateToFirstPageTrans();
    PaginateToNextPageTrans();
    PaginateToPrevPageTrans();
}
//method to paginate to last page
function PaginateToLastPageTrans() {

    if (collab == true) {
        $('#tab_6').find('.PagedList-skipToLast').click(function () {
            var pageCount = $('#TotalPages').val();
            // CheckSessionTimeOut();
            AjaxPaginationTrans(pageCount);
        });
    } else {
        var divId = '#tab_7';
        if ($('#tab_7').length == 0) {
            divId = '#trans_status_details';
        }
        $(divId).find('.PagedList-skipToLast').click(function () {
            var pageCount = $('#TotalPages').val();
            // CheckSessionTimeOut(); 
            AjaxPaginationTrans(pageCount);
        });
    }
}
//method to paginate to first page
function PaginateToFirstPageTrans() {
    if (collab == true) {
        $('#tab_6').find('.PagedList-skipToFirst').click(function () {
            AjaxPaginationTrans(1);
            //  CheckSessionTimeOut();
        });
    } else {
        var divId = '#tab_7';
        if ($('#tab_7').length == 0) {
            divId = '#trans_status_details';
        }
        $(divId).find('.PagedList-skipToFirst').click(function () {
            AjaxPaginationTrans(1);
            // CheckSessionTimeOut();
        });
    }
}
//method to paginate to Next page
function PaginateToNextPageTrans() {

    if (collab == true) {
        $('#tab_6').find('.PagedList-skipToNext').click(function () {
            var thisPage = $('#tab_6').find('.active').find('a').html();
            var nextPage = parseInt(thisPage) + 1;
            //  CheckSessionTimeOut();
            AjaxPaginationTrans(nextPage);
        });
    } else {
        var divId = '#tab_7';
        if ($('#tab_7').length == 0) {
            divId = '#trans_status_details';
        }
        $(divId).find('.PagedList-skipToNext').click(function () {
            var thisPage = $(divId).find('.active').find('a').html();
            var nextPage = parseInt(thisPage) + 1;
            // CheckSessionTimeOut();
            AjaxPaginationTrans(nextPage);
        });
    }
}
//method to paginate to Previous page
function PaginateToPrevPageTrans() {
    if (collab == true) {
        $('#tab_6').find('.PagedList-skipToPrevious').click(function () {
            var thisPage = $('#tab_6').find('.active').find('a').html();
            var prevPage = parseInt(thisPage) - 1;
            //  CheckSessionTimeOut();
            AjaxPaginationTrans(prevPage);
        });
    } else {
        var divId = '#tab_7';
        if ($('#tab_7').length == 0) {
            divId = '#trans_status_details';
        }
        $(divId).find('.PagedList-skipToPrevious').click(function () {
            var thisPage = $(divId).find('.active').find('a').html();
            var prevPage = parseInt(thisPage) - 1;
            // CheckSessionTimeOut();
            AjaxPaginationTrans(prevPage);
        });
    }
}
//function Ajax call fro pagination
function AjaxPaginationTrans(pageNum) {
    var selectedVal = $('#pageSizeVal').val();
    var esdal_ref_no = $('#NotificationCode').val();
    if (esdal_ref_no == undefined || esdal_ref_no == '') {
        esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    }
    var pageSize = selectedVal;
    if (collab == true) {
        sURL = 'Notification/CollaborationStatusList';
        $.ajax({
            url: '../' + sURL,
            data: { page: pageNum, pageSize: pageSize, SORTCollab: true, RefNo: esdal_ref_no },
            type: 'GET',
            async: false,
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                $('#tab_6').html(page);
                $('#pageSizeVal').val(pageSize);
                $('#pageSizeSelect').val(pageSize);
                removeHLinksTrans();
                PaginateGridTrans();
                fillPageSizeSelect();
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    else {
        var SortApp_Status = $('#SortAppStatus').val() ? $('#SortAppStatus').val() : 0;
        var tmf = { All: true, Delivered: true, Failed: true, Pending: true, Sent: true }
        sURL = 'Notification/TransmissionStatusList';
        $.ajax({
            url: '../' + sURL,
            data: { page: pageNum, pageSize: pageSize, PageView: true, TMF: tmf, Notification_Code: esdal_ref_no, showtrans: true, SortStatus: SortApp_Status },
            type: 'GET',
            async: false,
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {

                var divId = '#tab_7';
                if ($('#tab_7').length == 0) {
                    divId = '#trans_status_details';
                }
                $(divId).html(page);
                $('#pageSizeVal').val(pageSize);
                $('#pageSizeSelect').val(pageSize);
                removeHLinksTrans();
                PaginateGridTrans();
                fillPageSizeSelect();
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
}
function fillPageSizeSelect() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
}
function Select_AllocatePOP() {
    startAnimation();
    $("#active-button-allocate").css("display", "block");
    $("#container").css("display", "block");
    $("#active-button-allocate").css("background", "#275795");
    $("#active-button-allocate").css("color", "white");
    $("#inactive-button-allocate").css("display", "none");
    $('#AllocateUser').load('../SORTApplication/SORTAllocateUser?projectId=' + projectid + '&revisionNo=' + revisionno, {},
    function () {
        $("#overlay").hide();
        SortAllocateUserInit();
        stopAnimation();
    });
}
function removeAllocateButton() {
    $("#container").css("display", "none");
    $("#allocate").css("display", "none");
    $("#active-button-allocate").css("display", "none");
    $("#inactive-button-allocate").css("display", "block");
}
function onEdit() {
    $('#Txt_Mov_Name').val($('#spn_mov_name').text().substring(0, 35));
    $('#Txt_HA_Job_Ref').val($('#spn_ha_ref').text().substring(0, 20));
    $('#Txt_start_add').val($('#spn_start_add').text().substring(0, 100));
    $('#Txt_end_add').val($('#spn_end_add').text().substring(0, 100));
    $('#Txt_Load_summ').val($('#spn_load_summ').text());
    $("#afterEdit").css("display", "none");
    $("#beforeEdit").css("display", "block");
}
function editcancel() {
    $("#afterEdit").css("display", "block");
    $("#beforeEdit").css("display", "none");
}
function SortDecline() {
    var projectid = $('#projectid').val();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var esdal_ref_no = hauliermnemonic + "/" + esdalref;
    var Msg = "Do you want to decline application \"" + esdal_ref_no + '\" ?';
    flag = 2;
    ShowDialogWarningPop(Msg, 'No', 'Yes', 'WarningCancelBtn', 'SORTApplication', 1, 'warning', esdalref, projectid, flag)
}
function SortWithdraw() {
    var projectid = $('#projectid').val();
    var Enter_BY_SORT = $('#EnterBySort').val();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var esdal_ref_no = hauliermnemonic + "/" + esdalref;
    var Msg = "Do you want to withdraw application \"" + esdal_ref_no + '\" ?';
    flag = 1;
    if (Enter_BY_SORT == 1) {
        ShowDialogWarningPop(Msg, 'No', 'Yes', 'WarningCancelBtn', 'SORTApplication', 1, 'warning', esdal_ref_no, projectid, flag);
    } else {
        showToastMessage({
            message: "Haulier created application cannot be withdrawn from SORT!",
            type: "error"
        })
    }
}
function SortUnWithdraw() {

    var Enter_BY_SORT = $('#EnterBySort').val();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var esdal_ref_no = hauliermnemonic + "/" + esdalref;
    var Msg = "Do you want to unwithdraw application \"" + esdal_ref_no + '\" ?';
    flag = 1;
    if (Enter_BY_SORT == 1) {
        ShowDialogWarningPop(Msg, 'No', 'Yes', 'WarningCancelBtn', 'SORTApplicationUnwithdrawn', 1, 'warning');
    } else {
        ShowDialogWarningPop(Msg, 'No', 'Yes', 'WarningCancelBtn', 'SORTApplicationUnwithdrawn', 1, 'warning');
        //ShowDialogWarningPop('Withdrawn haulier created application cannot be unwithdrawn from SORT!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
}
function SORTApplicationUnwithdrawn() {
    WarningCancelBtn();
    CloseWarningPopupDialog();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var esdalRef = hauliermnemonic + "/" + esdalref;
    var lastrevisionno = $('#revisionno').val();
    var LastversionNo = $('#hf_LatVer').val() != '' ? $('#hf_LatVer').val() : 0;
    $.ajax({
        url: '../SORTApplication/ApplicationUnWithdraw',
        type: 'POST',
        cache: false,
        async: true,
        data: { Project_ID: projectid, EsdalRef: esdalRef, lastrevisionno: lastrevisionno, LastversionNo: LastversionNo},
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (result.Success == '1') {
                ShowDialogWarningPop('"' + esdalRef + '" application is unwithdrawn.', 'Ok', '', 'Redirect_ProjectOverview', '', 1, 'info');

            } else {
                ShowDialogWarningPop('Application is not unwithdrawn.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
            }
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function SORTApplication() {
    WarningCancelBtn();
    CloseWarningPopupDialog();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var esdalRef = hauliermnemonic + "/" + esdalref;
    var lastrevisiono = $('#revisionno').val();
    var lastversionno = $('#hf_LatVer').val() != '' ? $('#hf_LatVer').val() : 0;
    //flag: 1= Withdraw; 2= Decline;

    $.ajax({
        url: '../SORTApplication/ApplicationWithAndDecline',
        type: 'POST',
        cache: false,
        async: true,
        data: { Project_ID: projectid, flag: flag, EsdalRef: esdalRef, LastRevisionno: lastrevisiono, lastversionno: lastversionno },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (result.Success == '1') {
                if (flag == 1) {
                    ShowDialogWarningPop('"' + esdalRef + '" application is withdrawn.', 'Ok', '', 'linktomovementinbox', '', 1, 'info');
                } else {
                    ShowDialogWarningPop('"' + esdalRef + '" application is declined.', 'Ok', '', 'linktomovementinbox', '', 1, 'info');
                }
            } else {
                if (flag == 1) {
                    ShowDialogWarningPop('Application is not withdrawn.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                } else {
                    ShowDialogWarningPop('Application is not declined.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                }
            }
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function linktomovementinbox() {
    WarningCancelBtn();
    window.location.href = '/SORTApplication/SORTInbox';
}
function SortAudit() {
    ShowDialogWarningPop('Functionality is not implemented for this version.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
}
function btnsp_click(e) {
    var versionId = $('#versionId').val();
    var orderid = $(e).attr("data-id");
    var pstatuscode = $('#AppStatusCode').val() ? $('#AppStatusCode').val() : 0;
    var projectstatus = $('#Proj_Status').val();
    projectstatus = projectstatus.replace(/ /g, '%20');
    var checkingstatus = $('#CheckerStatus').val();
    var plannruserid = $('#PlannrUserId').val();
    var checker_id = $('#CheckerId').val();
    window.location = "/SORTApplication/ShowSpecialOrder" + EncodedQueryString("SpecialOrderId=" + orderid + "&VersionId=" + versionId + "&projectId=" + projectid + "&hauliermnemonic=" + hauliermnemonic + "&esdalref=" + esdalref + "&versionno=" + versionno + '&ProjectStatus=' + projectstatus + '&CheckingStatus=' + checkingstatus + '&Plannerid=' + plannruserid + '&Checkerid=' + checker_id + '&Projstatus=' + pstatuscode);
}
function VR1approval() {
    FlagValue = 9;
    var _verno = $('#versionno').val();
    mov_verid = $('#versionId').val();
    $.post('/BrokenRoutesReplanner/ShowBrokenRoutes?App_revision_id=0&can_revision_id=0&mov_version_id=' + mov_verid + '&FlagValue=' + FlagValue, function (data) {
        if (data == true) {
            var msg = 'Please be aware that due to the map upgrade route(s) in VR1 movement version (' + _verno + ') contain previous map data and have been updated since the haulier applied for the movement.';
            showWarningPopDialog(msg, 'Ok', '', 'VR1approvalChck', '', 1, 'info');
        }
        else {
            VR1approvalChck();
        }
        // made  function to code  to make sysnchronisation
    });
}
function VR1approvalChck() {
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var esdalRefNum = hauliermnemonic + "/" + esdalref;
    var Msg = "Do you want to approve \"" + esdalRefNum + '\" VR1 application?';
    ShowDialogWarningPop(Msg, 'No', 'Yes', 'WarningCancelBtn', 'VR1ApprovalFunct', 1, 'warning');
}
function VR1ApprovalFunct() {
    var revision_no = 0;
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var esdalRefNum = hauliermnemonic + "/" + esdalref;
    var projectid = $('#projectid').val();
    var status = $('#SortStatus').val();
    var VR1Number = $('#Vr1Number').val() != '' ? $('#Vr1Number').val() : 0;
    var versionno = $('#hf_LatVer').val() != '' ? $('#hf_LatVer').val() : 0;
    var revisionno = $('#revisionno').val();
    if (status == "Revisions" || status == "ViewProj") {
        var revision_no = $('#arev_no').val();
    }
    var dataModelPassed = { ProjectId: projectid, Rev_No: revision_no, EsdalRef: esdalRefNum, VR1_No: VR1Number, VersionNo: versionno, RevisionNo: revisionno };
    var result = SORTVR1Routing(2, dataModelPassed);
    if (result != undefined || result != "NOROUTE") {
        $.ajax({
            url: result.route, //"../SORTApplication/VR1ApprovalSubmit",
            type: 'POST',
            cache: false,
            async: true,
            data: {
                approveVr1CntrlModel: result.dataJson
            },
            beforeSend: function () {
                $("#overlay").show();
                $('.loading').show();
                startAnimation();
            },
            success: function (result) {
                if (result != 0) {
                    ShowDialogWarningPop('VR1 application"' + esdalRefNum + '" is approved.', 'Ok', '', 'Redirect_ProjectOverview', '', 1, 'info');
                } else {
                    ShowDialogWarningPop('Movement version is not approved.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
                }
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
                $("#overlay").hide();
                $('.loading').hide();
            }
        });
        //$.ajax({
        //    url: '../SORTApplication/VR1ApprovalSubmit',
        //    type: 'POST',
        //    cache: false,
        //    async: false,
        //    data: { ProjectId: projectid, Rev_No: revision_no, EsdalRef: esdalRefNum, VR1_No: VR1Number },
        //    beforeSend: function () {
        //        $("#overlay").show();
        //        $('.loading').show();
        //        startAnimation();
        //    },
        //    success: function (result) {
        //        if (result != 0) {
        //            ShowDialogWarningPop('VR1 application"' + esdalRefNum + '" is approved.', 'Ok', '', 'Redirect_ProjectOverview', '', 1, 'info');
        //        } else {
        //            ShowDialogWarningPop('Movement version is not approved.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
        //        }
        //    },
        //    error: function (xhr, textStatus, errorThrown) {
        //    },
        //    complete: function () {
        //        stopAnimation();
        //        $("#overlay").hide();
        //        $('.loading').hide();
        //    }
        //});
    }
}
function GenerateVR1Number() {

    var projectid = $('#projectid').val();
    $.ajax({
        url: '../SORTApplication/GenerateVR1Number',
        type: 'POST',
        cache: false,
        async: false,
        data: { ProjectId: projectid },
        beforeSend: function () {
            $("#overlay").show();
            $('.loading').show();
            startAnimation();
        },
        success: function (result) {
            if (result != "") {
                if (result == -1) {
                    ShowInfoPopup('VR1 number is already generated for this application.', 'Redirect_ProjectOverview');
                } else {
                    $('#Vr1Number').val(result);
                    //GenerateVR1Document();
                    ShowInfoPopup('VR1 number "' + result + '" successfully generated.', 'Redirect_ProjectOverview');
                }
            } else {
                ShowDialogWarningPop('Vr1 number is not generated.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            stopAnimation();
        },
        complete: function () {
            stopAnimation();
            $("#overlay").hide();
            $('.loading').hide();
        }
    });
}
function GenerateVR1Document() {
    var projectid = $('#projectid').val();
    var MovLatestVer = $('#MovLatestVer').val();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var vr1number = $('#Vr1Number').val();
    //-------------------------------------
    if (hauliermnemonic != "" && esdalref != "" && MovLatestVer != 0) {
        window.location.href = '../SORTApplication/GenerateVR1Document' + EncodedQueryString('Haulier_numeric=' + hauliermnemonic + '&Esdal_Ref=' + esdalref + '&Mov_Latest_ver=' + MovLatestVer);
    } else {
        ShowDialogWarningPop('Vr1 document is not generated.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
    }
}
function Mov_Agree_funt() {
    var FlagValue = 5;
    var _verno = $('#versionno').val();
    var _verid = $('#versionId').val();
    $("#overlay").show();
    $('.loading').show();
    removescroll();
    startAnimation();
    $.post('/BrokenRoutesReplanner/ShowBrokenRoutes?App_revision_id=0&can_revision_id=0&mov_version_id=' + _verid + '&FlagValue=' + FlagValue, function (data) {
        if (data == true) {
            var msg = 'Please be aware that due to the map upgrade route(s) in the proposed/re-proposed/agreed revised movement version (' + _verno + ') contain previous map data. Please review before agreeing the movement version.';
            showWarningPopDialog(msg, 'Ok', '', 'Mov_Agree_funt_chck', '', 1, 'info');

        }
        else {
            Mov_Agree_funt_chck();
        }
    });
    $("#overlay").hide();
    $('.loading').hide();
    removescroll();
    stopAnimation();

}
function Mov_Agree_funt_chck() {
    var esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + $('#hf_LatVer').val();
    var Msg = "Do you want to agree this movement version \"" + esdal_ref_no + '\" ?';
    flag = 1;
    ShowDialogWarningPop(Msg, 'NO', 'YES', 'WarningCancelBtn', 'MovementVersionFunct', 1, 'warning');

}
function Mov_Unagree_funt() {
    var esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + $('#hf_LatVer').val();
    var Msg = "Do you want to unagree this movement version \"" + esdal_ref_no + '\" ?';
    flag = 2;
    ShowWarningPopup(Msg, 'MovementVersionFunct', 'WarningCancelBtn');
}
function Mov_Withdraw_funt() {
    var esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + $('#hf_LatVer').val();
    var Msg = "Do you want to withdraw this movement version \"" + esdal_ref_no + '\" ?';
    flag = 3;
    ShowDialogWarningPop(Msg, 'No', 'Yes', 'WarningCancelBtn', 'MovementVersionFunct', 1, 'warning');
}
function MovementVersionFunct() {
    var esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + $('#hf_LatVer').val();
    var appref = hauliermnemonic + "/" + esdalref + "-" + revisionno;
    var esdalReferenceWorkflow = hauliermnemonic + "_" + esdalref;
    analysis_id = $('#candAnalysisId').val();
    var vr1Application = false;
    if ($('#vr1appln').val() === "True") {
        vr1Application = true;
    }
    var latestVersionId = $('#hf_LatestVersionId').val();
    var dataModelPassed = { Version_Id: latestVersionId, flag: flag, esdalRef: esdal_ref_no, ProjectId: projectid, AppRef: appref, AnalysisId: analysis_id, VersionNo: $('#hf_LatVer').val(), EsdalReferenceWorkflow: esdalReferenceWorkflow, isVr1Application: vr1Application, RevisionNo: revisionno, isWorkflow: true };
    var result = SORTSORouting(7, dataModelPassed);
    if (result != undefined || result != "NOROUTE") {
        $.ajax({
            url: result.route, //"../SORTApplication/MovementAgreeUnagreeWithdraw",
            type: 'POST',
            async: true,
            data: {
                movementAgreeUnagreeWithdrawCntrlModel: result.dataJson
            },
            beforeSend: function () {
                WarningCancelBtn();
                startAnimation();

            },
            success: function (result) {
                stopAnimation();
                if (result == 1) {
                    if (flag == 1) {
                        var latestVersion = $('#hf_LatVer').val();
                        latestVersion++;
                        esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + latestVersion;
                        $('#versionno').val(latestVersion);
                        ShowSuccessModalPopup('"' + esdal_ref_no + '" version is agreed.', 'Redirect_ProjectOverview');
                    } else if (flag == 2) {
                        ShowInfoPopup('"' + esdal_ref_no + '" version is unagreed.', 'Redirect_ProjectOverview');
                    } else if (flag == 3) {
                        ShowInfoPopup('"' + esdal_ref_no + '" version is withdrawn.', 'Redirect_ProjectOverview');
                    }
                } else {
                    if (flag == 1) {
                        ShowInfoPopup('Movement version is not agreed, Please generate route analysis from last candidate route version');
                    } else if (flag == 2) {
                        ShowInfoPopup('Movement version is not unagreed');
                    } else if (flag == 3) {
                        ShowInfoPopup('Movement version is not withdrawn');
                    }
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                stopAnimation();
                ShowInfoPopup('Process is not completed, Please contact helpdesk');
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
}
//function to show distribute movement pop up
function DistributeMovement() {

    startAnimation();
    WarningCancelBtn();
    var _verid = $('#versionId').val();
    var Version_Status = $('#versionStatus').val();
    var _verno = $('#versionno').val();
    var status = $('#SortStatus').val();
    var esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + _verno;
    var mov_analysisid = $('#analysis_id').val();
    var cand_analysisid = $('#candAnalysisId').val();
    if (status == "MoveVer") {
        cand_analysisid = $('#analysis_id').val();
    }
    $.post('/SORTApplication/CheckRouteAsseBeforeDistributing?AnalysisId=' + cand_analysisid, function (data) {
        switch (data) {
            case 1:
                $("#overlay").show();
                $('.loading').show();
                var options = { "backdrop": "static", keyboard: true };
                $.ajax({
                    type: "GET",
                    url: "../SORTApplication/DistributionCommentsPopUp",
                    //contentType: "application/json; charset=utf-8",
                    data: { EsdalRefNumber: esdal_ref_no, VersionId: _verid },
                    datatype: "json",
                    success: function (data) {
                        $('#generalPopupContent').html(data);
                        $('#generalPopup').modal(options);
                        $('#generalPopup').modal('show');
                        $('.modal-content').css({
                            'width': '32rem',
                            'height': '18rem'
                        });
                        $('.loading').hide();
                        stopAnimation();
                    },
                    error: function () {
                        location.reload();
                    }
                });
                break;
            case 2:
                ShowInfoPopup('Please generate driving instructions before distribute movement from last candidate route version!');
                break;
            case 3:
                ShowInfoPopup('Please generate affected structures before distribute movement from last candidate route version!');
                break;
            case 7:
                ShowInfoPopup('Please generate route description before distribute movement from last candidate route version!');
                break;
            case 8:
                ShowInfoPopup('Please generate affected parties before distribute movement from last candidate route version!');
                break;
            case 9:
                ShowInfoPopup('Please generate affected roads before distribute movement from last candidate route version!');
                break;
        }
    });
}
/* broken route check function for Distribute movments  
   305001-work in progress  should be blocked
   305004-agreed,305005-agreed revised,305006-agreed recleared should be warned
*/
function VerifyBrokenRouteDistribute() {
    var _verno = $('#versionno').val();
    var _verid = $('#versionId').val();
    var Version_Status = $('#versionStatus').val();
    var FlagValue = 5;
    if (Version_Status == 305001 || Version_Status == 305011) {
        FlagValue = 2;
    }
    $.ajax({
        url: '../BrokenRoutesReplanner/ShowBrokenRoutes',
        type: 'POST',
        cache: false,
        async: false,
        data: { App_revision_id: 0, can_revision_id: 0, mov_version_id: _verid, FlagValue: FlagValue },
        beforeSend: function () {
            if (FlagValue == 2) {
                startAnimation();
                $("#overlay").show();
                $("#dialogue").show();
                $('.loading').show();
            }
        },
        success: function (data) {
            if (data == "") {
                DistributeMovement();
            }
            else if (FlagValue == 2) {
                $('#dialogue').html(data);
            }
            else {
                var msg = 'Please be aware that due to the map upgrade route(s) in the movement version (' + _verno + ') contain previous map data. Please review before distributing agreed/agreed re-cleared movement version (' + _verno + ').';
                showWarningPopDialog(msg, 'Ok', '', 'DistributeMovement', '', 1, 'info');
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            if (FlagValue == 2) {
                removescroll();
                $('.loading').hide();
            }
        }
    });
}
//Create amendment order
function CreateAmendementOrder() {
    var Latest_SpeOrder = $('#LatestSpeOrder').val();
    var doctype = 'WORD';
    if (doctype != "") {
        window.location.href = '../SORTApplication/GenrateAmendDocuments' + EncodedQueryString('SOnumber=' + Latest_SpeOrder + '&doctype=' + doctype);
    } else {
        ShowInfoPopup('Document type is not there for this special order');
    }
}
//Redirect to project overiew
function Redirect_ProjectOverview() {
    WarningCancelBtn();
    CloseInfoPopup('InfoPopup');
    var sprojectid = $('#projectid').val();
    var sapprevId = $('#ApprevId').val();
    var sowner = $('#Owner').val();
    var vr1app = $('#VR1Applciation').val();
    var Enter_BY_SORT = $('#EnterBySort').val();
    sowner = sowner.replace(/ /g, '%20');
    var _orgId = $('#OrganisationId').val();

    var _movversionid = $('#versionId').val();
    var _movversionno = $('#versionno').val();
    var cstatus = $('#Work_Status').val();
    var analysis_id = $('#candAnalysisId').val();
    var revisionno = $('#revisionno').val();

    window.location = "/SORTApplication/SORTListMovemnets" + EncodedQueryString("SORTStatus=ViewProj&revisionId=" + revisionId + "&versionId=" + _movversionid + "&hauliermnemonic=" + hauliermnemonic + "&esdalref=" + esdalref + "&revisionno=" + revisionno + "&versionno=" + _movversionno + "&OrganisationId=" + _orgId + "&projecid=" + sprojectid + "&movementId=" + movementId + "&Owner=" + sowner + "&WorkStatus=" + cstatus + "&apprevid=" + sapprevId + "&pageflag=2" + "&VR1Applciation=" + vr1app + "&EnterBySORT=" + Enter_BY_SORT + "&analysisId=" + analysis_id);
}
function Acknowledgement_Click() {
    var _movversionno = $('#hf_LatVer').val() != '' ? $('#hf_LatVer').val() : 0;
    var _apprevno = $('#revisionno').val();
    var _esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + _movversionno;
    var enteredbysort = $('#EnteredBySort').val();

    $.post('/SORTApplication/Acknowledgement?ProjectId=' + projectid + '&EsdalReference=' + _esdal_ref_no + '&MovVersionNo=' + _movversionno + '&AppRevisionNo=' + _apprevno + '&RevisedBySort=' + enteredbysort + '&IsVr1=' + VR1Applciation, function (data) {
        if (data == 1) {
            ShowDialogWarningPop('Application is revised.', 'Ok', '', 'Redirect_ProjectOverview', '', 1, 'info');
        }
        else {
            ShowDialogWarningPop('Process is not completed.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
        }
    });
}
function CreateNewSpecialOrder() {
    CloseWarningPopup();
    var _plannerId = $('#PlannrUserId').val();
    var _project_status = $('#Proj_Status').val();

    window.location = "/SORTApplication/CreateSpecialOrder" + EncodedQueryString("sedalno=" + hauliermnemonic + "/" + esdalref + "/S" + versionno + "&ProjectId=" + projectid + "&VersionId=" + versionId + '&PlannerId=' + _plannerId + '&ProjectStatus=' + _project_status);
}
function UpdateOldSpecialOrder() {
    CloseWarningPopup();
    var IsVR1Applciation = $('#VR1Applciation').val();
    var CandRevisionID = $('#LastCandRevisionId').val();
    var analysis_id = $('#candAnalysisId').val();
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var esdal_ref_val = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    var typename = "QAChecking";
    var _sonumber = $('#SONumber').val();
    $('#sonum').val(_sonumber);
    var mov_analysis_id = $('#analysis_id').val();
    var atype = "QAChecking";
    $.ajax({
        url: '../SORTApplication/UpdateSpecialOrder',
        type: 'POST',
        data: { VersionId: versionId, ProjectId: projectid, esdal_ref: esdal_ref_val, sonumber: _sonumber },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (data == 1) {
                var options = { "backdrop": "static", keyboard: true };
                $.ajax({
                    type: "GET",
                    url: "../SORTApplication/ViewCandidateCheckerUsers",
                    //contentType: "application/json; charset=utf-8",
                    data: { AllocationType: atype },
                    datatype: "json",
                    success: function (result) {
                        $('#generalPopupContent').html(result);
                        $('#generalPopup').modal(options);
                        $('#generalPopup').modal('show');
                        $('.loading').hide();
                        stopAnimation();
                    },
                    error: function () {
                        location.reload();
                    }
                });
            }
            else {
                location.reload();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            location.reload();
        }
    });
}
function showSOWarningPopDialogue(message, btn1_txt, btn2_txt, btn1Action, btn2Action, autofocus, type) {
    $(".POP-dialogue111").hide();
    $(".POP-dialogue1").show();
    ResetPopUp();

    if (btn1_txt == 'Ok') {
        btn1_txt = 'OK';
    }

    if (btn2_txt == 'Ok') {
        btn2_txt = 'OK';
    }

    $('.pop-message').html(message);
    if (btn1_txt == '') { $('.box_warningBtn1').hide(); } else { $('.box_warningBtn1').html(btn1_txt); }
    if (btn2_txt == '') { $('.box_warningBtn2').hide(); } else { $('.box_warningBtn2').html(btn2_txt); }

    //if (autofocus == 1) $('.box_warningBtn1').focus(); else $('.box_warningBtn2').focus();
    if (btn1Action != '') {
        $("body").on('click', '.box_warningBtn1', function () {
            window[btn1Action]();
        });
    }
    if (btn2Action != '') {
        $("body").on('click', '.box_warningBtn2', function () {
            window[btn2Action]();
        });
    }

    switch (autofocus) {
        //case 1: $('.box_warningBtn1').focus(); break;
        //case 2: $('.box_warningBtn2').focus(); break;
        case 1: $('.box_warningBtn1').attr("autofocus", 'autofocus');
            $('.popup1 span').css({ "display": "block", "font-size": "11px" });
            $('.message1 div ').css({ "width": "260px" }); break;
        case 2: $('.box_warningBtn2').attr("autofocus", 'autofocus'); break;
        default: break;
    }

    switch (type) {
        case 'error': $('.message1').addClass("errror"); $('.popup1').css({ "background": '#fcd1d1' }); break;
        case 'info': $('.message1').addClass("info"); $('.popup1').css({ "background": '#cdecfe' }); break;
        case 'warning': $('.message1').addClass("warning"); $('.popup1').css({ "background": '#ffffd0' }); break;
        default: break;

    }
    $('#pop-warning').show();

}
function WarningCancelSOBtn() {
    $('.pop-message').html('');
    $('.box_warningBtn1').html('');
    $('.box_warningBtn2').html('');
    $('#pop-warning').hide();

    $('.box_warningBtn1').unbind();
    $('.box_warningBtn2').unbind();

    $('.popup1 span').css({ "display": "none", "font-size": "9px" });
    $('.message1 div ').css({ "width": "285px" });

    EnableBackButton();
    addscroll();
}
$('body').on('change', '#SORTHistory #pageSizeSelect', function () {
    var pageSize = $(this).val();
    var pagesize=  $('#pageSizeVal').val(pageSize);
    DisplayHistory(null,null,null,pageSize);
   
});
$('body').on('change', '.sort-history-page-change', function () {
    var pageSize = $(this).val();
    DisplayHistory(null, null, null, pageSize);

});
/*function GenerateRouteAssessment(versionId, esdalref, revisionId, notificationId, analysisId, prevAnalysisId, VSOType) {
    $.ajax({
        url: '../RouteAssessment/GenerateRouteAssessment',
        type: 'POST',
        cache: false,
        async: true,
        data: { versionId: versionId, esdalref: esdalref, revisionId: revisionId, notificationId: notificationId, analysisId: analysisId, prevAnalysisId: prevAnalysisId, vSoType: VSOType },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            stopAnimation();
        },
        error: function (xhr, textStatus, errorThrown) {
            stopAnimation();
        },
        complete: function () {
            stopAnimation();
        }
    });
}*/