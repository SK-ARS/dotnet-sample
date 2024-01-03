var incrListStructure = 0;
var incrListConstraint = 0;
var incrListCaution = 0;
var oldIdDiv = "";
var iscandidatert;
var SortStatus;
var AnalysisId;
var revisionId;
var versionid;
var isVR1Application = "";
var contentRefNo;
var struckCheck;
var constraintCheck;
var cautionCheck;
var analysisId;
var hf_contentRefNo;
var chk_status;
var sort_user_id;
var checker_id;
var SORTflag;
var routeAnalysisPanelInitFlag = false;
var routeAssessmentStep = 0;

function RouteAnalysisNotifInit() {
    routeAssessmentStep = 0;
    routeAnalysisPanelInitFlag = true;
    $("#IncrListStructure").val(incrListStructure);
    $("#IncrListConstraint").val(incrListConstraint);
    $("#IncrListCaution").val(incrListCaution);
    oldId = 'RBAnnot';
    iscandidatert = $('#IsCandidateRT').val();
    SortStatus = $('#SortStatus').val();
    AnalysisId = "";
    revisionId = "";
    versionid = 0;
    contentRefNo = $('#CRNo').val() != undefined ? $('#CRNo').val() : "";
    struckCheck = $('#hf_StructCheck').val();
    constraintCheck = $('#hf_ConstraintCheck').val();
    cautionCheck = $('#hf_CautionCheck').val();
    analysisId = $('#hf_AnalysisId').val();
    hf_contentRefNo = $('#hf_ContentRefNo').val();
    chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    SORTflag = $('#SORTflag').val();
    if ($('#vr1appln').val() != null) { //Haulier side portal calls

        isVR1Application = $('#vr1appln').val();
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            //ListSOMovements.cshtml page realted hidden variable to store version id
            if ($('#VersionId').val() != null) {
                versionid = $('#VersionId').val();
            }
        }
        else { // special order
            revisionId = 0;
            versionid = $('#VersionId').val();
        }
    }
    if ($('#VR1Applciation').val() != null) { // SORT Side portal calls

        isVR1Application = $('#VR1Applciation').val();

        if (isVR1Application == 'True' || isVR1Application == 'true') {
            //SORTListMovemnets.cshtml page realted hidden variable to store version id
            if ($('#versionId').val() != null) {
                versionid = $('#versionId').val();
            }
        }
    }

    
    //ListRouteDescription('RBRouteDescrp', 'routeDescription');
    if (struckCheck != 0) {
        if ($('#route_assessment_next_btn').is(":visible")) {
            $('.sidebar-soa-Helper .clickable:not(.btnAffectedStructures)').css({ "cursor": "not-allowed", "color": "#adaaaa" });
            $('.sidebar-soa-Helper .clickable:not(.btnAffectedStructures)').attr("title", "Click Next button to view");
        }
        ListAffectedStructures();
    } else if (constraintCheck != 0) {
        if ($('#route_assessment_next_btn').is(":visible")) {
            $('.sidebar-soa-Helper .clickable:not(.btnConstraints)').css({ "cursor": "not-allowed", "color": "#adaaaa" });
            $('.sidebar-soa-Helper .clickable:not(.btnConstraints)').attr("title", "Click Next button to view");
        }
        ListConstraints();
    } else if (struckCheck == 0 && constraintCheck == 0) {
        $('.sidebar-soa-Helper .clickable').css({ "cursor": "pointer", "color": "" });
        $('.sidebar-soa-Helper .clickable').attr("title", "");
        ListRouteDescription();
    }
    
    //$("#confirm_btn").prop("disabled", "true");
    if (struckCheck != 0 || constraintCheck != 0 ) {
        var message = "";
        $("#divRedirectUsers").show();
        incrListStructure = 1;
    }

    if (struckCheck != 0) {
        $("#needAttentionStruct").show();
        incrListStructure = 1;
    }
    else {
        incrListStructure = 0;
    }
    $("#IncrListStructure").val(incrListStructure);

    if (constraintCheck != 0) {
        $("#needAttentionConstraint").show();
        IncrListConstraint = 1;
    }
    else {
        IncrListConstraint = 0;
    }
    $("#IncrListConstraint").val(IncrListConstraint);

    if (cautionCheck != 0) {
        $("#needAttentionCaution").show();
        IncrListCaution = 1;
    }
    else {
        IncrListCaution = 0;
    }
    $("#IncrListCaution").val(IncrListCaution);


    if ($('#hf_Helpdest_redirect').val() == "true") {
        $("#SOGenerateID").css("display", "none");

    }
    
    if ($('#hf_IsDIGenerate').val() == 'True') {
        if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id)) {
            $('#btnmovdigenration').hide();
        }
        else {
            $('#btnmovdigenration').show();
        }
    }

    if ($('#hf_IsPlanMovmentGlobal').length > 0) {
        if (typeof showImminentMovementBanner != 'undefined') {
            showImminentMovementBanner('', '#divRouteAnalysis');
        }
    }
}
function closeActiveText(oldId) {
    $('#' + oldId).removeClass('active1');
}

$(document).ready(function () {
    $('body').on('click', '.BackToRouteAssessment', function (e) {
        e.preventDefault();
        BackToRouteAssessment(this);
    });

    $('body').on('click', '.btnRouteDescription', function (e) {
        e.preventDefault();
        if (!$('#route_assessment_next_btn').is(":visible")) {
            routeAnalysisPanelInitFlag = false;
            routeAssessmentStep = 2;
            var id = $(this).attr("rbroutedescrp");
            var divId = $(this).attr("routedescription");
            ListRouteDescription(id, divId);
        }
    });
    $('body').on('click', '.btnAffectedStructures', function (e) {
        e.preventDefault();
        var structuresCount = $('#hf_TotalStructureCount').val() || 0;
        var isPlanMovementFlag = $('#hf_IsPlanMovmentGlobal').length > 0;
        if (isPlanMovementFlag && structuresCount == 0 && $('#route_assessment_next_btn').is(":visible")) {
            //If no unsuitable structures found, the button will be disabled untill user clicks Next button in constraint page
            return false;
        }

        //LastStep
        if ($('#route_assessment_next_btn').is(":visible")) {
            if (LastStep == 1) {
                $('.sidebar-soa-Helper .clickable').css({ "cursor": "pointer", "color": "" });
                $('.sidebar-soa-Helper .clickable').attr("title", "");
                $('.sidebar-soa-Helper .clickable:not(.btnAffectedStructures):not(.btnConstraints)').css({ "cursor": "not-allowed", "color": "#adaaaa" });
                $('.sidebar-soa-Helper .clickable:not(.btnAffectedStructures):not(.btnConstraints)').attr("title","Click Next button to view");
            } else {
                $('.sidebar-soa-Helper .clickable').css({ "cursor": "pointer", "color": "" });
                $('.sidebar-soa-Helper .clickable').attr("title", "");
                $('.sidebar-soa-Helper .clickable:not(.btnAffectedStructures)').css({ "cursor": "not-allowed", "color": "#adaaaa" });
                $('.sidebar-soa-Helper .clickable:not(.btnAffectedStructures)').attr("title","Click Next button to view");
            }
        }
        //routeAnalysisPanelInitFlag = false;
        routeAssessmentStep = 0;
        var id = $(this).attr("rbaffectstruct");
        var divId = $(this).attr("affectedstructure");
        ListAffectedStructures(id, divId);
    });
    $('body').on('click', '.btnConstraints', function (e) {
        e.preventDefault();
        if (!$('#route_assessment_next_btn').is(":visible") || LastStep == 1) {
            routeAnalysisPanelInitFlag = false;
            routeAssessmentStep = 1;
            var id = $(this).attr("rbconstraints");
            var divId = $(this).attr("spnconstraints");
            ListConstraints(id, divId);
        }
    });
    $('body').on('click', '.btnCautions', function (e) {
        e.preventDefault();
        var id = $(this).attr("rbcaution");
        var divId = $(this).attr("spncautions");
        ListCautions(id, divId);
    });
    $('body').on('click', '.btnAffecteParties', function (e) {
        e.preventDefault();
        if (!$('#route_assessment_next_btn').is(":visible")) {
            routeAnalysisPanelInitFlag = false;
            routeAssessmentStep = 3;
            var id = $(this).attr("rbaffectedparty");
            var divId = $(this).attr("affectedparties");
            ListAffectedParty(id, divId);
        }
    });
    $('body').on('click', '.btnAffectedRoads', function (e) {
        e.preventDefault();
        if (!$('#route_assessment_next_btn').is(":visible")) {
            routeAnalysisPanelInitFlag = false;
            ListAffectedRoads(this);
        }
    });
    $('body').on('click', '.btnAnnotation', function (e) {
        e.preventDefault();
        if (!$('#route_assessment_next_btn').is(":visible")) {
            routeAnalysisPanelInitFlag = false;
            ListAnnotation(this);
        }
    });
    $('body').on('click', '.btnDrivingInstructions', function (e) {
        e.preventDefault();
        if (!$('#route_assessment_next_btn').is(":visible")) {
            routeAnalysisPanelInitFlag = false;
            ListDrivingInstructions(this);
        }
    });
    $('body').on('click', '#PrintReport', function (e) {
        e.preventDefault();
        PrintReport(this)
    });
    //Route Assessment - Next button
    var LastStep = 0;
    $('body').on('click', '#route_assessment_next_btn', function (e) {
        e.preventDefault();
        switch (routeAssessmentStep) {
            case 0:
                var routeRadioButtons = $('.route_select:visible').length;
                var isLastChecked = true;
                if (routeRadioButtons > 1) {
                    isLastChecked = false;
                }
                if ($('input[name="route_select"]:last').is(':checked')) {
                    isLastChecked = true;
                }

                if (hf_affectstarray.length == 0 || (hf_affectstarray.length == 1 && hf_affectstarray[0] == "") || (typeof isRouteAssessmentRequiredForWIP != 'undefined' && isRouteAssessmentRequiredForWIP == true && isLastChecked)) {
                    $('.sidebar-soa-Helper .clickable').css({ "cursor": "pointer", "color": "" });
                    $('.sidebar-soa-Helper .clickable').attr("title", "");

                    $('.sidebar-soa-Helper .clickable:not(.btnAffectedStructures):not(.btnConstraints)').css({ "cursor": "not-allowed", "color": "#adaaaa" });
                    $('.sidebar-soa-Helper .clickable:not(.btnAffectedStructures):not(.btnConstraints)').attr("title", "Click Next button to view");
                    ListConstraints();
                    LastStep = 1;
                }
                else {
                    if (!isLastChecked) {
                        $("input[name=route_select]:checked").parent('.pb-2').next('.pb-2').find('input[name=route_select]').click();
                        $('body').animate({
                            scrollTop: eval($('#AffectedStructureXslt').offset().top)
                        }, 1000);
                    }
                    //ShowErrorPopup("View all route parts before progressing.");
                }
                break;
            case 1:
                var proceedflag = 0;

                var routeRadioButtons = $('.route_select:visible').length;
                var isLastChecked = true;
                if (routeRadioButtons > 1) {
                    isLastChecked = false;
                }
                if ($('input[name="route_select"]:last').is(':checked')) {
                    isLastChecked = true;
                }

                if ($('#hf_TotalConstraintsCount').val() != undefined && $('#hf_TotalConstraintsCount').val() != 0) {
                    if (hf_afconstarray.length == 0 || (hf_afconstarray.length == 1 && hf_afconstarray[0] == "") ) {
                        proceedflag = 1;
                    }
                }
                if (proceedflag == 1 || (typeof isRouteAssessmentRequiredForWIP != 'undefined' && isRouteAssessmentRequiredForWIP == true && isLastChecked)) {
                    $('.sidebar-soa-Helper .clickable').css({ "cursor": "pointer", "color": "" });
                    $('.sidebar-soa-Helper .clickable').attr("title", "");
                    ListRouteDescription();//button hide logic added here 
                    LastStep = 2;
                }
                else {
                    if (!isLastChecked) {
                        $("input[name=route_select]:checked").parent('.pb-2').next('.pb-2').find('input[name=route_select]').click();
                        $('body').animate({
                            scrollTop: eval($('#div_ListConstraints').offset().top)
                        }, 1000);
                    }
                    //ShowErrorPopup("View all route parts before progressing.");
                }
                break;
            case 2:
                ListAffectedParty();               
                break;
        }
    });
});

function CheckConstraintList() {
    var analysisId = $('#hf_AnalysisId').val() || 0;
    var constraintID = 0;
    $.ajax({
        url: '../RouteAssessment/GetAffectedConstraintInfoList',
        type: 'POST',
        cache: false,
        async: false,
        data: { routeId: routeId, routeType: routeParams.routeType, BSortFlag: sortFlag, analysisId: analysisId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            //alert(data.result.check);
            constraintID = data.result.check;
            var randomNumber = Math.random();

            $("#dialogue").load('../Constraint/ViewConstraint?ConstraintID=' + constraintID + '&flageditmode=false' + '&random=' + randomNumber, function () {
                ViewConstraintInit();
            });
            //removescroll();
            stopAnimation();
            $("#dialogue").show();
            $("#overlay").show();

        },
        error: function (xhr, textStatus, errorThrown) {

        },
        complete: function () {

            //stopAnimation();
        }
    });
}
function GenerateInstr() {
    startAnimation();
    var _iscandidatert = $('#IsCandidateRT').val();
    var chkAnal_type = null;
    var MoveType = $('#MoveId').val();

    AnalysisId = $('#AnalysisId').val();
    revisionId = $('#RevID').val();
    $('#SearchNotifStructure').hide();
    var GenerateNewInstr = true;
    if (iscandidatert == 'True' || iscandidatert == 'true') {

        revisionId = $('#revisionId').val();
        AnalysisId = $('#candAnalysisId').val();
    }

    if (true) {
        // if ($('#RBDrivInstr').is(':checked')) {
        Flag = "DrivInstr";
        //msg="Driving instruction "
        chkAnal_type = 1;
    }
    else if ($('#RBAffectStruct').is(':checked')) {
        Flag = "AffectStruct";
        //msg = "Affected structure "
        chkAnal_type = 3;
    }
    else if ($('#RBAnnot').is(':checked')) {
        Flag = "Annot";
        // msg = "Annotation "
        chkAnal_type = 6;
    }
    else if ($('#RBCaution').is(':checked')) {
        Flag = "Caution";
        //msg = "Caution "
        chkAnal_type = 4;
    }
    else if ($('#RBConstr').is(':checked')) {
        Flag = "Constr";
        //msg = "Constraint "
        chkAnal_type = 5;
    }
    else if ($('#RBRouteDescrp').is(':checked')) {
        Flag = "RouteDescrp";
        //msg = "Route discription "
        chkAnal_type = 2;

    }
    else {
        chkAnal_type = 0;

    }
    if (chkAnal_type == 0) {

    }
    else {
        if ($('#SORTflag').val() != "true") {
            if (isVR1Application == 'True' || isVR1Application == 'true') {
                revisionId = 0;
                versionid = $('#VersionId').val();
            }
            else { // special order
                revisionId = 0;
                versionid = $('#VersionId').val();
            }
        }
        else {
            if (isVR1Application == 'True' || isVR1Application == 'true') {
                revisionId = 0;
                AnalysisId = $('#analysis_id').val();
                versionid = $('#versionId').val();
            }
        }
        if (SortStatus == "MoveVer") {
            AnalysisId = $('#analysis_id').val();
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        if (chkAnal_type == 1) {

            $.ajax
                ({
                    type: "POST",
                    url: "../RouteAssessment/GenerateAllDataFromService",
                    data: { analysisID: analysisId, anal_type: chkAnal_type, revisionId: revisionId, versionId: versionid, ContentRefNo: contentRefNo, GenerateNewInstr: GenerateNewInstr, IsCandidate: _iscandidatert, IsVR1: isVR1Application },
                    beforeSend: function () {
                        startAnimation();
                    },
                    success: function (data) {

                        if (data.result.errorcode == "Generated successfully ! Code : ERR#DIS005") {

                            if ($('#SORTflag').val() != "true") {

                                $('#drivingInstructions').load('../RouteAssessment/ListDrivingInstructions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&GenerateNewInstr=' + GenerateNewInstr + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
                                    {},
                                    function () {
                                        stopAnimation();
                                        $('#SearchStructure').hide();
                                        $('#div_StructureGeneralDetails').hide();
                                    });
                            } else {

                                $('#drivingInstructions').load('../RouteAssessment/ListDrivingInstructions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + $('#SORTflag').val() + '&GenerateNewInstr=' + GenerateNewInstr + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
                                    {},
                                    function () {
                                        stopAnimation();
                                        $('#SearchStructure').hide();
                                        $('#div_StructureGeneralDetails').hide();
                                    });
                            }
                            /* showWarningPopDialog('Driving instruction generated successfully.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');*/

                        }
                        else {
                            /*showWarningPopDialog(data.result.errorcode, 'Ok', '', 'WarningCancelBtn', '', 1, 'warning');*/
                        }
                    },
                    error: function (xhr, status, error) {
                        alert("error");
                    },
                    complete: function () {
                        stopAnimation();
                        $('#SearchStructure').hide();
                        $('#div_StructureGeneralDetails').hide();
                    }
                });

        }
        else if (chkAnal_type == 2) {
            $.ajax
                ({
                    type: "POST",
                    url: "../RouteAssessment/GenerateAllDataFromService",
                    data: { analysisID: analysisId, anal_type: 1, revisionId: revisionId, ContentRefNo: contentRefNo, GenerateNewInstr: GenerateNewInstr, IsCandidate: _iscandidatert, IsVR1: isVR1Application },
                    beforeSend: function () {
                        startAnimation();
                    },
                    success: function (data) {

                        if (data.result.errorcode == "Generated successfully ! Code : ERR#DIS005") {
                            startAnimation();
                            if ($('#SORTflag').val() != "true") {

                                $('#divRouteAssement').load('../RouteAssessment/ListRouteDescription?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid,
                                    {},
                                    function () {
                                        stopAnimation();
                                        $('#SearchStructure').hide();
                                        $('#div_StructureGeneralDetails').hide();
                                        showWarningPopDialog('Route description generated successfully', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                                    });
                            } else {

                                $('#RouteAssesment').load('../RouteAssessment/ListRouteDescription?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid,
                                    {},
                                    function () {
                                        stopAnimation();
                                        $('#SearchStructure').hide();
                                        $('#div_StructureGeneralDetails').hide();
                                        showWarningPopDialog('Route description generated successfully', 'Ok', '', 'WarningCancelBtn', '', 1, 'warning');
                                    });
                            }
                        }
                        else {
                            showWarningPopDialog(data.result.errorcode, 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                        }
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
        else if (chkAnal_type == 3) {
            startAnimation();

            if ($('#SORTflag').val() != "true") {
                $('#divRouteAssement').load('../RouteAssessment/ListAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
                    {},
                    function () {
                        stopAnimation();
                        $('#SearchStructure').show();
                        $('#div_StructureGeneralDetails').hide();
                    });
            } else {
                $('#RouteAssesment').load('../RouteAssessment/ListAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
                    {},
                    function () {
                        stopAnimation();
                        $('#SearchStructure').show();
                        $('#div_StructureGeneralDetails').hide();
                    });
            }
        }
        else if (chkAnal_type == 4) {
            startAnimation();
            if ($('#SORTflag').val() != "true") {
                $('#divRouteAssement').load('../RouteAssessment/ListCautions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
                    {},
                    function () {
                        stopAnimation();
                        $('#SearchStructure').hide();
                        $('#div_StructureGeneralDetails').hide();
                    });
                //stopAnimation();
                //$('#SearchStructure').hide();
            } else {
                $('#RouteAssesment').load('../RouteAssessment/ListCautions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
                    {},
                    function () {
                        stopAnimation();
                        $('#SearchStructure').hide();
                        $('#div_StructureGeneralDetails').hide();
                    });
            }
        }
        else if (chkAnal_type == 5) {
            startAnimation();
            if ($('#SORTflag').val() != "true") {
                $('#divRouteAssement').load('../RouteAssessment/ListConstraints?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
                    {},
                    function () {
                        stopAnimation();
                        $('#SearchStructure').hide();
                        $('#div_StructureGeneralDetails').hide();
                    });
                //stopAnimation();
                //$('#SearchStructure').hide();
            } else {
                $('#RouteAssesment').load('../RouteAssessment/ListConstraints?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
                    {},
                    function () {
                        stopAnimation();
                        $('#SearchStructure').hide();
                        $('#div_StructureGeneralDetails').hide();
                    });
            }
        }
        else if (chkAnal_type == 6) {
            startAnimation();
            if ($('#SORTflag').val() != "true") {
                $('#divRouteAssement').load('../RouteAssessment/ListAnnotations?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
                    {},
                    function () {
                        stopAnimation();
                        $('#SearchStructure').hide();
                        $('#div_StructureGeneralDetails').hide();
                    });

            } else {
                $('#RouteAssesment').load('../RouteAssessment/ListAnnotations?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
                    {},
                    function () {
                        stopAnimation();
                        $('#SearchStructure').hide();
                        $('#div_StructureGeneralDetails').hide();
                        $('input[type="button"]').hide();
                        $('.sortbuttons').show();
                    });
            }
        }
    }

}
function HideSpan() {
    $('#drivingInstructions').hide();
    $('#affectedstructure').hide();
    $('#annotations').hide();
    $('#spncautions').hide();
    $('#spnconstraints').hide();
    $('#affectedParties').hide();
    $('#affectedRoads').hide();
    $('#routeDescription').hide();
    $('#showAffectedparties').hide();
    $('#routeAssessmentBckbtn').hide();
}
function ListAffectedStructures(id, divId) {
    routeAssessmentStep = 0;
    id = id || 'RBAffectStruct';
    divId = divId || 'affectedstructure';
    $("#disclaimerBlock").hide();
    startAnimation();
    HideSpan();
    AnalysisId = $('#AnalysisId').val();
    revisionId = $('#RevID').val();
    var Org_ID = $('#Organisation_ID').val() ? $('#Organisation_ID').val() : 0;

    if (iscandidatert == 'True' || iscandidatert == 'true') {
        revisionId = $('#revisionId').val();
        AnalysisId = $('#candAnalysisId').val();
        //AnalysisId = $('#analysis_id').val();
    }

    if (SortStatus == "MoveVer") {
        AnalysisId = $('#analysis_id').val();
        revisionId = 0;
        versionid = $('#versionId').val();
    }
    anal_Type = 3
    $('#printbutton').hide();
    //$('#generatebutton').show();
   
    if ($('#SORTflag').val() != "true") {

        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        else { // special order
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        // $('#affectedstructure').load('../RouteAssessment/ListAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
        $('#affectedstructure').load('../RouteAssessment/GetAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&IsPlanMovement=true',
            {},
            function () {
                HandleAffectedStructureAutoNavigationByCount(id, divId);
            });
    } else {

        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            AnalysisId = $('#analysis_id').val();
            versionid = $('#versionId').val();
        }

        if (Org_ID != 0) {
            //$('#affectedstructure').load('../RouteAssessment/ListAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&FilterByOrgID=' + Org_ID,
            $('#affectedstructure').load('../RouteAssessment/GetAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&FilterByOrgID=' + Org_ID + '&IsPlanMovement=true',
                {},
                function () {
                    HandleAffectedStructureAutoNavigationByCount(id, divId);
                });
            $('#routeAssessmentBckbtn').show();
        }
        else {
            //$('#affectedstructure').load('../RouteAssessment/ListAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
            $('#affectedstructure').load('../RouteAssessment/GetAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&IsPlanMovement=true',
                {},
                function () {
                    HandleAffectedStructureAutoNavigationByCount(id, divId);
                }
            );

        }
    }
    animateToTopOnRouteAssessmentNavigationRouteAnalNotif();
}

//Check Unsuitable structure count, if count=0, navigate to constraints
function HandleAffectedStructureAutoNavigationByCount(id, divId) {
    var structuresCount = $('#hf_TotalStructureCount').val() || 0;
    var isPlanMovementFlag = $('#hf_IsPlanMovmentGlobal').length > 0;
    if ($('#route_assessment_next_btn').is(":visible") && parseInt(structuresCount) <= 0 && isPlanMovementFlag) {
        //navigate to constraints
        $('.sidebar-soa-Helper .clickable').css({ "cursor": "pointer", "color": "" });
        $('.sidebar-soa-Helper .clickable').attr("title", "");

        $('.sidebar-soa-Helper .clickable:not(.btnConstraints)').css({ "cursor": "not-allowed", "color": "#adaaaa" });
        $('.sidebar-soa-Helper .clickable:not(.btnConstraints)').attr("title", "Click Next button to view");
        ListConstraints();
        LastStep = 1;
    } else {
        stopAnimation();
        $('#viewEdit_Route').show();
        $('#SearchStructure').show();
        $('#div_StructureGeneralDetails').hide();
        routeAnalysisPanelInitFlag = false; //reset it false to avoid automated navigation in further clicks
        ListAffectedStructuresInit();
        openDetails(id, divId);
        $('#affectedstructure').show();
        $('#showAffectedparties').hide();
    }
}

function ListRouteDescription(id, divId) {
    routeAssessmentStep = 2;
    $('#back_btn').show();
    $('#confirm_btn').show();
    $('#route_assessment_next_btn').hide();

    if (typeof isRouteAssessmentRequiredForWIP != 'undefined' && isRouteAssessmentRequiredForWIP == true) {
        isRouteAssessmentRequiredForWIP = false;
        localStorage.removeItem("WorkInProgressNotifId")
    }

    id = id || 'RBRouteDescrp';
    divId = divId || 'routeDescription';
    $("#disclaimerBlock").show();
    startAnimation();
    HideSpan();
    $('#printbutton').hide();
    revisionId = $('#RevID').val();
    AnalysisId = $('#AnalysisId').val();
    anal_Type = 2;

    if (iscandidatert == 'True' || iscandidatert == 'true') {
        revisionId = $('#revisionId').val();
        AnalysisId = $('#candAnalysisId').val();
    }

    if (SortStatus == "MoveVer") {
        AnalysisId = $('#analysis_id').val();
        revisionId = 0;
        versionid = $('#versionId').val();
    }

    if (SORTflag != "true") {
        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        else { // special order
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        $.ajax({
            url: "../RouteAssessment/ListRouteDescription",
            type: 'GET',
            async: false,
            cache: false,
            contentType: 'application/json; charset=utf-8',
            data: { analysisID: AnalysisId, revisionId: revisionId, IsCandidate: iscandidatert, ContentRefNo: contentRefNo, versionId: versionid, IsVR1: isVR1Application },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                $('#routeDescription').html('');
                $('#routeDescription').html(data);
                $('#routeDescription').show();

            },
            complete: function () {
                stopAnimation();
            },
            error: function () {
                stopAnimation();
            }
        });
    }
    else {

        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            AnalysisId = $('#analysis_id').val();
            versionid = $('#versionId').val();
        }

        $('#routeDescription').load('../RouteAssessment/ListRouteDescription?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
            {},
            function () {
                stopAnimation();
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
            });
    }
    $('#routeDescription').show();
    openDetails(id, divId);
    animateToTopOnRouteAssessmentNavigationRouteAnalNotif();
}
function ListAffectedParty(id, divId) {
    routeAssessmentStep = 3;    
    id = id || 'RBAffectedParty';
    divId = divId || 'affectedParties';
    $("#disclaimerBlock").hide();
    var x = $('#VSOType').val();
    var y = $('#hf_VSOType').val();
    startAnimation();
    HideSpan();
    $('#printbutton').hide();
    revisionId = $('#RevID').val();
    AnalysisId = $('#AnalysisId').val();
    anal_Type = 7;
    var Distributed_mov_analysisid = 0;
    if (iscandidatert == 'True' || iscandidatert == 'true') {
        revisionId = $('#revisionId').val();
        AnalysisId = $('#candAnalysisId').val();
        Distributed_mov_analysisid = $('#DistributedMovAnalysisId').val();
    }

    if (SortStatus == "MoveVer") {
        AnalysisId = $('#analysis_id').val();
        revisionId = 0;
        versionid = $('#versionId').val();
    }

    var randomNum = Math.random();

    if ($('#SORTflag').val() != "true") {
        //stopAnimation();
        //ShowModalPopup('This functionality is not implemented');
        //showWarningPopDialog('This functionality is not implemented', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');

        $('#affectedParties').load('../Notification/GetAffectedParties?AnalysisId=' + AnalysisId + '&NotificationID=' + $('#NotificationId').val() + '&ContentRefNo=' + $('#CRNo').val() + '&VSOType=' + $('#hf_VSOType').val(), function () {
            WarningCancelBtn();
            $('#SearchStructure').hide();
            $('#div_StructureGeneralDetails').hide();
            stopAnimation();
            ListAffectedPartiesInit();
            ManualAddedPartiesInit();
        });

    } else {
        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            AnalysisId = $('#analysis_id').val();
            versionid = $('#versionId').val();
        }
        //Notification id is not passed ! passed as 0 this is from sort side affected parties page needs to be called
        //$('#affectedParties').load('../SORTApplication/AffectedParties?analysisId=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&random=' + randomNum + '&DistributedMovAnalysisId=' + Distributed_mov_analysisid, function () {
        $('#affectedParties').load('../SORTApplication/GetAffectedParties?analysisId=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&random=' + randomNum + '&DistributedMovAnalysisId=' + Distributed_mov_analysisid, function () {
            WarningCancelBtn();
            stopAnimation();
            $('#SearchStructure').hide();
            $('#div_StructureGeneralDetails').hide();
            ListAffectedPartiesInit();
            ManualAddedPartiesInit();
        });
    }
    openDetails(id, divId);
    $('#affectedParties').show();
    animateToTopOnRouteAssessmentNavigationRouteAnalNotif();
}
function ListCautions(id, divId) {
    $("#disclaimerBlock").hide();
    startAnimation();
    HideSpan();
    $('#printbutton').hide();
    revisionId = $('#RevID').val();
    AnalysisId = $('#AnalysisId').val();
    anal_Type = 4;

    if (iscandidatert == 'True' || iscandidatert == 'true') {
        revisionId = $('#revisionId').val();
        AnalysisId = $('#candAnalysisId').val();
        //AnalysisId = $('#analysis_id').val();
    }

    if (SortStatus == "MoveVer") {
        AnalysisId = $('#analysis_id').val();
        revisionId = 0;
        versionid = $('#versionId').val();
    }

    if ($('#SORTflag').val() != "true") {
        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        else { // special order
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        //$('#spncautions').load('../RouteAssessment/ListCautions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
        $('#spncautions').load('../RouteAssessment/GetAffectedCautions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
            {},
            function () {
                stopAnimation();
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
                selectedmenu('Movements'); // for selected menu
                $('#div_Cautions').find('.route_select').eq(0).attr({ "checked": true });
            });

    } else {
        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            AnalysisId = $('#analysis_id').val();
            versionid = $('#versionId').val();
        }

        //$('#spncautions').load('../RouteAssessment/ListCautions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + $('#SORTflag').val() + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
        $('#spncautions').load('../RouteAssessment/GetAffectedCautions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + $('#SORTflag').val() + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
            {},
            function () {
                stopAnimation();
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
                selectedmenu('Movements'); // for selected menu
                $('#div_Cautions').find('.route_select').eq(0).attr({ "checked": true });
            });
    }
    $('#spncautions').show();
    openDetails(id, divId);
    animateToTopOnRouteAssessmentNavigationRouteAnalNotif();
}
function ListConstraints(id, divId) {
    var selectedRadio = $('#div_ListConstraints input[name=route_select]:checked').val();
    routeAssessmentStep = 1;
    id = id || 'RBConstraints';
    divId = divId || 'spnconstraints';
    var UnSuitableShowAllConstraint = $('#div_ListConstraints .chk_UnSuitableShowAllConstraint').prop('checked') == true ? true : false;//true-all constraints , false - show only unsuitable
    var UnSuitableShowAllCautions = $('#div_ListConstraints .chk_UnSuitableShowAllCautions').prop('checked') == true ? true : false;//true-all cautions , false - show only unsuitable
    $("#disclaimerBlock").hide();
    startAnimation();
    HideSpan();
    $('#printbutton').hide();
    revisionId = $('#RevID').val();
    AnalysisId = $('#AnalysisId').val();
    anal_Type = 5;

    if (iscandidatert == 'True' || iscandidatert == 'true') {
        revisionId = $('#revisionId').val();
        AnalysisId = $('#candAnalysisId').val();
    }

    if (SortStatus == "MoveVer") {
        AnalysisId = $('#analysis_id').val();
        revisionId = 0;
        versionid = $('#versionId').val();
    }

    if ($('#SORTflag').val() != "true") {
        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        else { // special order
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        //$('#spnconstraints').load('../RouteAssessment/ListConstraints?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
        $('#spnconstraints').load('../RouteAssessment/GetAffectedConstraints?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&UnSuitableShowAllConstraint=' + UnSuitableShowAllConstraint + '&UnSuitableShowAllCautions=' + UnSuitableShowAllCautions + '&IsPlanMovement=true',
            {},
            function () {
                HandleConstraintsAutoNavigationByCount(id, divId, selectedRadio);
            });

    } else {
        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            AnalysisId = $('#analysis_id').val();
            versionid = $('#versionId').val();
        }

        //$('#spnconstraints').load('../RouteAssessment/ListConstraints?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + $('#SORTflag').val() + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
        $('#spnconstraints').load('../RouteAssessment/GetAffectedConstraints?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + $('#SORTflag').val() + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&UnSuitableShowAllConstraint=' + UnSuitableShowAllConstraint + '&UnSuitableShowAllCautions=' + UnSuitableShowAllCautions + '&IsPlanMovement=true',
            {},
            function () {
                HandleConstraintsAutoNavigationByCount(id, divId, selectedRadio);
            });
    }
    animateToTopOnRouteAssessmentNavigationRouteAnalNotif();
}

function animateToTopOnRouteAssessmentNavigationRouteAnalNotif() {
    $('body').animate({
        scrollTop: eval($('body').offset().top)
    }, 1000);
}

//Check constraints count, if count=0, navigate to Route Desc
function HandleConstraintsAutoNavigationByCount(id, divId, selectedRadio) {
    var constraintCountCount = $('#hf_TotalConstraintsCount').val() || 0;
    var isPlanMovementFlag = $('#hf_IsPlanMovmentGlobal').length > 0;
    if ($('#route_assessment_next_btn').is(":visible") && parseInt(constraintCountCount) <= 0 && isPlanMovementFlag) {
        //navigate to Route Desc
        $('.sidebar-soa-Helper .clickable').css({ "cursor": "pointer", "color": "" });
        $('.sidebar-soa-Helper .clickable').attr("title", "");
        ListRouteDescription();
        LastStep = 2;
    } else {
        stopAnimation();
        //$('#confirm_btn').show();
        //$('#route_assessment_next_btn').hide();
        $('#SearchStructure').hide();
        $('#btn_ViewEdit_Route_Constraint').show();
        $('#div_StructureGeneralDetails').hide();
        ListConstraintsInit(selectedRadio);
        selectedmenu('Movements'); // for selected menu
        $('#div_ListConstraints').find('.route_select').eq(0).attr({ "checked": true });
        routeAnalysisPanelInitFlag = false; //reset it false to avoid automated navigation in further clicks
        $('#spnconstraints').show();
        openDetails(id, divId);
    }
}
function openDetails(id, idDiv) {
    //if (oldId !== id && oldIdDiv !== idDiv) {
        $('#' + idDiv).css('display','block');
        $('.clickable').removeClass('active1');
        $('#' + id).toggleClass('active1');
    //}
    //else if (this.oldIdDiv == idDiv && idDiv ==='drivingInstructions') {
    //    const x1 = document.getElementById(idDiv);
    //    x1.style.display = 'block';

    //    const x = document.getElementById(id);
    //    x.classList.toggle('active1');

    //    this.closeActiveText(this.oldId);
    //    this.oldIdDiv = idDiv
    //    this.oldId = id;
    //}
}
function BackToRouteAssessment() {
    startAnimation()
    $('#Organisation_ID').val(0);
    $('#Organisation_Name').val("");
    $('#Spn_OrgName').hide();
    $('#RBDrivingInstruction').show();
    $('#RBAnnot').show();
    $('#RBCaution').show();
    $('#RBConstraints').show();
    $('#RBRouteDescrp').show();
    $('#RBAffectedParty').show();
    $('#generatebutton').show();
    $('#RBAffectedRoad').show();
    $('#RBAffectedRoad_StructureWise').hide();

    $('#printbutton').hide();
    var revisionId = 0;
    var iscandidatert = $('#IsCandidateRT').val();
    isVR1Application = $('#VR1Applciation').val();
    if (iscandidatert == 'True' || iscandidatert == 'true') {
        revisionId = $('#revisionId').val();
        AnalysisId = $('#candAnalysisId').val();
    }

    if (SortStatus == "MoveVer") {
        AnalysisId = $('#analysis_id').val();
        revisionId = $('#revisionId').val();
    }
    //flag to set revision id = 0 in case of VR-1 route analysis
    if (isVR1Application == 'True' || isVR1Application == 'true') {
        revisionId = 0;
        versionid = $('#versionId').val();
    }

    var randomNum = Math.random();

    anal_Type = 7;//setting anal_type to pass to print
    var SORTflag = $('#SORTflag').val() ? $('#SORTflag').val() : false;

    $('#generatebutton').show();

    HideSpan();

    $('#affectedParties').load('../SORTApplication/AffectedParties?analysisId=' + AnalysisId + '&IsVR1=' + isVR1Application, function () {
        stopAnimation();
        $('#SearchStructure').hide();
        $('#div_StructureGeneralDetails').hide();
    });
    $('#affectedParties').show();
    openDetails('RBAffectedParty', 'affectedParties');
}
function ListAnnotation(_this) {
    var id = $(_this).attr("id");
    var divId = $(_this).attr("divid");
    $("#disclaimerBlock").hide();
    startAnimation();
    HideSpan();
    $('#printbutton').hide();
    revisionId = $('#RevID').val();
    AnalysisId = $('#AnalysisId').val();
    anal_Type = 6;

    if (iscandidatert == 'True' || iscandidatert == 'true') {
        revisionId = $('#revisionId').val();
        AnalysisId = $('#candAnalysisId').val();
        //AnalysisId = $('#analysis_id').val();
    }

    if (SortStatus == "MoveVer") {
        AnalysisId = $('#analysis_id').val();
        revisionId = 0;
        versionid = $('#versionId').val();
    }
    if ($('#SORTflag').val() != "true") {
        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        else { // special order
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        // $('#annotations').load('../RouteAssessment/ListAnnotations?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
        $('#annotations').load('../RouteAssessment/GetAffectedAnnotations?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
            {},
            function () {
                stopAnimation();
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
                ListAnnotationInit();
            });

    } else {
        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            AnalysisId = $('#analysis_id').val();
            versionid = $('#versionId').val();
        }
        // $('#annotations').load('../RouteAssessment/ListAnnotations?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + $('#SORTflag').val() + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
        $('#annotations').load('../RouteAssessment/GetAffectedAnnotations?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + $('#SORTflag').val() + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
            {},
            function () {
                stopAnimation();
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
                $('input[type="button"]').hide();
                $('.sortbuttons').show();
                ListAnnotationInit();
            });
    }
    $('#annotations').show();
    openDetails(id, divId);
    animateToTopOnRouteAssessmentNavigationRouteAnalNotif();
}
function ListAffectedRoads(_this) {
    var id = $(_this).attr("id");
    var divId = $(_this).attr("divid");
    var isStructureDetails = $(_this).attr("isstructuredetails");

    $("#disclaimerBlock").hide();
    startAnimation();
    HideSpan();
    $('#printbutton').hide();
    revisionId = $('#RevID').val();
    AnalysisId = $('#AnalysisId').val();
    anal_Type = 8;
    var Org_ID = $('#Organisation_ID').val() ? $('#Organisation_ID').val() : 0;

    if (iscandidatert == 'True' || iscandidatert == 'true') {
        revisionId = $('#revisionId').val();
        AnalysisId = $('#candAnalysisId').val();
    }

    if (SortStatus == "MoveVer") {
        AnalysisId = $('#analysis_id').val();
        revisionId = 0;
        versionid = $('#versionId').val();
    }

    //flag to set revision id = 0 in case of VR-1 route analysis
    if (isVR1Application == 'True' || isVR1Application == 'true') {
        revisionId = 0;
        AnalysisId = $('#analysis_id').val();
        versionid = $('#versionId').val();
    }

    //uncomment for the calls,
    if ($('#SORTflag').val() != "true") {
        //$('#affectedRoads').load('../RouteAssessment/AffectedRoads?analysisID=' + AnalysisId + '&ContentRefNo=' + $('#CRNo').val(), function () {
        $('#affectedRoads').load('../RouteAssessment/GetAffectedRoads?analysisID=' + AnalysisId + '&ContentRefNo=' + $('#CRNo').val() + '&isStructureAffected=' + isStructureDetails, function () {
            $('#SearchStructure').hide();
            $('#div_StructureGeneralDetails').hide();
            stopAnimation();
            ListAfeectedRoadInit();
        });
    }
    else {
        if (Org_ID != 0) {
            //$('#affectedRoads').load('../RouteAssessment/AffectedRoads?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&anal_type=' + anal_Type + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&FilterByOrgID=' + Org_ID, function () {
            $('#affectedRoads').load('../RouteAssessment/GetAffectedRoads?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&anal_type=' + anal_Type + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&FilterByOrgID=' + Org_ID + '&isStructureAffected=' + isStructureDetails, function () {
                stopAnimation();
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
                ListAfeectedRoadInit();
            });
            $('#routeAssessmentBckbtn').show();
        } else {
            //$('#affectedRoads').load('../RouteAssessment/AffectedRoads?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&anal_type=' + anal_Type + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application, function () {
            $('#affectedRoads').load('../RouteAssessment/GetAffectedRoads?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&anal_type=' + anal_Type + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&isStructureAffected=' + isStructureDetails, function () {
                stopAnimation();
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
                ListAfeectedRoadInit();
            });
        }
    }
    $('#affectedRoads').show();
    openDetails(id, divId);
    animateToTopOnRouteAssessmentNavigationRouteAnalNotif();
}
function ListDrivingInstructions(_this) {
    var id = $(_this).attr("id");
    var divId = $(_this).attr("divid");

    $("#disclaimerBlock").hide();
    startAnimation();
    HideSpan();
    AnalysisId = $('#AnalysisId').val();
    revisionId = $('#RevID').val();
    anal_Type = 1;//setting anal_type to pass to print
    $('#generatebutton').show();
    $('#printbutton').show();

    if (iscandidatert == 'True' || iscandidatert == 'true') {
        revisionId = $('#revisionId').val();
        AnalysisId = $('#candAnalysisId').val();
    }

    if ($('#SORTflag').val() != "true") {
        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        else { // special order
            revisionId = 0;
            versionid = $('#versionId').val();
        }
        //$('#drivingInstructions').load('../RouteAssessment/ListDrivingInstructions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&GenerateNewInstr=true',
        $('#drivingInstructions').html('');
        $('#drivingInstructions').load('../RouteAssessment/GetDrivingInstructions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&GenerateNewInstr=true',
            {},
            function () {
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
                stopAnimation();
            });
    } else {

        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            AnalysisId = $('#analysis_id').val();
            versionid = $('#versionId').val();
        }

        //$('#drivingInstructions').load('../RouteAssessment/ListDrivingInstructions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + $('#SORTflag').val() + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
        $('#drivingInstructions').load('../RouteAssessment/GetDrivingInstructions?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + $('#SORTflag').val() + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
            {},
            function () {
                //stopAnimation();
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
                if ($('#nodrive').val() == 'nodrive') {
                    $('#drivingInstructions').html('');
                    //GenerateInstr();
                }
                else { stopAnimation(); }
            });
    }
    openDetails(id, divId);
    $('#drivingInstructions').show();
    animateToTopOnRouteAssessmentNavigationRouteAnalNotif();
}
function PrintReport(_this) {
    var AnalysisId = $(_this).attr("analysisid");
    var anal_Type = 1;
    var link = "../RouteAssessment/PrintInstructionReport?AnalysisId=" + AnalysisId + "&anal_type=" + anal_Type + "";
    //condition to show the printable document in a new tab

    if ($('#hf_NoDataFound').val() != '1') {
        window.open(link, "_blank");
    }
    else {
        showWarningPopDialog('Functionality not implemented ...', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
}

