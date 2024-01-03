var VariableObject = $('#VariableObject').val();
var ObjectValue = JSON.parse(VariableObject);

var SessionObject = $('#SessionObject').val();
var SessionValue = JSON.parse(SessionObject);

//Script Copied from PlanMovement.cshtml


let BackNavigateUrl;
let StepFlag = 0;
let SubStepFlag = 0;
let CurrentStep = "";
let SelectedVehicles = [];
let BackToPreviousMovementList = false;
let BackToPreviousRouteMovementList = false;
let AmendFlag = 1;

$(document).ready(function () {
    $('#AppRevisionId').val(ObjectValue.RevisionId);
    $('#NotificationId').val(ObjectValue.NotificationId);
    $('#MovementId').val(ObjectValue.VehicleMoveId);
    if (ObjectValue.IsRouteAssessmentDone == "True" || ObjectValue.IsRouteAssessmentDone == 'true') { $('#IsRouteAssessment').val(true); }

    if (ObjectValue.NotificationId > 0) {
        $('#NotifVersionId').val(ObjectValue.VersionId);
        $("#IsNotif").val(true);
        $('#CRNo').val(ObjectValue.ContenRefNo);
        $('#NotifVersionId').val(ObjectValue.VersionId);
        $('#NotifAnalysisId').val(ObjectValue.AnalysisId);
    }
    else {
        $('#AppVersionId').val(ObjectValue.VersionId);
        $("#IsNotif").val(false);
    }
    NavigateURL();
});

function NavigateURL() {
    if (ObjectValue.IsSortApp == "True") {
        if (ObjectValue.IsRevise == "True") {
            $('#IsSortRevise').val(true);
        }
    }
    var type = "POST";
    var url = '';
    var data = {};
    var div;
    HideUnloadSections();
    $('#plan_movement_hdng').text("PLAN MOVEMENT");
    if (ObjectValue.IsApp == "True") {
        if (ObjectValue.IsVr1App == 'False') {
            $("#IsSoApp").val(true);
            $('#plan_movement_hdng').text("APPLY FOR SPECIAL ORDER");
            $('#apply_btn').text('APPLY FOR SPECIAL ORDER');
        }
        else {
            $("#IsVR1").val(true);
            $('#plan_movement_hdng').text("APPLY FOR VR1");
            $('#apply_btn').text('APPLY FOR VR1');
        }
    }
    else if (ObjectValue.NotificationId > 0) {
        $('#plan_movement_hdng').text("NOTIFICATION");
        $('#apply_btn').text('SUBMIT NOTIFICATION');
    }

    if (ObjectValue.NextAction != 0) {
        SelectMenu(2);
        switch (ObjectValue.NextAction) {
            case 0:
                if (ObjectValue.isSortApp == "True") {
                    url = '../SORTApplication/HaulierDetails';
                    data = { SORTStatus: "CreateSO" };
                    div = '#haulier_details_section';
                }
                else {
                    url = '../Movements/SelectVehicle';
                    div = '#select_vehicle_section';
                }
                break;
            case 1:
                url = '../Movements/SelectVehicle';
                div = '#select_vehicle_section';
                break;
            case 2:
                url = '../Movements/MovementSelectedVehicles';
                data = { movementId: ObjectValue.VehicleMoveId };
                div = '#vehicle_details_section';
                break;
            case 3:
                url = '../Movements/GetMovementTypeConfirmation';
                data = {
                    apprevisionId: ObjectValue.RevisionId, appVersionId: ObjectValue.VersionId, notificationId: ObjectValue.NotificationId
                };
                div = '#movement_type_confirmation';
                break;
            case 4:
                url = '../Routes/MovementRoute';
                data = {
                    apprevisionId: ObjectValue.RevisionId, versionId: ObjectValue.VersionId, contRefNum: ObjectValue.ContenRefNo, isNotif: ObjectValue.IsNotif, workflowProcess: "HaulierApplication",
                    IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val()
                };
                div = '#select_route_section';
                break;
            case 5:
                if (ObjectValue.isNotification == 'True') {
                    url = '../Notification/NotificationRouteAssessment';
                    data = { workflowProcess: 'HaulierApplication' };
                    div = '#route_assessment_section';
                }
                else {
                    if (ObjectValue.IsSupplimentarySaved == 'True') {
                        type = "POST";
                        url = '../Application/ViewSupplementary';
                        data = { appRevisionId: ObjectValue.RevisionId };
                        div = '#supplimentary_info_section';
                    }
                    else {
                        type = "POST";
                        url = '../Application/ApplicationSupplimentaryInfo';
                        data = { appRevisionId: ObjectValue.RevisionId };
                        div = '#supplimentary_info_section';
                    }
                }
                break;
            case 5.1:
                if (ObjectValue.isNotification == 'True') {
                    url = '../Notification/NotificationRouteAssessment';
                    data = { workflowProcess: 'HaulierApplication' };
                    div = '#route_assessment_section';
                }
                else {
                    type = "POST";
                    url = '../Application/ViewSupplementary';
                    data = { appRevisionId: ObjectValue.RevisionId };
                    div = '#supplimentary_info_section';
                }
                break;
            case 6:
                if (ObjectValue.IsNotif == 'True') {
                    if (ObjectValue.IsNotifGeneralSaved == 'True') {
                        type = "POST";
                        url = '../Notification/NotificationOverview';
                        data = { notificationId: ObjectValue.NotificationId, workflowProcess: "HaulierApplication" };
                        div = '#overview_info_section';
                    }
                    else {
                        type = "POST";
                        url = '../Notification/SetNotificationGeneralDetails';
                        data = { notificationId: ObjectValue.NotificationId, workflowProcess: "HaulierApplication" };
                        div = '#overview_info_section';
                    }

                }
                else {
                    if (ObjectValue.IsSoOverView == 'True') {
                        type = "POST";
                        url = '../Application/ViewApplicationOverview';
                        data = { appRevisionId: ObjectValue.RevisionId, versionId: ObjectValue.VersionId, workflowProcess: "HaulierApplication" };
                        div = '#overview_info_section';
                    }
                    else {
                        type = "POST";
                        url = '../Application/ApplicationOverview';
                        data = { appRevisionId: ObjectValue.RevisionId, versionId: ObjectValue.VersionId, workflowProcess: "HaulierApplication" };
                        div = '#overview_info_section';
                    }
                }
                break;
            case 6.1:
                if (ObjectValue.IsNotif == 'True') {
                    type = "POST";
                    url = '../Notification/NotificationOverview';
                    data = { notificationId: ObjectValue.NotificationId, workflowProcess: "HaulierApplication" };
                    div = '#overview_info_section';
                }
                else {
                    type = "POST";
                    url = '../Application/ViewApplicationOverview';
                    data = { appRevisionId: ObjectValue.RevisionId, versionId: ObjectValue.VersionId, workflowProcess: "HaulierApplication" };
                    div = '#overview_info_section';
                }
                break;
            default:
                url = '../Movements/SelectVehicle';
                div = '#select_vehicle_section';
                break;
        }
        LoadContentForAjaxCalls(type, url, data, div);
    }
    else {
        if (ObjectValue.isSortApp == "True") {
            LoadContentForAjaxCalls("POST", '../SORTApplication/HaulierDetails', { SORTStatus: "CreateSO" }, '#haulier_details_section');
        }
        else {
            if ($("#IsVSO").val() == 'True') {
                LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { IsVSO: true }, '#select_vehicle_section', 'isVSO');
            }
            else {
                LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', {}, '#select_vehicle_section');
            }
        }
        SelectMenu(2);
    }
}

function LoadContentForAjaxCalls(Type, Url, Params, ResLoadContnr, isVSO) {
    $.ajax({
        type: Type,
        url: Url,
        data: Params,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#haulier_details_section').hide();
            $('#select_vehicle_section').hide();
            $('#vehicle_details_section').hide();
            $('#vehicle_import_section').html('');
            $('#vehicle_import_section').hide();
            $('#movement_type_confirmation').hide();
            $('#select_route_section').html('');
            $('#select_route_section').hide();
            $('#route').html('');
            $('#route').hide();
            $('#route_vehicle_assign_section').hide();
            $('#route_assessment_section').html('');
            $('#route_assessment_section').hide();
            $('#supplimentary_info_section').hide();
            $('#overview_info_section').hide();
            $('#vehicle_edit_section').html('');
            $('#vehicle_Component_edit_section').html('');
            $('#vehicle_Create_section').html('');
            $('#vehicle_edit_section').hide();
            $('#vehicle_Component_edit_section').hide();
            $('#vehicle_Create_section').hide();
            $('#AllocateUser').hide();
            $("#allocate_btn").hide();
            $('#select_route_section_agredRt').hide();
            $(ResLoadContnr).show();
            $(ResLoadContnr).html(response);
            if (isVSO == 'isVSO') {
                setTimeout(function () {
                    notifiedSOAPolice();
                }, 1000);
            }
        },
        error: function (result) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function BackToPrevMovm() {
    LoadContentForAjaxCalls("POST", '../SORTApplication/GetPreviousMovemntList', { projectId: PrePrjId, isVehicleImport: $("#IsVehicle").val(), movmntType: PreMovType }, '#select_route_section');
    $('#back_btn_Rt_prv').hide();
    $('#back_btn').show();
}

function OnBackButtonClick() {
    $(".btn-primary").blur();
    var url = '../Movements/OnBackButtonClick';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        data: { stepFlag: StepFlag, subStepFlag: SubStepFlag },
        beforeSend: function () {
        },
        success: function (page) {

        },
        complete: function () {
        }
    });
    if (SubStepFlag != 5.1) {
        HideUnloadSections();
    }

    if (($('#MvmtConfirmFlag').val() == 'true') && (StepFlag == 1 || StepFlag == 2 || StepFlag == 3))
        RemoveNavForWrkflwSequence();   //remove work flow sequence nav if the user comes back to the vehicle page to edit

    switch (StepFlag) {
        case 0:
            if (SubStepFlag == 0.1) {
                $('#haulier_details_section').show();
                SubStepFlag = 0;
            }
            else if (SubStepFlag == 0.2 || SubStepFlag == 0.3) {
                BackToCreateHaulier();
                $('#haulier_details_section').show();
                SubStepFlag = 0;
            }
            else {
                window.location = '../Home/SORT';
            }
            break;
        case 1:
            var previousList = "false";
            if ($('#BackToPreviousList').val() != undefined) {
                previousList = $('#BackToPreviousList').val().toLowerCase();
            }
            if (SubStepFlag == 1.1) {
                var importFrom = $('#ImportFrom').val();
                if ((BackToPreviousMovementList == true || previousList == "true") && (importFrom == 'prevMov')) {
                    ImportVehicle('prevMov', 'true');
                    BackToPreviousMovementList = false;
                    break;
                }
                else if (SelectedVehicles.length > 0) {
                    StepFlag = 2;
                    SubStepFlag = 0;
                    CurrentStep = "Vehicle Details";
                    $('#current_step').text(CurrentStep);
                    $('#vehicle_details_section').show();
                    $('#confirm_btn').show();
                    $("#back_btn").prop('disabled', true);
                }
                else {
                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section');
                }
            }
            else if (SubStepFlag == 1.4) {
                $('#vehicle_import_section').html('');
                $('#select_vehicle_section').show();
                SubStepFlag = 1.1;
            }
            //Navigate back from Select Vehicle page
            else if (SubStepFlag == 2.3) {
                SelectVehicleFromFleet();
                SubStepFlag = 2;
            }
            else if (SubStepFlag == 2.4) {
                SelectVehiclecomponentFromFleet();
                SubStepFlag = 2.1;
            }
            else if (SubStepFlag == 2.2) {
                OnBackBtnClick();
                SubStepFlag = 2.1;
            }
            else if (SelectedVehicles.length > 0) {
                if (SubStepFlag == 2.1) {
                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section');
                }
                else {
                    StepFlag = 2;
                    CurrentStep = "Vehicle Details";
                    $('#current_step').text(CurrentStep);
                    $('#vehicle_details_section').show();
                    $('#confirm_btn').show();
                    $("#back_btn").prop('disabled', true);
                }
            }
            else if (SubStepFlag == 2.1) {
                if (SelectedVehicles.length == 0) {
                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section');
                }
                else {
                    OnBackBtnClick();
                }
            }
            else if (SubStepFlag != 0) {
                var previousList = "false";
                if ($('#BackToPreviousList').val() != undefined) {
                    previousList = $('#BackToPreviousList').val().toLowerCase();
                }
                var importFrom = $('#ImportFrom').val();
                if ((BackToPreviousMovementList == true || previousList == "true") && (importFrom == 'prevMov')) {
                    ImportVehicle('prevMov', 'true');
                    BackToPreviousMovementList = false;
                }
                else {
                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section');
                }
            }
            else if (ObjectValue.userTypeID == 696008) {
                StepFlag = 0;
                CurrentStep = "Haulier Details";
                $('#current_step').text(CurrentStep);
                $('#haulier_details_section').show();
                $('#confirm_btn').removeClass('blur-button');
                $('#confirm_btn').attr('disabled', false);
            }
            else {
                window.location = '../Home/Hauliers';
            }
            break;
        case 2: //Navigate back from vehicle details page
            if (SubStepFlag == 2.1) {
                if (SelectedVehicles.length > 0) {
                    StepFlag = 2;
                    SubStepFlag = 0;
                    CurrentStep = "Vehicle Details";
                    $('#current_step').text(CurrentStep);
                    //$('#vehicle_details_section').show();
                    LoadContentForAjaxCalls("POST", '../Movements/MovementSelectedVehicles', { isBackCall: true, movementId: $('#MovementId').val() }, '#vehicle_details_section');
                    $('#confirm_btn').show();
                }
                else {
                    OnBackBtnClick();
                }
            }
            else if (SubStepFlag == 2.2) {
                OnBackBtnClick();
                SubStepFlag = 2.1;
            }
            else if (SubStepFlag == 2.5) {
                OnBackEditBtnClick();
                SubStepFlag = 2.4;
            }
            else if (SubStepFlag == 0 || SubStepFlag == 1.1) {
                if (SubStepFlag == 1.1) {
                    var previousList = "false";
                    var importFrom = $('#ImportFrom').val();
                    if ((BackToPreviousMovementList == true || previousList == "true") && (importFrom == 'prevMov')) {
                        ImportVehicle('prevMov', 'true');
                        BackToPreviousMovementList = false;
                        break;
                    }
                    else if (SelectedVehicles.length > 0) {
                        StepFlag = 2;
                        SubStepFlag = 0;
                        CurrentStep = "Vehicle Details";
                        $('#current_step').text(CurrentStep);
                        $('#vehicle_details_section').show();
                        $('#confirm_btn').show();
                    }
                    else {
                        LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section');
                    }
                }
                else {
                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section');
                }
            }
            else if (SelectedVehicles.length > 0) {
                StepFlag = 2;
                SubStepFlag = 0;
                CurrentStep = "Vehicle Details";
                $('#current_step').text(CurrentStep);
                $('#vehicle_details_section').show();
                $('#confirm_btn').show();
                $("#back_btn").prop('disabled', true);
            }
            break;
        case 3: //Navigate back from mvnt confirmtn page
            StepFlag = 2;
            CurrentStep = "Vehicle Details";
            $('#current_step').text(CurrentStep);
            //$('#vehicle_details_section').show();
            LoadContentForAjaxCalls("POST", '../Movements/MovementSelectedVehicles', { isBackCall: true, movementId: $('#MovementId').val() }, '#vehicle_details_section');

            $('#save_btn').hide();
            $('#apply_btn').hide();
            $('#confirm_btn').removeClass('blur-button');
            $('#confirm_btn').attr('disabled', false);
            $('#confirm_btn').show();
            break;
        case 4: //Navigate back from Route page
            var previousList = "false";
            if ($('#BackToRoutePreviousList').val() != undefined) {
                previousList = $('#BackToRoutePreviousList').val().toLowerCase();
            }
            if (SubStepFlag == 0) {
                $('#select_route_section').html('');
                LoadContentForAjaxCalls("POST", '../Movements/GetMovementTypeConfirmation', { appRevisionId: $('#AppRevisionId').val(), appVersionId: $('#AppVersionId').val(), notificationId: $('#NotificationId').val(), isBackCall: true }, '#movement_type_confirmation');
            }
            else if (SubStepFlag == 4.1) {
                var importFrom = $('#ImportFrom').val();
                if ((BackToPreviousRouteMovementList == true || previousList == "true") && (importFrom == 'prevMov')) {
                    SelectRouteFromPrevMovements(undefined, 'true');
                    BackToPreviousRouteMovementList = false;
                    break;
                }
                else {
                    $('#select_route_section').html('');
                    LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication' }, '#select_route_section');
                }
            }
            else if (SubStepFlag == 4.2) {
                if (ObjectValue.isSortApp == "True") {
                    $('#select_route_section').show();
                    $('#back_btn_Rt_prv').show();
                    $('#back_btn').hide();
                }
                else {
                    $('#select_route_section').show();
                }
                SubStepFlag = 4.1;
            }
            else if (SubStepFlag == 4.9) {
                StepFlag = 5;
                var IsFavouriteRoute = $('#IsFavouriteRoute').val();
                ImportFromLibrary(IsFavouriteRoute,);
            }
            else {
                $('#select_route_section').html('');
                LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication' }, '#select_route_section');
            }
            break;
        case 5: //Navigate back from supplimentary info page
            StepFlag = 4;
            if (SubStepFlag == 5.1) {
                StepFlag = 5;
                SubStepFlag = 0;
                BacktoAffectedParties();
            }
            else {
                CurrentStep = "Route Details";
                $('#select_route_section').html('');
                LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication' }, '#select_route_section');
            }
            break;
        case 6: //Navigate back from overview page to Supplimetary View / Save for SO/VR1 or to Route Assessment for Notification
            StepFlag = 5;

            if ($('#IsNotif').val().toLowerCase() == 'true') {
                /* if ('@planMovePayLoad.IsNotif' == 'True' || '@planMovePayLoad.IsNotif' == 'true') {*/

                LoadContentForAjaxCalls("POST", '../Notification/NotificationRouteAssessment', { workflowProcess: 'HaulierApplication' }, '#route_assessment_section');
            }
            else {
                CurrentStep = "Supplimentary Information";
                if (ObjectValue.IsSupplimentarySaved == 'True') {
                    LoadContentForAjaxCalls("POST", '../Application/ViewSupplementary', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section');
                    $('#back_btn').show();
                    $('#confirm_btn').show();
                    $('#save_btn').hide();
                    $('#apply_btn').hide();
                }
                else {
                    LoadContentForAjaxCalls("POST", '../Application/ApplicationSupplimentaryInfo', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section');
                }
            }
            break;
    }
}

function OnConfirm() {

    $(".btn-primary").blur();

    switch (StepFlag) {
        case 0: //On confirm from haulier details page
            if (SubStepFlag == 0.2) {
                if ($('#AppRevisionId').val() > 0) {
                    // LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', {}, '#select_vehicle_section');
                    StepFlag = 2;
                    CurrentStep = "Vehicle Details";
                    $('#current_step').text(CurrentStep);
                    //$('#vehicle_details_section').show();
                    LoadContentForAjaxCalls("POST", '../Movements/MovementSelectedVehicles', { movementId: $('#MovementId').val() }, '#vehicle_details_section');
                    $('#save_btn').hide();
                    $('#apply_btn').hide();
                    $('#confirm_btn').removeClass('blur-button');
                    $('#confirm_btn').attr('disabled', false);
                    $('#confirm_btn').show();
                    $('#back_btn').show();
                } else {
                    ValidateHaulierDetails();
                }
            }
            else {
                LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', {}, '#select_vehicle_section');
            }
            break;
        case 1: //On confirm from select vehicle page
            StepFlag = 2;
            CurrentStep = "Vehicle Details";
            $('#current_step').text(CurrentStep);
            $('#select_vehicle_section').hide();
            $('#vehicle_details_section').show();
            $('#save_btn').hide();
            $('#apply_btn').hide();
            $('#confirm_btn').removeClass('blur-button');
            $('#confirm_btn').attr('disabled', false);
            $('#confirm_btn').show();
            break;
        case 2: //On confirm from vehicle details page
            if ($('#IsAddVehicleError').val() == 6) {
                ShowErrorPopup("Construction and use vehicle. Does not need a notification.", 'CloseErrorPopup');
            }
            else if ($('#IsAddVehicleError').val() == 2) {
                ShowErrorPopup("The vehicles in this movement are in different category. So we are unable to proceed further. Please edit the vehicle and proceed.", 'CloseErrorPopup');
            }
            else if ($('#IsAddVehicleError').val() == 3) {
                ShowErrorPopup("Axle weight is more than gross weight for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');
            }
            else if ($('#IsAddVehicleError').val() == 4) {
                ShowErrorPopup("Total axle distance exceeds vehicle length for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');
            }
            else if (ObjectValue.MovementClassId == 207004 || $('#IsAddVehicleError').val() == 7) {
                ShowErrorPopup("Based on the details you’ve filled in system couldn't identified the movement. Please edit the vehicle dimensions to match with an application.", 'CloseErrorPopup');
            }
            else if ($('#IsAddVehicleError').val() != 5) {

                var isBckCall = false;
                var appRevId = $('#AppRevisionId').val();
                var notifId = $('#NotificationId').val()
                if (appRevId > 0 || notifId > 0) {
                    isBckCall = true;
                }
                LoadContentForAjaxCalls("POST", '../Movements/GetMovementTypeConfirmation', { isBackCall: isBckCall }, '#movement_type_confirmation');
            }
            else {
                ShowErrorPopup("Based on the vehicle details you entered, system assess the movement as a Notification and thus cannot proceed further. Please edit the vehicle dimensions to match with an application.", 'CloseErrorPopup');

            }
            break;
        case 3: //On confirm from mvmnt confirmtn page'

            var radioValue = $("input[name='applicationType']:checked").val();
            if (radioValue == "207003") {
                AmendFlag = GetVehicleRegistration();
            }
            //if (AmendFlag >= 1) {
            if ($('#NotificationId').val() != 0 || $('#AppRevisionId').val() != 0) {
                if (MovementTypeValidation()) {
                    UpdateMovementType();
                }
            }
            else
                if (MovementTypeValidation()) {
                    InsertMovementType();
                }
            //}
            if (AmendFlag == 0) {
                ShowInfoPopup('The vehicle registration details are missing for the vehicle. Go to vehicle details page and click on amend vehicle button to enter the mandatory registration details.');
            }
            break;
        case 4: //On confirm from route details page

            if (SubStepFlag != 4.3) {
                var isVehicleAutoAssignedFlag = $('#IsVehicleAutoAssigned_Flag').val();
                var isReturnRouteAvailableFlag = $('#IsReturnRouteAvailable_Flag').val();

                if (($('#IsAgreedNotify').val() == 'true' || $('#IsAgreedNotify').val() == 'True') && (vehicleIdArr.length > 1 && routeIdArr.length > 1 && returnRouteIdArr.length > 1)) {
                    //$("#IsRouteAssessment").val(true);
                    //$('#select_route_section_agredRt').hide();//hiding agreed route div
                    //LoadContentForAjaxCalls("POST", '../Notification/NotificationRouteAssessment', { workflowProcess: 'HaulierApplication' }, '#route_assessment_section');
                    LoadContentForAjaxCalls("POST", '../Movements/GetVehicleAssignmentList', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), workflowProcess: 'HaulierApplication' }, '#route_vehicle_assign_section');
                }
                else {
                    if ((isReturnRouteAvailableFlag == 'true' || isReturnRouteAvailableFlag == 'True') && (isVehicleAutoAssignedFlag == 'false' || isVehicleAutoAssignedFlag == 'False') && routeIdArr.length == 2 && returnRouteIdArr.length == 1 && vehicleIdArr.length == 1) //(RouteVehicleAssignFlag )
                    {
                        AutoAssignVehiclesToRouteAndReturnRoute();
                    }
                    else if (!RouteVehicleAssignFlag && returnRouteIdArr.length <= 0) {//&& (isReturnRouteAvailableFlag == 'false' || isReturnRouteAvailableFlag == 'False') && (isVehicleAutoAssignedFlag == 'true' || isVehicleAutoAssignedFlag == 'True')
                        AutoAssignVehiclesToRoute();
                    }
                    else { // if (RouteVehicleAssignFlag) && returnRouteIdArr.length <= 0//&& (isReturnRouteAvailableFlag == 'false' || isReturnRouteAvailableFlag == 'False') && (isVehicleAutoAssignedFlag == 'true' || isVehicleAutoAssignedFlag == 'True') )
                        LoadContentForAjaxCalls("POST", '../Movements/GetVehicleAssignmentList', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), workflowProcess: 'HaulierApplication' }, '#route_vehicle_assign_section');
                    }
                }
            }
            else {
                AssignVehiclesToRoute();  //On confirm from route assignment page
            }

            break;
        case 5:  //On confirm from supplimentary/route assessment page
            if ($('#IsNotif').val() == 'true' || $('#IsNotif').val() == 'True') {
                IsRouteAnalysisComplete();
            }
            else {
                if ($('#OverviewInfoSaveFlag').val() == 'true') {
                    $('#save_btn').hide();
                    $('#apply_btn').show();
                    LoadContentForAjaxCalls("POST", '../Application/ViewApplicationOverview', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val() }, '#overview_info_section');
                }
                else {


                    LoadContentForAjaxCalls("POST", '../Application/ApplicationOverview', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), workflowProcess: "HaulierApplication" }, '#overview_info_section');
                }
            }
            break;
    }
}

function NavigationForWrkflwSequence(NavStep) {
    HideUnloadSections();
    if (($('#MvmtConfirmFlag').val() == 'true') && (NavStep == 0 || NavStep == 1 || NavStep == 2))
        RemoveNavForWrkflwSequence();   //remove work flow sequence nav if the user comes back to the vehicle page to edit
    switch (NavStep) {
        case 0:  //Haulier details
            StepFlag = 0;
            CurrentStep = "Haulier Details";
            $('#current_step').text(CurrentStep);
            var appRevisionId = $('#AppRevisionId').val();
            if (appRevisionId > 0) {
                ViewHaulierDetails();
            }
            else {
                $('#haulier_details_section').show();
            }
            $('#back_btn').hide();
            $('#confirm_btn').removeClass('blur-button');
            $('#confirm_btn').attr('disabled', false);
            $('#confirm_btn').show();
            break;
        case 1:  //Select Vehicle
        //$('#vehicle_details_section').show();
        //$('#overview_info_section').html('');
        //LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', {contactId: $("#SOHndContactID").val() }, '#select_vehicle_section');
        // break;
        case 2:  //Vehicle Details
            StepFlag = 2;
            CurrentStep = "Vehicle Details";
            $('#current_step').text(CurrentStep);
            //$('#vehicle_details_section').show();
            LoadContentForAjaxCalls("POST", '../Movements/MovementSelectedVehicles', { isBackCall: true, movementId: $('#MovementId').val() }, '#vehicle_details_section');
            $('#save_btn').hide();
            $('#apply_btn').hide();
            $('#confirm_btn').removeClass('blur-button');
            $('#confirm_btn').attr('disabled', false);
            $('#confirm_btn').show();
            break;
        case 3:  //Movement Type
            LoadContentForAjaxCalls("POST", '../Movements/GetMovementTypeConfirmation', { isBackCall: true }, '#movement_type_confirmation');
            break;
        case 4:  //Route Details
            LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication' }, '#select_route_section');
            break;
        case 5:  //Supplimentary info/Route Assessmnt
            StepFlag = 5;
            if ($('#IsNotif').val() == 'true') {
                $("#IsRouteAssessment").val(true);
                LoadContentForAjaxCalls("POST", '../Notification/NotificationRouteAssessment', { workflowProcess: 'HaulierApplication' }, '#route_assessment_section');
            }
            else {
                CurrentStep = "Supplimentary Information";
                if (ObjectValue.IsSupplimentarySaved == 'True') {

                    LoadContentForAjaxCalls("POST", '../Application/ViewSupplementary', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section');
                    $('#back_btn').show();
                    $('#confirm_btn').show();
                    $('#save_btn').hide();
                    $('#apply_btn').hide();
                }
                else {

                    LoadContentForAjaxCalls("POST", '../Application/ApplicationSupplimentaryInfo', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section');
                }
            }
            break;
    }
}

function OnSave() {

    switch (StepFlag) {
        case 0:  //Save from Haulier details page
            SaveOrganisationData();
            break;
        case 5:  //Save from supplimentary info page
            SaveSupplimentaryInfo();
            break;
        case 6:  //Save from overview page
            if ($('#IsNotif').val() == 'true') {
                ValidateGeneralDetails();
            }
            else {
                OverViewValidation();
            }
            break;
    }
}

function onApply() {
    if ($('#IsNotif').val() == 'true') {
        ValidateGeneralDetails();
    }
    else {
        OverViewValidation();
    }
}

function RedirectToMovementInbox() {
    if (ObjectValue.userTypeID == 696008) {
        window.location.href = '/SORTApplication/SORTInbox';
    }
    else {
        window.location.href = '/Movements/MovementList';
    }
}

function RedirectToNotificationOverview(notificationId, notificationCode) {
    window.location.href = '/Notification/DisplayNotification?notificationId=' + notificationId + '&notificationCode=' + notificationCode;
}

function NavigateToEditSupplementary() {
    var url = '../Application/ApplicationSupplimentaryInfo';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        data: { appRevisionId: $('#AppRevisionId').val(), isClone: $('#IsClone').val(), isRevise: $('#IsRevise').val() },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#viewsupplimentary').show();
            $('#viewsupplimentary').html('');
            $('#viewsupplimentary').html(response);
            $('#btnEditSupplementary').hide();
            $('#btnSaveSupplementary').show();
        },
        error: function (result) {
            location.readload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function NavigateToSupplConfirm() {
    var isClone = $('#IsClone').val();
    var isRevise = $('#IsRevise').val();
    if (isClone == 1 || isRevise == 1) {
        ViewSupplementary();
    }
    else {
        $('#save_btn').hide();
        $('#confirm_btn').show();
        CloseSuccessModalPopup();
        LoadContentForAjaxCalls("POST", '../Application/ViewSupplementary', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section');
    }
}

function NavigateToViewSupplementary() {
    var url = '../Application/ViewSupplimentaryApplication';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        data: { appRevisionId: $('#AppRevisionId').val(), isClone: $('#IsClone').val(), isRevise: $('#IsRevise').val() },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            // console.log('response' + response);
            $('#viewsupplimentary').show();
            $('#viewsupplimentary').html('');
            $('#viewsupplimentary').html(response);
            $('#btnEditSupplementary').show();
            $('#btnSaveSupplementary').hide();
        },
        error: function (result) {
            location.readload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function ViewSupplementary() {
    var url = '../Application/ViewSupplementary';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        data: { appRevisionId: $('#AppRevisionId').val(), isClone: $('#IsClone').val(), isRevise: $('#IsRevise').val() },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            // console.log('response' + response);
            $('#viewsupplimentary').show();
            $('#viewsupplimentary').html('');
            $('#viewsupplimentary').html(response);
            $('#btnEditSupplementary').show();
            $('#btnSaveSupplementary').hide();
        },
        error: function (result) {
            location.readload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function HideUnloadSections() {
    $('#haulier_details_section').hide();
    $('#select_vehicle_section').hide();
    $('#vehicle_details_section').hide();
    $('#movement_type_confirmation').hide();
    $('#select_route_section').hide();
    $('#route').html('');
    $('#route').hide();
    $('#route_vehicle_assign_section').html('');
    $('#route_vehicle_assign_section').hide();
    $('#route_assessment_section').html('');
    $('#route_assessment_section').hide();
    $('#supplimentary_info_section').hide();
    $('#overview_info_section').hide();
    $('#vehicle_edit_section').html('');
    $('#vehicle_edit_section').hide();
    $('#vehicle_Component_edit_section').hide();
    $('#vehicle_Create_section').hide();
    $('#allocate').hide();
    $('#allocate_btn').hide();
    $('#back_btn_Rt_prv').hide();
    $('#select_route_section_agredRt').hide();
}

//function PlanMovementDirectRedirect(NavStep) {
//    var url = '../Movements/PlanMovementDirectRedirect';
//    $.ajax({
//        url: url,
//        type: 'POST',
//        cache: false,
//        data: { stepFlag: NavStep },
//        beforeSend: function () {
//        },
//        success: function (page) {
//        },
//        complete: function () {
//        }
//    });
//}



function RemoveNavForWrkflwSequence() {
    for (let index = 3; index < 6; index++) {
        $('#step' + index + ' p').css('cursor', '');
        $('#step' + index + ' p').css('text-decoration', '');
        $('#step' + index + ' p').removeAttr("onclick");
    }
}


function GetVehicleRegistration() {
    var Count = 0;
    var url = '../VehicleConfig/GetVehicleRegistration';
    $.ajax({
        url: url,
        type: 'GET',
        async: false,
        data: { vehicleIds: SelectedVehicles },
        traditional: true,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            Count = response.Count;
        },
        error: function (response) {
            console.log(response);
        },
        complete: function () {
            stopAnimation();
        }
    });
    return Count;
}
