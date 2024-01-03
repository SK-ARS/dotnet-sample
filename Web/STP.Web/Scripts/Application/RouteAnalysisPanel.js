oldId = 'RBAnnot';
var iscandidatert;
var SortStatus;
var AnalysisId;
var revisionId;
var versionid = 0;
var isVR1Application = "";
var contentRefNo;
var analysisid;
var hf_contentRefNo;
var chk_status;
var sort_user_id;
var checker_id;
var routeAnalysisPanelInitFlag = false;

function RouteAnalysisPanelInit() {
    routeAnalysisPanelInitFlag = true;
    iscandidatert = $('#IsCandidateRT').val();
    SortStatus = $('#SortStatus').val();
    AnalysisId = "";
    revisionId = "";
    contentRefNo = $('#CRNo').val() != undefined ? $('#CRNo').val() : "";
    analysisid = $('#hf_AnalysisId').val();
    hf_contentRefNo = $('#hf_ContentRefNo').val();
    chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;

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
    //
    //if ($('#SortUserTypeId').val() == 696008 || $('#SortUserTypeId').val() == "696008" || $('#hf_IsPlanMovmentGlobal').length<=0) {
    //    ListDrivingInstructions();
    //} else {
    //    ListAffectedStructures();
    //}
    ListDrivingInstructions();

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
    $('#RBDrivInstr').attr('checked', true);
}
$(document).ready(function () {
    $('body').on('click', '#RBDrivingInstruction', function () { ListDrivingInstructions(this); });
    $('body').on('click', '#sm1', function () { window['openStructure'](); });
    $('body').on('click', '#RBAffectStruct', function () { ListAffectedStructures(this); } );
    $('body').on('click', '#BackToRouteAssessment', function () { BackToRouteAssessment(this); } );
    $('body').on('click', '#RBAnnot', function () { ListAnnotation(this); } );
    $('body').on('click', '#RBCaution', function () { ListCautions(this); } );
    $('body').on('click', '#RBConstraints', function () { ListConstraints(this); } );
    $('body').on('click', '#RBRouteDescrp', function () { ListRouteDescription(this); });
    $('body').on('click', '#PrintReport', function () { PrintReport(this); } );
    $('body').on('click', '#RBAffectedParty', function () { ListAffectedParty(this); } );
    $('body').on('click', '#RBAffectedRoad', function () { ListAffectedRoads(this); } );
    $('body').on('click', '#RBAffectedRoad_StructureWise', function () { ListAffectedRoads(this); } );
});


function closeActiveText(oldId) {
    $('#' + oldId).removeClass("active1");
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
                    data: { analysisID: analysisid, anal_type: chkAnal_type, revisionId: revisionId, versionId: versionid, ContentRefNo: contentRefNo, GenerateNewInstr: GenerateNewInstr, IsCandidate: _iscandidatert, IsVR1: isVR1Application },
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
                            /* showWarningPopDialog('Driving instruction generated successfully', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');*/

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
                    data: { analysisID: analysisid, anal_type: 1, revisionId: revisionId, ContentRefNo: contentRefNo, GenerateNewInstr: GenerateNewInstr, IsCandidate: _iscandidatert, IsVR1: isVR1Application },
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
                                //stopAnimation();
                                //$('#SearchStructure').hide();
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
function ListDrivingInstructions(_this) {
    //'RBDrivingInstruction', 'drivingInstructions'
    var id = $(_this).attr('id') || 'RBDrivingInstruction';
    var divId = $(_this).attr('divid') || 'drivingInstructions';
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
    animateToTopOnRouteAssessmentNavigationRouteAnalPanel();
}
function ListAffectedStructures(_this) {
    var id = $(_this).attr('id') ||'RBAffectStruct';
    var divId = $(_this).attr('divid') ||'affectedstructure';
    startAnimation();
    HideSpan();
    $('#AffectedStructure_xslt').html('');
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
    var SORTflag = $('#SORTflag').val();
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
        $('#affectedstructure').load('../RouteAssessment/GetAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
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
            $('#affectedstructure').load('../RouteAssessment/GetAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&FilterByOrgID=' + Org_ID,
                {},
                function () {
                    HandleAffectedStructureAutoNavigationByCount(id, divId);
                });
            $('#routeAssessmentBckbtn').show();
        }
        else {
            //$('#affectedstructure').load('../RouteAssessment/ListAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
            $('#affectedstructure').load('../RouteAssessment/GetAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + SORTflag + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
                {},
                function () {
                    HandleAffectedStructureAutoNavigationByCount(id, divId);
                }
            );

        }
    }
    //stopAnimation();
   
    animateToTopOnRouteAssessmentNavigationRouteAnalPanel();
}

//Check Unsuitable structure count, if count=0, navigate to constraints
function HandleAffectedStructureAutoNavigationByCount(id, divId) {
    var structuresCount = $('#hf_TotalStructureCount').val() || 0;
    var isPlanMovementFlag = $('#hf_IsPlanMovmentGlobal').length > 0;
    if (routeAnalysisPanelInitFlag && parseInt(structuresCount) <= 0 && isPlanMovementFlag) {
        //navigate to constraints
        ListConstraints();
    } else {
        stopAnimation();
        $('#SearchStructure').show();
        $('#div_StructureGeneralDetails').hide();
        routeAnalysisPanelInitFlag = false; //reset it false to avoid automated navigation in further clicks
        ListAffectedStructuresInit();
        openDetails(id, divId);
        $('#affectedstructure').show();
        $('#showAffectedparties').hide();
    }
}

function ListAnnotation(_this) {
    var id = $(_this).attr('id');
    var divId = $(_this).attr('divid');
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
    animateToTopOnRouteAssessmentNavigationRouteAnalPanel();
}
function ListRouteDescription(_this) {
    var id = $(_this).attr('id') ||'RBRouteDescrp';
    var divId = $(_this).attr('divid') ||'routeDescription';
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

        $('#routeDescription').load('../RouteAssessment/ListRouteDescription?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
            {},
            function () {
                stopAnimation();
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
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
                selectedmenu(2);
                stopAnimation();
                $('#SearchStructure').hide();
                $('#div_StructureGeneralDetails').hide();
            });
    }
    $('#routeDescription').show();
    openDetails(id, divId);
    animateToTopOnRouteAssessmentNavigationRouteAnalPanel();
}
function ListAffectedParty(_this) {
    var id = $(_this).attr('id');
    var divId = $(_this).attr('divid');
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
        versionid = $('#versionId').val();
    }

    var randomNum = Math.random();

    if ($('#SORTflag').val() != "true") {
        $('#affectedParties').load('../Notification/GetAffectedParties?AnalysisId=' + AnalysisId + '&NotificationID=' + $('#NotificationId').val() + '&ContentRefNo=' + $('#CRNo').val() + '&VSOType=' + $('#VSOType').val(), function () {
            WarningCancelBtn();
            $('#SearchStructure').hide();
            $('#div_StructureGeneralDetails').hide();
            stopAnimation();
        });

    } else {
        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            AnalysisId = $('#analysis_id').val();
            versionid = $('#versionId').val();
        }
        var haulierOrgId = $('#OrganisationId').val();
        var apprevisionid = $('#ApprevId').val();
        //Notification id is not passed ! passed as 0 this is from sort side affected parties page needs to be called
        $('#affectedParties').load('../SORTApplication/GetAffectedParties?analysisId=' + AnalysisId + '&IsVR1=' + isVR1Application + '&revisionId=' + apprevisionid + '&haulierOrgId=' + haulierOrgId , function () {
            WarningCancelBtn();
            stopAnimation();
            $('#SearchStructure').hide();
            $('#div_StructureGeneralDetails').hide();
            Sort_AffectedPartiesInit();
            ManualAddedPartiesInit();
        });

    }
    openDetails(id, divId);
    $('#affectedParties').show();
    animateToTopOnRouteAssessmentNavigationRouteAnalPanel();
}
function ListAffectedRoads(_this) {
    var id = $(_this).attr('id');
    var divId = $(_this).attr('divid');
    var isStructureDetails = $(_this).attr('isstructuredetails');
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
    animateToTopOnRouteAssessmentNavigationRouteAnalPanel();
}
function ListCautions(_this) {
    var id = $(_this).attr('id');
    var divId = $(_this).attr('divid');
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
                ListCautionsInit();
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
                ListCautionsInit();
            });
    }
    $('#spncautions').show();
    openDetails(id, divId);
    animateToTopOnRouteAssessmentNavigationRouteAnalPanel();
}
function ListConstraints(_this) {
    var selectedRadio = $('#div_ListConstraints input[name=route_select]:checked').val();
    startAnimation();
    var id = $(_this).attr('id') ||'RBConstraints';
    var divId = $(_this).attr('divid') ||'spnconstraints';
    var UnSuitableShowAllConstraint = $('#div_ListConstraints .chk_UnSuitableShowAllConstraint').prop('checked') == true ? true : false;//true-all constraints , false - show only unsuitable
    var UnSuitableShowAllCautions = $('#div_ListConstraints .chk_UnSuitableShowAllCautions').prop('checked') == true ? true : false;//true-all cautions , false - show only unsuitable
    //startAnimation();
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
        $('#spnconstraints').load('../RouteAssessment/GetAffectedConstraints?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&UnSuitableShowAllConstraint=' + UnSuitableShowAllConstraint + '&UnSuitableShowAllCautions=' + UnSuitableShowAllCautions,
            {},
            function () {
                HandleConstraintsAutoNavigationByCount(id,divId,selectedRadio);
            });

    } else {
        //flag to set revision id = 0 in case of VR-1 route analysis
        if (isVR1Application == 'True' || isVR1Application == 'true') {
            revisionId = 0;
            AnalysisId = $('#analysis_id').val();
            versionid = $('#versionId').val();
        }

        //$('#spnconstraints').load('../RouteAssessment/ListConstraints?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + $('#SORTflag').val() + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application,
        $('#spnconstraints').load('../RouteAssessment/GetAffectedConstraints?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&SORTflag=' + $('#SORTflag').val() + '&IsCandidate=' + iscandidatert + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&UnSuitableShowAllConstraint=' + UnSuitableShowAllConstraint + '&UnSuitableShowAllCautions=' + UnSuitableShowAllCautions,
            {},
            function () {
                HandleConstraintsAutoNavigationByCount(id, divId, selectedRadio);
            });
    }
    animateToTopOnRouteAssessmentNavigationRouteAnalPanel();
    
}

function animateToTopOnRouteAssessmentNavigationRouteAnalPanel() {
    $('body').animate({
        scrollTop: eval($('body').offset().top)
    }, 1000);
}

//Check constraints count, if count=0, navigate to Route Desc
function HandleConstraintsAutoNavigationByCount(id, divId, selectedRadio) {
    
    var constraintCountCount = $('#hf_TotalConstraintsCount').val() || 0;
    var isPlanMovementFlag = $('#hf_IsPlanMovmentGlobal').length > 0;
    if (routeAnalysisPanelInitFlag && parseInt(constraintCountCount) <= 0 && isPlanMovementFlag) {
        //navigate to Route Desc
        ListRouteDescription();
    } else {
        stopAnimation();
        $('#SearchStructure').hide();
        $('#div_StructureGeneralDetails').hide();
        routeAnalysisPanelInitFlag = false; //reset it false to avoid automated navigation in further clicks
        ListConstraintsInit(selectedRadio);
        $('#spnconstraints').show();
        openDetails(id, divId);
    }
}

var oldIdDiv = "";
function openDetails(id, idDiv) {
    if (oldId !== id && oldIdDiv !== idDiv) {
        $('#' + idDiv).css("display","block");
        $('#' + id).addClass("active1");
        closeActiveText(oldId);
        oldIdDiv = idDiv
        oldId = id;
    }
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


function PrintReport(_this) {
    var AnalysisId = $(_this).attr('analysisid');
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

