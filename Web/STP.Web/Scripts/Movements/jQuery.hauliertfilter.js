var inputData;
var isVr1App;
$('body').on('click', '#filter_haulier_movement .filters,#filter_SORT .filters', function (e) {
    e.preventDefault();
    if ($('#hf_IsPlanMovmentGlobal').length > 0 || $('#filter_haulier_movement').length > 0) {
        //filter_SORT - plan movement SORT  ,  filter_haulier_movement - haulier movement list filter
        var parentElem = $('#filter_haulier_movement').length > 0 ? '#filter_haulier_movement' : '#filter_SORT';
        var targetElem = $(this).data('target') || $(this).data('targetid');
        if ($(parentElem + ' #' + targetElem).is(":visible")) {//visible
            $(parentElem + ' #' + targetElem).css("display", "none");
            $(parentElem + ' #' + targetElem + " #chevlon-up-icon").css("display", "none");
            $(parentElem + ' #' + targetElem + " #chevlon-down-icon").css("display", "block");

            $(this).find(".chevlon-up-icon").css("display", "none");
            $(this).find(".chevlon-down-icon").css("display", "block");

        } else {
            $(parentElem + ' #' + targetElem).css("display", "block");
            $(parentElem + ' #' + targetElem + " #chevlon-up-icon").css("display", "block");
            $(parentElem + ' #' + targetElem + " #chevlon-down-icon").css("display", "none");

            $(this).find(".chevlon-up-icon").css("display", "block");
            $(this).find(".chevlon-down-icon").css("display", "none");
        }
    }
});
$(document).ready(function () {
    $('body').on('change', '.vehicle-dimension', function (e) {
        VehicleDimension(this);
    });
    $('body').on('change', '.operator-count', function (e) {
        OperatorCountRange(this)
    });
    $('body').on('click', '#AddOption', function (e) {
        e.preventDefault();
        AddVehicleFilter(this);
    });
    $('body').on('click', '#RemoveOption', function (e) {
        e.preventDefault();
        RemoveVehicleFilter(this);
    });
});
function ClearHaulierAdvancedData(resetData=true) {
    $("#ESDALReference").val('');
    $("#frmFilterMoveInbox #NotifyVSO").val('');
    var _advFilter = $('#filter_haulier_movement');
    _advFilter.find('input:text').each(function () {
        $(this).val('');
    });
    _advFilter.find('input:radio').eq(0).prop('checked', true);
    _advFilter.find('input:checkbox:not("#NeedsAttention")').prop('checked', false);

    _advFilter.find('input:checkbox').closest('.row').find('input:text').attr('disabled', 'disabled');
    _advFilter.find('option:selected').prop("selected",);
    $('#filter_haulier_movement').find('option:selected').prop("selected", false)
    if (resetData) {
        _advFilter.find('#NeedsAttention').prop('checked', false);
        $('#NeedsAttention').prop('checked', false);
        ResetData(true);
    }
    $(".VehicleFilter").slice(1).remove();
    $('#AddOption').show();
    $('#RemoveOption').hide();
    $('.gross_weightUnit').text('kg');
    $('.gross_weight1Unit').text('kg');
    var oprcount = $('#OperatorCount').val();
    if (oprcount == '<=') {
        $('#gross_weight_max_kg1').hide();
        $('.gross_weight1').hide();
    }

}
function WeightCountRange() {
    var WeightCount = $("#WeightCount").val();
    if (WeightCount == 2) {
        $(".GrossWeight1").show();
    }
    else {
        $(".GrossWeight1").hide();
    }
}
function WidthCountRange() {
    var WidthCount = $("#WidthCount").val();
    if (WidthCount == 2) {
        $(".OverallWidth1").show();
    }
    else {
        $(".OverallWidth1").hide();
    }
}
function LengthCountRange() {
    var LengthCount = $("#LengthCount").val();
    if (LengthCount == 2) {
        $(".Length1").show();
    }
    else {
        $(".Length1").hide();
    }
}
function HeightCountRange() {
    var Height = $("#HeightCount").val();
    if (Height == 2) {
        $(".Height1").show();
    }
    else {
        $(".Height1").hide();
    }
}
function AxleCountRange() {
    var AxleCount = $("#AxleCount").val();
    if (AxleCount == 2) {
        $(".AxleWeight1").show();
    }
    else {
        $(".AxleWeight1").hide();
    }
}
function ResetData(isclear) {
    $.ajax({
        url: '../Movements/ClearSOHaulierFilter',
        type: 'POST',
        success: function (data) {
            var pagesize = $('#pageSizeSelect').val();
            SearchHaulierList(false, pagesize, isclear);
            //SearchHaulierList();
            $('#filterimage').hide();

        }
    });
}
function DeleteSoApplication(HaulierReference, ApplicationRevId) {
    var Haulier_Reference = HaulierReference;
    let Msg = "Do you want to delete application?";
    if (Haulier_Reference != '')
        Msg = "Do you want to delete application \"" + Haulier_Reference + '\" ?';

    ShowWarningPopupParam(Msg, 'DeleteSoApp', '', ApplicationRevId, Haulier_Reference);
}
function DeleteSoApp(ApplicationRevId, Haulier_Reference) {
    CloseWarningPopup();
    var AppRevId = ApplicationRevId ? ApplicationRevId : 0;
    var HRef = Haulier_Reference;
    $.ajax({
        url: "../Application/DeleteApplication",
        type: 'POST',
        cache: false,        
        data: { apprevisionId: AppRevId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (result.Success) {
                let Msg = "Application deleted successfully";
                if (Haulier_Reference != '')
                    Msg = "\"" + HRef + '\" application deleted successfully';

                ShowSuccessModalPopup(Msg, 'loadmovementinbox');
            }
            else {
                let Msg = "Application not deleted";
                if (Haulier_Reference != '')
                    Msg = "\"" + HRef + '\" application not deleted';

                ShowErrorPopup(Msg);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function linktomovementinbox() {
    CloseSuccessModalPopup();
    window.location.href = '/Movements/MovementList';
}
function loadmovementinbox() {
    CloseSuccessModalPopup();
    window.location.href = '../Movements/MovementList';
}
function linktoSortmovementinbox() {
    CloseSuccessModalPopup();
    window.location.href = '/SORTApplication/SORTInbox';
}
function WithdrawSoApplication(projectid, Enteredbysort, ESDALReference, ApplicationRevId, movementType, RedirectParam) {
    
    var Enteredby = Enteredbysort;
    if (Enteredby == 0) {
        CheckLatestAppStatus(projectid, Enteredbysort, ESDALReference, ApplicationRevId, movementType, RedirectParam);// && app status checked for #7855
    }
    else {
        ShowErrorPopup('SORT created application cannot be withdrawn from Haulier!');
    }
}
function WithdrawSoApplicationFromSort(projectid, Enteredbysort, ESDALReference, ApplicationRevId, movementType, RedirectParam) {
  
    var project_id = projectid ? projectid : 0;
    var Enteredby = Enteredbysort;
    var ESDAL_Reference = ESDALReference;
    if (Enteredby == 1) {
        //CheckLatestAppStatus(project_id);// && app status checked for #7855
        let Msg = "Do you want to withdraw application?";
        if (ESDAL_Reference != '')
            Msg = "Do you want to withdraw application \"" + ESDAL_Reference + '\" ?';

        ShowWarningPopup(Msg, 'WithdrawSoApp', '', projectid, ApplicationRevId, ESDAL_Reference, movementType, RedirectParam);
    }
    else {
        ShowErrorPopup('Haulier created application cannot be withdrawn from SORT!');
    }
}
function WithdrawSoApp(projectid, ApplicationRevId, esdal_ref, movementType, RedirectParam) {
    
    CloseWarningPopup();
    var project_id = projectid ? projectid : 0;
    var AppRevId = ApplicationRevId ? ApplicationRevId : 0;
    var esdalRef = esdal_ref;
    var docType = 'Special Order';
    var MType = movementType;
    if (MType == 'special order') {
        docType = 'Special Order';
    }
    else { docType = 'VR-1'; }
    if (Flag_App_Status != 308001) {
        $.ajax({
            url: "../Application/WithdrawApplication",
            type: 'POST',
            cache: false,            
            data: { Project_ID: project_id, Doc_type: docType, EsdalRefNumber: esdalRef, app_rev_id: AppRevId },
            beforeSend: function () {
                startAnimation();
                ClearPopUp();
            },
            success: function (result) {
                if (result.Success) {
                    let Msg = "Application withdrawn successfully";
                    if (esdalRef != '')
                        Msg = "\"" + esdalRef + '\" application withdrawn successfully';
                    if (RedirectParam == 1) {
                        ShowSuccessModalPopup(Msg, 'linktomovementinbox');
                    } else {
                        ShowSuccessModalPopup(Msg, 'linktoSortmovementinbox');
                    }

                }
            },
            error: function (xhr, textStatus, errorThrown) {
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    else {
        var Msg = "A WIP application exists for the current project and hence cannot be withdrawn. Please delete the WIP application to continue.";
        ShowErrorPopup(Msg);
        $(".modal-content").height('16rem');
    }
}
function CheckLatestAppStatus(Proj_ID, Enteredbysort, ESDALReference, ApplicationRevId, movementType, RedirectParam) {// && app status checked for #7855
    CloseWarningPopup();
    $.ajax({
        url: "../Application/CheckLatestAppStatus",
        type: 'POST',
        cache: false,
        data: { Project_ID: Proj_ID },
        beforeSend: function () {
            startAnimation();
        },
        success: function (Result) {
            var dataCollection = Result;
            if (dataCollection.result.ApplicationStatus > 0) {
                Flag_App_Status = dataCollection.result.ApplicationStatus;
                LatestAppRevID = dataCollection.result.ApplicationrevId;

                let Msg = "Do you want to withdraw application?";
                if (ESDALReference != '')
                    Msg = "Do you want to withdraw application \"" + ESDALReference + '\" ?';

                ShowWarningPopup(Msg, 'WithdrawSoApp', '', Proj_ID, ApplicationRevId, ESDALReference, movementType, RedirectParam);
            }
            else {
                ShowErrorPopup("Error occurred while withdraw");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function DeleteNotification(HaulierReference, NotificationId) {
    var Haulier_Reference = HaulierReference;
    let Msg = "Do you want to delete notification?";
    if (Haulier_Reference != '')
        Msg = "Do you want to delete notification \"" + Haulier_Reference + '\" ?';
    ShowWarningPopupParam(Msg, 'DeleteNotifi','', NotificationId , Haulier_Reference);
}
function DeleteNotifi(NotificationId, Haulier_Reference) {
    CloseWarningPopup();
    var notifId = NotificationId ? NotificationId : 0;
    var HRef = Haulier_Reference;
    $.ajax({
        url: "../Notification/DeleteNotification",
        type: 'POST',
        cache: false,        
        data: { notificationId: notifId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (result) {
                let Msg = "Notification deleted successfully";
                if (Haulier_Reference != '')
                    Msg = "\"" + HRef + '\" notification deleted successfully';

                ShowSuccessModalPopup(Msg, 'loadmovementinbox');
            }
            else {
                let Msg = "Notification not deleted";
                if (Haulier_Reference != '')
                    Msg = "\"" + HRef + '\" notification not deleted';

                ShowErrorPopup(Msg);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function AddVehicleFilter(_this) {

    var divId = $(_this).closest('.VehicleFilter').attr('id');
    var condition = $('#' + divId).find('#Operator').val();
    var vehicleFiltercount = $("div[class*='VehicleFilter']").length;
    if (condition == "1" || condition == "2") {
        $('#' + divId).find('#errormsg').hide();
        var div = $('#viewAdvHaulier').html();
        if ($("#VehicleFilterData_" + vehicleFiltercount).length > 0) {
            vehicleFiltercount++;
        }
        var newFilterDiv = "#VehicleFilterData_" + vehicleFiltercount;
        var newDiv = $(div).find("#" + divId).attr("id", "VehicleFilterData_" + vehicleFiltercount);
        $('#VehicleFilterDiv').append(newDiv);
        $('#' + 'VehicleFilterData_' + vehicleFiltercount).find(".gross_weight1").hide();
        $('#' + 'VehicleFilterData_' + vehicleFiltercount).find('#RemoveOption').show();
        $('#' + divId).find('#AddOption').hide();
        $('#' + divId).find('#RemoveOption').show();

        $(newFilterDiv).find('.gross_weightUnit').text('kg');
        $(newFilterDiv).find('.gross_weight1Unit').text('kg');

        $(newFilterDiv).find('.vehicletextbox').attr("id", "gross_weight_max_kg");
        $(newFilterDiv).find('.vehicletextbox').attr("name", "gross_weight_max_kg");
        $(newFilterDiv).find('.vehicletextbox1').attr("id", "gross_weight_max_kg1");
        $(newFilterDiv).find('.vehicletextbox1').attr("name", "gross_weight_max_kg1");
    }
    else {
        $('#' + divId).find('#errormsg').show();
    }
}
function OperatorCountRange(_this) {

    var OperatorCount = $(_this).val();
    var divId = $(_this).closest('.VehicleFilter').attr('id');
    if (OperatorCount == "between") {
        $('#' + divId).find(".gross_weight1").show();
    }
    else {
        $('#' + divId).find(".gross_weight1").hide();
    }
}
function VehicleDimension(_this) {

    var fieldData = $(_this).val();
    var divId = $(_this).closest('.VehicleFilter').attr('id');
    $('#' + divId).find("#gross_weight_max_kg").attr("name", fieldData);
    $('#' + divId).find("#gross_weight_max_kg").attr("id", fieldData);
    $('#' + divId).find("#gross_weight_max_kg1").attr("name", fieldData + "1");
    $('#' + divId).find("#gross_weight_max_kg1").attr("id", fieldData + "1");
    if (fieldData != "gross_weight_max_kg" && fieldData != "max_axle_weight_max_kg") {
        $('#' + divId).find('.gross_weightUnit').text('m');
        $('#' + divId).find('.gross_weight1Unit').text('m');
    }
    else {
        $('#' + divId).find('.gross_weightUnit').text('kg');
        $('#' + divId).find('.gross_weight1Unit').text('kg');
    }
}
function RemoveVehicleFilter(_this) {
    var divId = $(_this).closest('.VehicleFilter').attr('id');
    var vehicleFiltercount = $("div[class*='VehicleFilter']").length;
    if (vehicleFiltercount > 1) {
        $('#' + divId).remove();
        if (vehicleFiltercount == 2) {
            $('.VehicleFilter').find('#AddOption').show();
            $('.VehicleFilter').find('#RemoveOption').hide();
        }
    }
}
$('body').on('click', '.reverse', function (e) {
    var Enteredbysort = $(this).data("enteredbysort");
    var ESDALReference = $(this).data("esdalreference");
    var revisionno = $(this).data("revisionno");
    var ApplicationrevId = $(this).data("applicationrevid");
    var movementType = $(this).data("movementtype");
    var versionId = $(this).data("versionid");
    var VersionNo = $(this).data("moveversionno");
    if (Enteredbysort == 0) {
        var MSG = "Click Yes to create a new version of \"" + ESDALReference + '\" for editing.';
        ShowWarningPopupCloneRenotif(MSG, function () {
            $('#WarningPopup').modal('hide');
            ReviseApplication(ESDALReference, revisionno, ApplicationrevId, movementType, versionId, VersionNo);
        });
    }
    else {
        ShowErrorPopup('SORT created application cannot be revised from haulier!');
    }
});
$('body').on('click', '.renotify', function (e) {
    var PreviousNotificationCode = $(this).data("previousnotificationcode");
    var notificationId = $(this).data("notificationid");
    var RevisionId = $(this).data("revisionid");
    var versionStatus = $(this).data('versionstatus');
    var vr1_renotify = 0;
    if (RevisionId > 0) {
        vr1_renotify = 1;
    }
    Renotify(PreviousNotificationCode, notificationId, vr1_renotify, versionStatus);
});
$('body').on('click', '.notifyapp', function (e) {
    var ApplicationrevId = $(this).data("applicationrevid");
    var versionstatus = $(this).data("versionstatus");
    var startdate = $(this).data("startdate");
    var enddate = $(this).data("enddate");
    var movementType = $(this).data("movementtype");
    var isVr1 = 1;
    isVr1App = true;
    if (movementType == 'special order' || movementType == 'so') { isVr1 = 0; isVr1App = false;}
    var VersionID = $(this).data("versionid");
    inputData = { versionId: VersionID, MoveStartDate: startdate, MoveEndDate: enddate, ApplrevisionId: ApplicationrevId, isVR1: isVr1, versionStatus: versionstatus };
    CheckIsBroken({ VersionId: inputData.versionId }, function (response) {
        GetBrokenRouteNotifyAppInitial(isVr1App, inputData, response);
    });
});
$('body').on('click', '.clone-application', function (e) {
    var ESDAL_Reference = $(this).data("esdalreference");
    var ApplicationrevId = $(this).data("applicationrevid");
    var movementType = $(this).data("movementtype");
    var VersionID = $(this).data("versionid");
    var isHistoric = $(this).data("historic");
    CloneApplication(ESDAL_Reference, VersionID, ApplicationrevId, movementType, isHistoric);
});
$('body').on('click', '.clonenotification', function (e) {
    var notifCode = $(this).data('notifcode');
    var notifId = $(this).data('notifid');
    var isHistoric = $(this).data('historic');
    var contentreferencenum = $(this).data('contentreferencenum');
    CloneNotification(notifCode, notifId, isHistoric, contentreferencenum);
});
$('body').on('click', '.deletesoaapplication', function (e) {
    var HaulierReference = $(this).data("haulierreference");
    var ApplicationRevId = $(this).data("applicationrevid");
    DeleteSoApplication(HaulierReference, ApplicationRevId)
});
$('body').on('click', '.deletenotification', function (e) {
    var HaulierReference = $(this).data("haulierreference");
    var NotificationId = $(this).data("notificationid");
    DeleteNotification(HaulierReference, NotificationId)
});
$('body').on('click', '.withdrawsoapplication', function (e) {
    var projectid = $(this).data("projectid");
    var Enteredbysort = $(this).data("enteredbysort");
    var ESDALReference = $(this).data("esdalreference");
    var ApplicationRevId = $(this).data("applicationrevisionid");
    var movementType = $(this).data("movementtype");
    var RedirectParam = $(this).data("redirectparam");
    WithdrawSoApplication(projectid, Enteredbysort, ESDALReference, ApplicationRevId, movementType, RedirectParam)
});
