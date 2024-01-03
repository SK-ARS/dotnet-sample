var id = 0;
var RoutePartId = 0;
var revisionId;
var hf_AccessToALSAT;
var hf_StructureAssessList;
var selectedStructList = [];
var totalStructureCount = 0;
var ViewOrEditRouteFromRouteAssessment = {};
var isAssessed = 0;

function ListAffectedStructuresInit(selectedRadio) {
    if (isAssessed) {
        var savedCheckboxStates = {};
        isAssessed = 0;
    }
    const checkboxes = $('.struct_select');
    savedCheckboxStates = JSON.parse(sessionStorage.getItem('checkboxStates')) || {};
    checkboxes.each(function () {
        const checkboxID = this.id;
        $(this).prop('checked', savedCheckboxStates[checkboxID]);
        $(this).change(function () {
            if (isAssessed) {
                sessionStorage.removeItem('checkboxStates');
                savedCheckboxStates = JSON.parse(sessionStorage.getItem('checkboxStates')) || {};
                isAssessed = 0;
            }
            savedCheckboxStates[checkboxID] = $(this).is(':checked');
            sessionStorage.setItem('checkboxStates', JSON.stringify(savedCheckboxStates));
        });
    });

    selectedmenu('Movements'); // for selected menu
    $('#SearchStructure').show();
    revisionId = $('#hf_revisionId').val();
    hf_affectstarray = $('#hf_AFStrArray').val().split(',');
    hf_AccessToALSAT = $('#hf_AccessToALSAT').val();
    hf_StructureAssessList = $('#hf_StructureAssessList').val()!=undefined? JSON.parse($('#hf_StructureAssessList').val().replace(/&quot;/ig, '"')):undefined;
    var Org_name = $('#Organisation_Name').val();
    var Application_SORT = $('#SORTApplication').val();
    if (Application_SORT == "True") {
        if (Org_name != "") {
            $('#div_mylistaffstruct').show();
            $('#Spn_OrgName').show();
            $('#Spn_OrgName').text(Org_name);
        }
    }
        
    if ($('#routediv1 td').length > 0) {
        $('#divNoAffected').hide();
    }
    else {
        $('#divNoAffected').show();
    }
    if (parseInt(hf_AccessToALSAT) != 1) {
        $('.struct_select').hide();
        $('.chk_SelectAll').hide();
    }

    AffectedStructuresInit(selectedRadio);
    if ($('#hf_IsPlanMovmentGlobal').length>0) {
        $('#viewEdit_Route').show();
    }

    if ($("#UserTypeId").val() == 696007) {
        CheckAssessmentResult();
    }

    if ($('#hf_isStructDataExist').length > 0 || $('.affected-structure-list-struct-ids').length==0) {
        $('#PerformAssessment').hide();
    }
}
$(document).ready(function () {
    $('body').on('click', '#PerformAssessment', function (e) {
        e.preventDefault();
        CheckAssessmentCount(this);
    });
    $('body').on('click', '#IDClearAffectedStructure', function (e) {
        e.preventDefault();
        ClearAffectedStructure(this);
    });
    $('body').on('click', '#IddisplayStructure', function (e) {
        e.preventDefault();
        displayStructure(this);
    });
    $('body').on('change', '#AffectedStructureXslt .chk_UnSuitableShowAllStructures', function (e) {
        if ($(this).prop('checked')) {
            $('#AffectedStructureXslt .chk_UnSuitableShowAllStructures').prop('checked', true);
        } else {
            $('#AffectedStructureXslt .chk_UnSuitableShowAllStructures').prop('checked', false);
        }
        displayStructure(this);
    });
    $('body').on('change', '#AffectedStructureXslt .chk_UnSuitableShowAllCautions', function (e) {
        if ($(this).prop('checked')) {
            $('#AffectedStructureXslt .chk_UnSuitableShowAllCautions').prop('checked', true);
        } else {
            $('#AffectedStructureXslt .chk_UnSuitableShowAllCautions').prop('checked', false);
        }
        displayStructure(this);
    });
    $('body').on('click', '#IDcloseFilters', function (e) {
        e.preventDefault();
        closeFilters(this);
    });

    $('body').on('change', '#chk_SelectAll', function (e) {
        var $inputs = $('#StructureAssessment input[type=checkbox]');

        if (e.originalEvent === undefined) {
            var allChecked = true;
            $inputs.each(function () {
                allChecked = allChecked && this.checked;
            });
            this.checked = allChecked;
            if (allChecked == true) {
                SelectAllStructures();
            }
        } else {
            $inputs.prop('checked', this.checked);
        }
    });
    $('body').on('change', '#LASStructs', function (e) {
        if (this.checked) {
            document.getElementById('LASUnderbridge').checked = true;
            document.getElementById('LASOverbridge').checked = true;
            document.getElementById('LASLevelCrossing').checked = true;
        }
        else {
            document.getElementById('LASUnderbridge').checked = false;
            document.getElementById('LASOverbridge').checked = false;
            document.getElementById('LASLevelCrossing').checked = false;
        }

    });
    $('body').on('change', '#LASUnderbridge', function (e) {
        if (this.checked) {
            document.getElementById('LASUnderbridge').checked = true;
            $('.list-aff-str-filter #LASStructs').prop("checked", false);
            document.getElementById('LASOverbridge').checked = false;
            document.getElementById('LASLevelCrossing').checked = false;
        }
        else {
            document.getElementById('LASStructs').checked = false;
            document.getElementById('LASOverbridge').checked = false;
            document.getElementById('LASLevelCrossing').checked = false;
        }

    });
    $('body').on('change', '#LASOverbridge', function (e) {
        if (this.checked) {
            document.getElementById('LASOverbridge').checked = true;
            $('.list-aff-str-filter #LASStructs').prop("checked", false);
            document.getElementById('LASUnderbridge').checked = false;
            document.getElementById('LASLevelCrossing').checked = false;
        }
        else {
            document.getElementById('LASStructs').checked = false;
            document.getElementById('LASUnderbridge').checked = false;
            document.getElementById('LASLevelCrossing').checked = false;
        }

    });
    $('body').on('change', '#LASLevelCrossing', function (e) {
        if (this.checked) {
            document.getElementById('LASLevelCrossing').checked = true;
            document.getElementById('LASOverbridge').checked = false;
            $('.list-aff-str-filter #LASStructs').prop("checked", false);
            document.getElementById('LASUnderbridge').checked = false;
        }
        else {
            document.getElementById('LASOverbridge').checked = false;
            document.getElementById('LASStructs').checked = false;
            document.getElementById('LASUnderbridge').checked = false;
        }

    });
    $('body').on('change', '#StructureAssessment input[type=checkbox]', function (e) {
        $('#chk_SelectAll').trigger('change');
    });
    $('body').on('change', '#showallstruct', function (e) {
        var a = $("#StructAnalysisID").val();
        if ($("#showallstruct").is(":checked")) {
            $("#ShowAllAffectStruct").val(true);
            DisplayAffectedStructure(a);
        }
        else {
            $("#ShowAllAffectStruct").val(false);
            DisplayAffectedStructure(a);
        }
    });


    $('body').on('click', '.btn_View_Org_Details_AffStr', function (e) {
        e.preventDefault();
        var popupId = $(this).data('target');
        $('#' + popupId).modal('show');
        ViewContactDetails_Obj.ContactParent = null;
    });
    $('body').on('click', '.orgPopupContent .CloseOrgPopup', function (e) {
        e.preventDefault();
        var popupId = $(this).data('target');
        $('#' + popupId).modal('hide');
        ViewContactDetails_Obj.ContactParent = null;
    });

    $('body').off('click', '.btnOpenCautionDetails');
    $('body').on('click', '.btnOpenCautionDetails', function (e) {
        e.preventDefault();
        var popupId = $(this).data('target');
        if ($('#' + popupId).is(":visible")) {
            $(this).attr('title', 'View Caution');
            $(this).attr('src', '/Content/assets/images/down-chevlon-filter.svg');
            $('#' + popupId).hide();
        }
        else {
            $(this).attr('title', 'Hide Caution');
            $(this).attr('src', '/Content/assets/images/up-chevlon-filter.svg');
            $('#' + popupId).show();
        }

    });

    $('body').off('click', '#viewEdit_Route');
    $('body').on('click', '#viewEdit_Route', function (e) {
        viewEditRouteFlagStructures = 1;
        viewEditRouteFlagConstraints = 1;
         vieweditflagStruct = 1;
         vieweditflagconst = 1;
        Structurearrayempty();
        ViewOrEditRouteFromRouteAssessment = { RouteId: $('#AffectedStructureXslt [name="route_select"]:checked').data('routeid'),Type:"STRUCTURE" };
        console.log(ViewOrEditRouteFromRouteAssessment);
        $('#viewHfRouteId').val(ViewOrEditRouteFromRouteAssessment.RouteId);
        $('#route_assessment_next_btn').hide();
        LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', {
            apprevisionId: RevisionIdVal, versionId: VersionIdVal, contRefNum: ContenRefNoVal, isNotif: IsNotifVal, workflowProcess: "HaulierApplication",
            IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), IsRouteSummaryPage: 0
        }, '#select_route_section', '', function () {
            MovementRouteInit(false, true);
        });
    });
});


function openFilters() {
    document.getElementById("filters").style.width = "450px";
    document.getElementById("banner").style.background = "white";
    document.getElementById("navbar").style.background = "white";
    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("filters").style.width = "200px";
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
    $("#filters button").removeAttr("card");
}

function closeFilters() {
    $('#filters').css("width", "0");
    $('#banner').css("filter", "unset");
    $('#navbar').css("filter", "unset");
}

function ListStructureIDs(StructID, StructCode, routeId, sectionId, structCount) {

    totalStructureCount = structCount;
    var test = "'" + StructCode + "_" + routeId + "'";
    if ($("#" + StructCode + "_" + routeId).prop("checked")) {
        var struct = { ESRN: StructCode, SectionId: sectionId, RouteId: routeId, RoutePartNo: 1 };
        selectedStructList.push(struct);
    }
    else {
        removeByAttr(selectedStructList, 'ESRN', StructCode);
    }
}
var removeByAttr = function (arr, attr, value) {
    var i = arr.length;
    while (i--) {
        if (arr[i]
            && arr[i].hasOwnProperty(attr)
            && (arguments.length > 2 && arr[i][attr] === value)) {

            arr.splice(i, 1);

        }
    }
    return arr;
}

function SelectUnselectToggle() {
    if ($('#chk_SelectAll').is(':checked')) {
        selectedStructList = hf_StructureAssessList;
        totalStructureCount = selectedStructList.length;
    }
    else {
        totalStructureCount = 0;
        selectedStructList = [];
    }
}

function CheckAssessmentResult() {
    var routeId = $('input[type=radio][name=route_select]:checked').data('routeid');
    var esdalRefNo = $("#ESDAL_Reference").val();
    var structList = [];
    structList = hf_StructureAssessList;
    $.ajax({
        url: '../RouteAssessment/GetStructureAssessmentResult',
        type: 'POST',
        data: { MovementRefNo: esdalRefNo, RouteID: routeId },
        dataType: 'json',
        success: function (data) {
            for (var i = 0; i < data.structureJSON.EsdalStructure.length; i++) {
                for (var j = 0; j < structList.length; j++) {
                    if (structList[j].ESRN == data.structureJSON.EsdalStructure[i].ESRN && structList[j].RouteId == routeId) {
                        var esrn = structList[j].ESRN;
                        var seq_number = data.structureJSON.EsdalStructure[i].SeqNumber;
                        var assessment_status = data.structureJSON.EsdalStructure[i].Status;
                        var status = "#status_" + esrn + "_" + routeId + "_" + structList[j].SectionId;
                        if (assessment_status == "Completed") {
                            $(".currently-loading").hide();
                            $(status).addClass("completedimg");
                            $(status).attr("title", "Assessment Status : Completed " + '\n' + "Sequence Number : " + seq_number);
                        }
                        else {
                            $(".currently-loading").hide();
                            $(status).addClass("inprogressimg");
                            $(status).attr("title", "Assessment Status : In Progress " + '\n' + "Sequence Number : " + seq_number);
                        }
                    }
                }
            }
            $(".currently-loading").hide();
        }
    });

}
function SelectUnselectAll(chk_SelectAll, routePartId) {
    var routePartId = $("." + routePartId).val();
    if ($('#' + chk_SelectAll).is(':checked')) {
        SelectAllStructures(routePartId);
    }
    else {
        UnselectAllStructures(routePartId);
    }
}
function SelectAllStructures(routepartId) {
    const checkboxes = $('.struct_select');
    const savedCheckboxStates = {};
    checkboxes.each(function () {
        const checkboxID = this.id;
        $(this).prop('checked', savedCheckboxStates[checkboxID]);
        savedCheckboxStates[checkboxID] = true;
        sessionStorage.setItem('checkboxStates', JSON.stringify(savedCheckboxStates));
    });
    var structureassesslist = hf_StructureAssessList;
    selectedStructList = structureassesslist;
    var routeId = routepartId;
    var selectedStructListByRoute = [];
    for (var i = 0; i < selectedStructList.length; i++) {
        var esrn = selectedStructList[i].ESRN;
        $('input:checkbox[id=' + esrn + '_' + routeId + ']').prop('checked', true);
    }
    totalStructureCount = selectedStructList.length;
}
function UnselectAllStructures(routepartId) {
    sessionStorage.removeItem('checkboxStates');
    var structureassesslist = hf_StructureAssessList;
    selectedStructList = structureassesslist;
    var routeId = routepartId;
    for (var i = 0; i < structureassesslist.length; i++) {
        var esrn = structureassesslist[i].ESRN;
        $('input:checkbox[id=' + esrn + '_' + routeId + ']').prop('checked', false);
    }
    totalStructureCount = 0;
    selectedStructList = [];
}
function CheckAssessmentCount() {
    sessionStorage.removeItem('checkboxStates');
    var routeId = $('input[type=radio][name=route_select]:checked').data('routeid');
    var dataToSend = JSON.stringify(selectedStructList);
    if (totalStructureCount == 0 || dataToSend == "[]") {
        showToastMessage({
            message: "Please select at least one structure to send for assessment",
            type: "error"
        });
        return false;
    }
    var notificationID = $("#NOTIFICATION_ID").val();
    var esdalRefNo = $("#ESDAL_Reference").val();
    var analysisID = $("#StructAnalysisID").val();
    startAnimation("Checking for any existing assessment for the selected structures. Please wait...");
    $.ajax({
        url: '../RouteAssessment/GetStructureAssessmentCount',
        type: 'POST',
        data: { StuctureList: JSON.stringify(selectedStructList), MovementRefNo: esdalRefNo, RouteID: routeId },
        dataType: 'json',
        success: function (result) {
            if (result.success == 1) {
                stopAnimation();
                ShowDialogWarningPop('One or more structure(s) are already under assessment. Do you want to perform the assessment again? ', 'Cancel', 'Ok', 'cancelPerformAssessment', 'PerformAssessment', 1, 'warning');
            }
            else {
                stopAnimation();
                PerformAssessment();
            }
        }
    });
}
//perform assessment for the selected structures
function PerformAssessment() {
    WarningCancelBtn();
    var routeId = $('input[type=radio][name=route_select]:checked').data('routeid');
    var dataToSend = JSON.stringify(selectedStructList);
    var notificationID = $("#NOTIFICATION_ID").val();
    var esdalRefNo = $("#ESDAL_Reference").val();
    var analysisID = $("#StructAnalysisID").val();
    startAnimation("Structure assessment has been started. Please wait…");
    $.ajax({
        url: '../RouteAssessment/PerformStructureAssessment',
        type: 'POST',
        data: JSON.stringify({ StuctureList: selectedStructList, NotificationID: notificationID, MovementRefNo: esdalRefNo, AnalysisID: analysisID, RouteID: routeId }),
        beforeSend: function () {

        },
        success: function (result) {
            if (result.sequenceNo > 0) {
                isAssessed = 1;
                for (var i = 0; i < selectedStructList.length; i++) {
                    var esrn = selectedStructList[i].ESRN;
                    var status = "#status_" + esrn + "_" + routeId + "_" + selectedStructList[i].SectionId;
                    $(status).removeClass("completedimg");
                    $(status).addClass("inprogressimg");
                    $(status).attr("title", "Assessment Status : In Progress " + '\n' + "Sequence Number : " + result.sequenceNo);
                }
                stopAnimation();
                UnselectAllStructures(routeId);
                ShowInfoPopup('The selected structures have been sent for assessment. The sequence number is ' + result.sequenceNo);
            }
            else {
                stopAnimation();
                ShowDialogWarningPop('The selected structures cannot be sent for assessment ', 'Ok', '', 'WarningCancelBtn', '', 1, 'warning');
            }
        },
        error: function () {
        },
        complete: function () {
        }
    });

}
function cancelPerformAssessment() {
    UnselectAllStructures();
    WarningCancelBtn();
}

function ShowNoteToHaulier(Note, structureCode) {
    startAnimation();
    $.ajax({
        type: 'POST',
        url: '../RouteAssessment/ViewNoteToHaulier',
        data: JSON.stringify({
            Note: Note, structureCode: structureCode
        }),
        beforeSend: function (xhr) {
        },
        success: function (result) {
            $('#generalPopupContent').html(result);
            $(".modal-content").css({ "width": "32rem" });
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
            $('#generalPopup').modal('show');
            $('.loading').hide();
        }
    })
}

function ViewAssessmentComment(comment, structureCode) {
    resetdialogue();
    startAnimation();
    $.ajax({
        type: 'POST',
        url: '../RouteAssessment/ViewAssessmentComment',
        data: JSON.stringify({
            comment: comment, structureCode: structureCode
        }),
        beforeSend: function (xhr) {
        },
        success: function (result) {
            $('#generalPopupContent').html(result);
            $(".modal-content").css({ "width": "32rem" });
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
            $('#generalPopup').modal('show');
            $('.loading').hide();
        }
    })
}

function DisplayStructureGENT(StructCode, routeId, sectionId) {

    $('#hfRouteID').val(routeId);
    ShowStructuredetailspopup(StructCode, sectionId);
    
}
function ClearAffectedStructure() {
    document.getElementById('LASStructs').checked = false;
    document.getElementById('LASUnderbridge').checked = false;
    document.getElementById('LASOverbridge').checked = false;
    document.getElementById('LASLevelCrossing').checked = false;
    displayStructure();
}
function displayStructure() {
    var selectedRadio = $('#AffectedStructureXslt input[name=route_select]:checked').val();
    var _iscandidatert = $('#IsCandidateRT').val();
    var routid = $('#routeId').val();
    var unSuitableShowAllStructures = $('#AffectedStructureXslt .chk_UnSuitableShowAllStructures').prop('checked') == true ? true : false;//true-all structures , false - show only unsuitable
    var unSuitableShowAllCautions = $('#AffectedStructureXslt .chk_UnSuitableShowAllCautions').prop('checked') == true ? true : false;//true-all Cautions , false - show only unsuitable
    var analysisId = $('#AnalysisID').val();
    var Org_ID = $('#Organisation_ID').val() ? $('#Organisation_ID').val() : 0;
    if (iscandidatert == 'True' || iscandidatert == 'true') {
        revisionId = $('#revisionId').val();
        AnalysisId = $('#candAnalysisId').val();
    }
    if (SortStatus == "MoveVer") {
        AnalysisId = $('#analysis_id').val();
        revisionId = $('#revisionId').val();
    }
    var IsOverbridge = $('#LASOverbridge').is(':checked');
    var IsUnderbridge = $('#LASUnderbridge').is(':checked');
    var IsLevelCrossing = $('#LASLevelCrossing').is(':checked');
    var isPlanMovment = $('#hf_IsPlanMovmentGlobal').length > 0;

    //$('#AffectedStructure_xslt').html('');
    startAnimation();
    $('#AffectedStructure_xslt').load('../RouteAssessment/GetAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + _iscandidatert + '&FilterByOrgID=' + Org_ID + '&UnSuitableShowAllStructures=' + unSuitableShowAllStructures
        + '&IsOverbridge=' + IsOverbridge + '&IsUnderbridge=' + IsUnderbridge + '&IsLevelCrossing=' + IsLevelCrossing + '&UnSuitableShowAllCautions=' + unSuitableShowAllCautions + '&IsPlanMovement=' + isPlanMovment,
        {},
        function () {
            if (IsOverbridge)
                $('#LASOverbridge').prop('checked', true);
            if (IsUnderbridge)
                $('#LASUnderbridge').prop('checked', true);
            if (IsLevelCrossing)
                $('#LASLevelCrossing').prop('checked', true);
            ListAffectedStructuresInit(selectedRadio);
        });
    stopAnimation();
    closeFilters();
}
