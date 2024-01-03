
var ProjectID, hauliermnemonic, esdalref, _pstatus, Checker = "", _checkerid, _checkerstatus;
function StruApplRevisions_SelectCurrMovmt(V_IsVehicleCurrentMovenet) {
    var  OwnerName = '',  checkingstatus, _isvr1, Enter_BY_SORT;
    OwnerName = '';
    plannruserid = $('#PlannrUserId').val();
    checkingstatus = $('#CheckerStatus').val();
    _isvr1 = $('#VR1Applciation').val();
    Enter_BY_SORT = $('#EnterBySort').val();
    var N_IsRouteCurrentMovenet = true;
    if (V_IsVehicleCurrentMovenet == 'True')
        N_IsRouteCurrentMovenet = false;
    startAnimation()
    Checker = Checker.replace(/ /g, '%20');
   
    $('#SelectCurrentMovements1').load('../SORTApplication/SORTApplRevisions?ProjectID=' + ProjectID + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&Checker=' + Checker + '&PlannerId=' + plannruserid + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&ProjectStatus=' + _pstatus + '&IsChooseCurrMovmOption=' + N_IsRouteCurrentMovenet + '&IsVehicleCurrentMovenet=' + V_IsVehicleCurrentMovenet, {},
        function () {
            $('#SelectCurrentMovementsVehicle1').show();
            $('#SelectCurrentMovements1').show();
            stopAnimation()
        });
    
    
}
function StruMovementVersion__SelectCurrMovmt(V_IsVehicleCurrentMovenet) {
    var _isvr1 = false;
    var OwnerName = '';
    var plannruserid = $('#PlannrUserId').val();
    var checkingstatus = $('#CheckerStatus').val();
    var Enter_BY_SORT = $('#EnterBySort').val();

  
    var N_IsRouteCurrentMovenet = true;
    if (V_IsVehicleCurrentMovenet == 'True')
        N_IsRouteCurrentMovenet = false;
    startAnimation()

   
    Checker = Checker.replace(/ /g, '%20');
    
    $('#SelectCurrentMovements2').load('../SORTApplication/SORTAppMovementVersion?ProjectID=' + ProjectID + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&EnterBySORT=' + Enter_BY_SORT + '&IsVR1App=' + _isvr1 + '&Checker=' + Checker + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&IsChooseCurrMovmOption=' + N_IsRouteCurrentMovenet + '&IsVehicleCurrentMovenet=' + V_IsVehicleCurrentMovenet, {},
            function () {
                $('#SelectCurrentMovementsVehicle2').show();
                $('#SelectCurrentMovements2').show();
                stopAnimation()
            });
    
   
}
function StruRelatedMov_viewDetails(VAnalysisId, VPrj_Status, Vhauliermnemonic, Vesdalref, Vprojectid) {
   
    $('#StruRelatedMove').val("Yes");
    ProjectID = Vprojectid;
    hauliermnemonic = Vhauliermnemonic;
    _pstatus = VPrj_Status;
    esdalref = Vesdalref;
    $("#StruRelatedMov").hide();
    $("#btnBackToStruRelatedMov").show();
    $("#header-fixed").hide(); 
    $("#tableheader").hide();
    
    $("#PrevMove_projectid").val(Vprojectid);
    $("#PrevMove_hauliermnemonic").val(Vhauliermnemonic);
    $("#PrevMove_esdalref").val(Vesdalref);
    $("#IsPrevMoveOpion").val(true);

    $('#RoutePart').hide();
    $('#SelectCurrentMovementsVehicle').show();
    $('#divCurrentMovement').html('');
    WarningCancelBtn();
    startAnimation();
    var wstatus = 'ggg';
    $('#StruRelatedMov_viewDetails').load('../SORTApplication/SORTAppCandidateRouteVersion?AnalysisId=' + VAnalysisId + '&Prj_Status=' + VPrj_Status + '&hauliermnemonic=' + Vhauliermnemonic + '&esdalref=' + Vesdalref + '&projectid=' + Vprojectid + '&IsCandPermision=' + 'true' + '&IsCurrentMovenet=true', {},
        function () {

            //ClosePopUp();
            $("#dialogue").html('');
            $("#dialogue").hide();
            $("#overlay").hide();
            addscroll();
            stopAnimation();
        });
}
function BackToStruRelatedMov() {

    $("#StruRelatedMov").show();
    $("#btnBackToStruRelatedMov").hide();

    $("#StruRelatedMov_viewDetails").html('');
    $("#SelectCurrentMovements2").html('');
    $("#SelectCurrentMovements1").html('');
}
//function CurreMovemenRouteList(V_ReviosionId, VRList_type) {
//    if ($("#IsCreateApplicationRoute").val() == "true") {
//        showPreviousMovement(V_ReviosionId, VRList_type);
//    }
//    else {
//        showRelatedMovements(V_ReviosionId, VRList_type);
//    }
//}
function showPreviousMovement(V_ReviosionId, VRList_type) {
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
    var VIsIsCreateApplication = false;
    var VIsRelStruMov = false;
    if ($("#IsMyStructure").val() == "Yes")
        VIsRelStruMov = true;
    $('#tem_ReviosionId').val(V_ReviosionId);
    $('#temRList_type').val(VRList_type);
    removescroll();
    $("#overlay").show();
    $('.loading').show();
    if ($("#IsCreateApplicationRoute").val() == "true")
        VIsIsCreateApplication = true;
    $.ajax({
        url: "../SORTApplication/CandiVersionRoutesList",
        type: 'post',
        async: false,
        data: { routerevision_id: rtrevisionId, CheckerId: _checkerid, CheckerStatus: _checkerstatus, IsCandLastVersion: iscandlastversion, planneruserId: plannruserid, appStatusCode: appstatuscode, SONumber: sonumber, RList_type: VRList_type, IsIsCreateApplication: VIsIsCreateApplication, IsRelaStruMov: VIsRelStruMov },
        success: function (data) {
            $('#dialogue').html(data);
            $("#overlay").show();
            $("#dialogue").show();
            $('.loading').hide();
        },
        error: function () {
        }
    });
}
function showRelatedMovements(V_ReviosionId, VRList_type) {
    var app_id = V_ReviosionId;
    var type = VRList_type;
    var link = '';
    $.ajax({
        url: "../SORTApplication/RelatedMovements",
        type: 'post',
        async: false,
        data: { app_Id: app_id, type: VRList_type },
        success: function (data) {
            var project_id = data.result.ProjectID;
            var hauliermnemonic = data.result.Hauli_Mneu;
            var esdalrefnum = data.result.Reference_no;
            var revision_id = data.result.ApplicationrevId;
            var version_id = data.result.VersionID;
            var version_no = data.result.VersionNo;
            var revision_no = data.result.RevisionNo;
            var max_version_no = data.result.LastVersionNo;
            var enter_by_sort = data.result.Enteredbysort;
            var organisation_id = data.result.OrgID;
            var owner_name = data.result.OwnerName;
            var esdal_reference = data.result.esdal_reference;
            var cand_analysis_id = data.result.CandAnalysisId;
            var cand_revision_id = data.result.CandRevisionId;
            var cand_rev_no = data.result.CandRevisionNo;
            var last_can_rev_no = data.result.LastCandRevisionNo;
            var cand_name = data.result.CandRtName;
            var cand_rt_id = data.result.CandRouteId;
            var flag = false;
            if (last_can_rev_no == cand_rev_no)
                flag = true;
            if (VRList_type == 'M')
                link = '../SORTApplication/SORTListMovemnets?SORTStatus=MoveVer&projecid=' + project_id + '&VR1Applciation=' + false + '&reduceddetailed=' + false + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalrefnum + '&revisionId=' + revision_id + '&movementId=' + 270006 + '&apprevid=' + revision_id + '&revisionno=' + revision_no + '&OrganisationId=' + organisation_id + '&versionno=' + version_no + '&versionId=' + version_id + '&VR1Applciation=' + false + '&reduceddetailed=' + false + '&pageflag=' + 2 + '&esdal_history=' + esdal_reference + '&LatestVer=' + max_version_no + '&WorkStatus=' + 'undefined' + '&EnterBySORT=' + enter_by_sort + '&Checker=' + '' + '&Owner=' + owner_name + '&ViewFlag=' + 1;
            else if (VRList_type == 'C')
                link = '../SORTApplication/SORTListMovemnets?SORTStatus=CandidateRT&projecid=' + project_id + '&VR1Applciation=' + false + '&reduceddetailed=' + false + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalrefnum + '&revisionId=' + cand_revision_id + '&movementId=' + 27006 + '&apprevid=' + revision_id + '&revisionno=' + revision_no + '&OrganisationId=' + organisation_id + '&versionno=' + version_no + '&versionId=' + version_id + '&VR1Applciation=' + false + '&reduceddetailed=' + false + '&pageflag=' + 2 + '&esdal_history=' + esdal_reference + '&candName=' + cand_name + '&candVersionno=' + cand_rev_no + '&CandRouteId=' + cand_rt_id + '&LatestRevisionId=' + '' + '&analysisId=' + cand_analysis_id + '&IsLastVersion=' + flag + '&EnterBySORT=' + enter_by_sort + '&Owner=' + owner_name + '&ViewFlag=' + 1;
            else if (VRList_type == 'A')
                link = '../SORTApplication/SORTListMovemnets?SORTStatus=Revisions&projecid=' + project_id + "&OrganisationId=" + organisation_id + '&VR1Applciation=' + false + '&reduceddetailed=' + false + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalrefnum + '&revisionId=' + revision_id + '&versionId=' + version_id + '&movementId=' + 270006 + '&apprevid=' + revision_id + '&revisionno=' + revision_no + '&versionno=' + version_no + '&pageflag=' + 2 + '&arev_no=' + revision_no + '&arev_Id=' + revision_id + '&ver_no=' + 0 + '&WorkStatus=' + 'undefined' + '&Checker=' + '' + '&EnterBySORT=' + enter_by_sort + '&Owner=' + owner_name + '&ViewFlag=' + 1;;
        },
        complete: function (data) {
            window.open(link, '_blank');
        },
        error: function () {
        }
    });
}
function ClosePopUp() {
    $("#dialogue").html('');
    $("#dialogue").hide();
    $("#overlay").hide();
    addscroll();
    stopAnimation();
}
function ResetDataMovent() {
   
    $('#GrossWeight').val('');
    $('#OverallWidth').val('');
    $('#OverallLength').val('');
    $('#RigidLength').val('');
    $('#Height').val('');
    $('#AxleWeight').val('');
    $.ajax({
        url: '../Movements/ClearSOAdvancedFilter',
        type: 'POST',
        success: function (data) {
        }
    });
}

$('body').on('click', '.strurelatedmov', function (e) {
    var VAnalysisId = $(this).data("vanalysisid");
    var VPrj_Status = $(this).data("vprj_status");
    var Vhauliermnemonic = $(this).data("vhauliermnemonic");
    var Vesdalref = $(this).data("vesdalref");
    var Vprojectid = $(this).data("vprojectid");
    StruRelatedMov_viewDetails(VAnalysisId, VPrj_Status, Vhauliermnemonic, Vesdalref, Vprojectid);
    return false
});
