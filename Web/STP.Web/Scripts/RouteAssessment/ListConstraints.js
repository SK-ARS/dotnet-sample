$(document).ready(function () {
    $('body').on('click', '.btn_ViewConstraints', function (e) {
        e.preventDefault();
        var ecrn = $(this).data('ecrn');
        ViewConstraints(ecrn);
    });

    //**************Constraint---Caution related events
    $('body').on('change', '#div_ListConstraints .chk_UnSuitableShowAllConstraint', function (e) {
        if ($(this).prop('checked')) {
            $('#div_ListConstraints .chk_UnSuitableShowAllConstraint').prop('checked', true);
        } else {
            $('#div_ListConstraints .chk_UnSuitableShowAllConstraint').prop('checked', false);
        }
        ListConstraints();
    });
    $('body').on('change', '#div_ListConstraints .chk_UnSuitableShowAllCautions', function (e) {
        if ($(this).prop('checked')) {
            $('#div_ListConstraints .chk_UnSuitableShowAllCautions').prop('checked', true);
        } else {
            $('#div_ListConstraints .chk_UnSuitableShowAllCautions').prop('checked', false);
        }
        ListConstraints();
    });

    $('body').on('click', '.btn_View_Org_Details_Constraint', function (e) {
        e.preventDefault();
        var popupId = $(this).data('target');
        $('#' + popupId).modal('show');
    });
    $('body').on('click', '.orgPopupConstraintContent .CloseOrgPopup', function (e) {
        e.preventDefault();
        var popupId = $(this).data('target');
        $('#' + popupId).modal('hide');
    });

    $('body').off('click', '.btnOpenCautionDetailsConstraint');
    $('body').on('click', '.btnOpenCautionDetailsConstraint', function (e) {
        e.preventDefault();
        var popupId = $(this).data('target');
        if ($('#' + popupId).is(":visible")) {
            $(this).attr('title', 'View Caution');
            $('#' + popupId).slideToggle();
            $(this).attr('src', '/Content/assets/images/down-chevlon-filter.svg');
        }
        else {
            $(this).attr('title', 'Hide Caution');
            $('#' + popupId).slideToggle();
            $(this).attr('src', '/Content/assets/images/up-chevlon-filter.svg');
        }

    });
    //**************Constraint---Caution related events === END
    $('body').off('click', '#btn_ViewEdit_Route_Constraint');
    $('body').on('click', '#btn_ViewEdit_Route_Constraint', function (e) {
        viewEditRouteFlagStructures = 1;
        viewEditRouteFlagConstraints = 1;
         vieweditflagStruct = 1;
         vieweditflagconst = 1;
        
        Structurearrayempty();
        ViewOrEditRouteFromRouteAssessment = { RouteId: $('.constraints-container [name="route_select"]:checked').data('routeid'), Type: "CONSTRAINT" };
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
var hf_afconstarray;
function ListConstraintsInit(selectedRadio) {
    selectedmenu('Movements'); // for selected menu
    if (selectedRadio == undefined || selectedRadio == null) {
        $('#div_ListConstraints .route_select').eq(0).attr({ "checked": true });
        $('#div_ListConstraints .routedivclasscon').hide();
        $('#div_ListConstraints .routedivclasscon').eq(0).show();
        hf_afconstarray = $('#hf_AfConArray').val().split(',');
    } else {
        $('#div_ListConstraints [name=route_select][value="' + selectedRadio + '"]').prop('checked', true).trigger('change');
    }

    
    $('body').off('change', '#div_ListConstraints .route_select');//to avoid duplicate click
    $('body').on('change', '#div_ListConstraints .route_select', function (e) {
        e.preventDefault();
        selectConstraints(this);
    });
}
function selectConstraints(x) {
    $('.routedivclasscon').hide();
    $('#' + x.value).show();
    if (hf_afconstarray.includes(x.value)) {
        var pos = 0;
        for (var i = 0; i < hf_afconstarray.length - 1; i++) {
            if (hf_afconstarray[i] == x.value) {
                break;
            }
            pos = pos + 1;
        }
       hf_afconstarray.splice(pos,1);
    }
}

function ViewConstraints(code) {
    var constraintID = 0;
    $.ajax({
        url: '../RouteAssessment/GetConstraintId',
        type: 'POST',
        cache: false,
        async: false,
        data: { ConstraintCode: code },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            //alert(data.result.check);
            constraintID = data.result.check;
            var randomNumber = Math.random();
            $("#dialogue").load('../Constraint/ViewConstraint?ConstraintID=' + constraintID + '&flageditmode=false' + '&random=' + randomNumber, function () {
                ViewConstraintInit();
                stopAnimation();
                $("#dialogue").show();
                $("#overlay").show();
            });
            //removescroll();
        },
        error: function (xhr, textStatus, errorThrown) {

        },
        complete: function () {
            //stopAnimation();
        }
    });
}

//$('#div_ListConstraints input[name="route_select"]:checked').val();
