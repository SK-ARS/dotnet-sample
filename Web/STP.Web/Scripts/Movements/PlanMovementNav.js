let BackNavigateUrl;
let StepFlag = 0;
let SubStepFlag = 0;
let CurrentStep = "";
let SelectedVehicles = [];
let BackToPreviousMovementList = false;
let BackToPreviousRouteMovementList = false;
let AmendFlag = 1;
var RevisionIdVal = $("#hf_RevisionId").val();
var NotificationIdVal = $("#hf_NotificationId").val();
var VehicleMoveIdVal = $("#hf_VehicleMoveId").val();
var VersionIdVal = $("#hf_VersionId").val();
var ContenRefNoVal = $("#hf_ContenRefNo").val();
var AnalysisIdVal = $("#hf_AnalysisId").val();
var IsRouteAssessmentDoneVal = $("#hf_IsRouteAssessmentDone").val();
var IsSortAppVal = $("#hf_IsSortApp").val();
var IsReviseVal = $("#hf_IsRevise").val();
var IsAppVal = $("#hf_IsApp").val();
var IsVr1AppVal = $("#hf_IsVr1App").val();
var NextActionVal = $("#hf_NextAction").val();
var IsNotifVal = $("#hf_IsNotif").val();
var IsNotifGeneralSavedVal = $("#hf_IsNotifGeneralSaved").val();
var IsSupplimentarySavedVal = $("#hf_IsSupplimentarySaved").val();
var IsSoOverViewVal = $("#hf_IsSoOverView").val();
var userTypeIDVal = $("#hf_userTypeID").val();
var isSortAppVal = $("#hf_isSortApp").val();
var isNotificationVal = $("#hf_isNotification").val();
var MovementClassIdVal = $("#hf_MovementClassId").val();
var IsRoutePlanned = false;
var AutoAssignRouteVehicle = false;
var ContinueRouteAssessment = false;
var VehicleAssignedList = [];
var VehicleAssigned = false;
var AddNewRoute = 0;
var isWorkFlowCompleted = false;
var btnContinueToRouteAssessmentClicked = false;
var hf_Vr1SoExistingPopUp = false;
var isRouteAssessmentRequiredForWIP = false;

$(document).ready(function () {
    $('#AppRevisionId').val(RevisionIdVal);
    $('#NotificationId').val(NotificationIdVal);
    $('#MovementId').val(VehicleMoveIdVal);
    if (IsRouteAssessmentDoneVal == "True" || IsRouteAssessmentDoneVal == 'true') { $('#IsRouteAssessment').val(true); }

    if (NotificationIdVal > 0) {
        $('#NotifVersionId').val(VersionIdVal);
        $("#IsNotif").val(true);
        $('#CRNo').val(ContenRefNoVal);
        $('#NotifVersionId').val(VersionIdVal);
        $('#NotifAnalysisId').val(AnalysisIdVal);
    }
    else {
        $('#AppVersionId').val(VersionIdVal);
        $("#IsNotif").val(false);
    }

    var workInProgressNotifId = localStorage.getItem("WorkInProgressNotifId") || 0;
    if (workInProgressNotifId > 0 && workInProgressNotifId == NotificationIdVal) {
        //Check action completed step is overview and set RouteAssessment is required flag
        var actionCompletedVal = $("#hf_ActionCompleted").val() || 0;
        if (actionCompletedVal == NavigationEnum.OVERVIEW) {
            isRouteAssessmentRequiredForWIP = true;
        }
    }
    NavigateURL();

    $('body').on('click', '#back_btn_Altered,#back_btn,#btn_back_to_movement_type_confirm', function (e) {
        e.preventDefault();
        PlanMoveNavigateFlow(StepFlag, true);
    });
    $('body').on('click', '#backBtnExistingSOVr1', function (e) {
        e.preventDefault();
        OnBackExistingSOVr1ToMovementType();
    });
    $('body').on('click', '#back_btn_Rt_prv', function (e) {
        e.preventDefault();
        BackToPrevMovm(this);
    });
    $('body').on('click', '#save_btn', function (e) {
        e.preventDefault();
        OnSave(this);
    });
    $('body').on('click', '#allocate_btn', function (e) {
        e.preventDefault();
        AllocatePOP(this);
    });
    $('body').on('click', '#confirm_btn,#assignVehicleToRoute', function (e) {
        e.preventDefault();
        OnConfirm(this);
    });
    $('body').on('click', '#apply_btn', function (e) {
        e.preventDefault();
        if ($("#IsSortRevise").val() == 'true') {
            ShowWarningPopup("Do you want to submit?", "onApply", "CloseWarningPopupRef");
        }
        else {
            CheckBrokenRouteExistBeforeSubmit(function () {
                onApply();
            });
        }
    });
    $('body').on('click', '#btn_continue_to_route_summary', function (e) {
        e.preventDefault();
        isVehicleChanged = false;//This will be used for top navigation validation
        var isUnsavedChange = ValidateUnSavedChange("map") || CheckMapInputHasChanges();
        if (isUnsavedChange) {
            ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "RedirectToRouteListNew", "CloseWarningPopupRef");
        }
        else {
            RedirectToRouteListNew();
        }
    });
    
    $('body').on('click', '#btn_continue_to_route_assessment ', function (e) {
        e.preventDefault();
        isVehicleChanged = false;//This will be used for top navigation validation
        var isUnsavedChange = ValidateUnSavedChange("map") || CheckMapInputHasChanges();
        if (isUnsavedChange) {
            ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "ConfirmFromMapPage", "CloseWarningPopupRef");
        }
        else {
            btnContinueToRouteAssessmentClicked = true;
            ConfirmFromMapPage();
        }
    });
    $('body').on('click', '#btnSelectExistingVR1SO ', function (e) {
        $('#Vr1SoExistingPopUp').modal('hide');
        $('#hf_Vr1SoExistingPopUp').val('true');
        hf_Vr1SoExistingPopUp = true;
        ImportVehicle('prevMov', 'false', true);
        CreateLocalObject(StepFlag);
        $('#back_btn').show();
    });
    $('body').on('click', '#Vr1SoExistingClosePopUp ', function (e) {
        e.preventDefault();
        $('#Vr1SoExistingPopUp').modal('hide');
    });
    $('body').on('click', '#btnContinueNotif ', function (e) {
        $('#Vr1SoExistingPopUp').modal('hide');
        RemoveLocalObject(StepFlag);
        InsertMovementType();
    });
    $('body').on('click', '#planRouteonMap', function (e) {
        e.preventDefault();
        AddNewRoute = 1;
        openContentLoader('html');
        LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', {
            apprevisionId: RevisionIdVal, versionId: VersionIdVal, contRefNum: ContenRefNoVal, isNotif: IsNotifVal, workflowProcess: "HaulierApplication",
            IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), IsRouteSummaryPage: 0 }, '#select_route_section', '', function () {
            MovementRouteInit(false);
        });
    });

    var oldConfigVal = "";
    $('body').off('focus', '.flowComponent #config_create_section input[type=text],.flowComponent #config_create_section textarea');
    $('body').on('focus', '.flowComponent #config_create_section input[type=text],.flowComponent #config_create_section textarea', function (e) {
        oldConfigVal = "";
        oldConfigVal = $(this).val();
});
    
    $('body').off('blur', '.flowComponent #config_create_section input[type=text],.flowComponent #config_create_section textarea');
    $('body').on('blur', '.flowComponent #config_create_section input[type=text],.flowComponent #config_create_section textarea', function (e) {
        var currentVal = $(this).val();
        if (currentVal != oldConfigVal && $(this).is(':visible')) {
            isVehicleHasChanged = true;
        }
    });

    //When user click top nav links, we need to display warning popup
    $('#navbar a,button.btn-layout-logout').click(function (e) {
        var ignoreCheck = $(e.target).hasClass('ignore-unsaved-changes');
        if (!ignoreCheck) {
            var href = $(this).attr('href');
            var isLogout = $(this).hasClass('btn-layout-logout');
            var isDropdown = $(this).hasClass('dropdown-toggle');
            if (BackAndForthNavMethods.IsWorkFlowStarted() == false && isWorkFlowCompleted == false && href != "#" && !isDropdown) {
                e.preventDefault();
                if (isSortAppVal == "True") {
                    var message = "<b>Your Movement Has Not Been Saved</b></br> Are you sure you want to leave this page?</br></br>To save your movement select 'No' and continue to<br/>Step 4 - Movement Type. Enter all details on the page</br> and select Confirm and Continue";
                    ShowWarningPopup(message, "LeavePage", "CloseWarningPopupRef", href, isLogout, this);
                }
                else {
                    var message = "<b>Your Movement Has Not Been Saved</b></br> Are you sure you want to leave this page?</br></br>To save your movement select 'No' and continue to<br/>Step 3 - Movement Type. Enter all details on the page</br> and select Confirm and Continue";
                    ShowWarningPopup(message, "LeavePage", "CloseWarningPopupRef", href, isLogout, this);
                }
            }
            else if (BackAndForthNavMethods.IsWorkFlowStarted() == true && isWorkFlowCompleted == false && href != "#" && !isDropdown &&
                (StepFlag == NavigationEnum.VEHICLE_DETAILS || StepFlag == NavigationEnum.MOVEMENT_TYPE || StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY || StepFlag == NavigationEnum.OVERVIEW || StepFlag == NavigationEnum.ROUTE_DETAILS)) {
                e.preventDefault();
                if ((href == "" || href == undefined || href == null) && isLogout) {
                    href = '../Account/LogOut';
                }
                CheckSaveAndLeavePageOnHeaderLinkClick(href, isLogout, this);
            } else if (isLogout) {
                location.href = '../Account/LogOut';
            }
        }
    });

    $('body').on('click', 'input[name="applicationType"]', function (e) {
        if (typeof showImminentMovementBanner != 'undefined') {
            showImminentMovementBanner($('#FromDate').val(), '#movement_type_confirmation ');
        }
    });
});
function OnBackExistingSOVr1ToMovementType() {
    $('#importlist_cntr').hide();
    $('#movement_type_confirmation').show();
    $('#confirm_btn').show();
}
function NavigateURL() {
    startAnimation();
    if (IsSortAppVal == "True") {
        if (IsReviseVal == "True") {
            $('#IsSortRevise').val(true);
        }
    }
    var type = "POST";
    var url = '';
    var data = {};
    var div;
    var initMethods = null;
    HideUnloadSections();
    $('#plan_movement_hdng').text("PLAN MOVEMENT");
    if (IsAppVal == "True") {
        if (IsVr1AppVal == 'False') {
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
    else if (NotificationIdVal > 0) {
        $('#plan_movement_hdng').text("NOTIFICATION");
        $('#apply_btn').text('SUBMIT NOTIFICATION');
    }
    NextActionVal = parseInt(NextActionVal);
    IsRoutePlanned = false;
    if (NextActionVal != 0) {
        SelectMenu(2);
        switch (NextActionVal) {
            case 0:
                if (isSortAppVal == "True") {
                    url = '../SORTApplication/HaulierDetails';
                    data = { SORTStatus: "CreateSO" };
                    div = '#haulier_details_section';
                    initMethods = function () {
                        CreateOrganisationInit();
                    };
                }
                else {
                    url = '../Movements/SelectVehicle';
                    div = '#select_vehicle_section';
                    initMethods = function () {
                        MovementSelectVehicleInit();
                    };
                }
                break;
            case 1:
                url = '../Movements/SelectVehicle';
                div = '#select_vehicle_section';
                initMethods = function () {
                    MovementSelectVehicleInit();
                };
                break;
            case 2:
                url = '../Movements/MovementSelectedVehicles';
                data = { movementId: VehicleMoveIdVal };
                div = '#vehicle_details_section';
                initMethods = function () {
                    MovementSelectedVehiclesInit();
                    VehicleDetailsInit();
                    ViewConfigurationGeneralInit();
                    GeneralVehicCompInit();
                    MovementAssessDetailsInit();
                };
                break;
            case 3:
                url = '../Movements/GetMovementTypeConfirmation';
                data = { apprevisionId: RevisionIdVal, appVersionId: VersionIdVal, notificationId: NotificationIdVal };
                div = '#movement_type_confirmation';
                initMethods = function () {
                    MovementTypeConfirmationInit();
                };
                break;
            case 4:
                url = '../Routes/MovementRoute';
                data = {
                    apprevisionId: RevisionIdVal, versionId: VersionIdVal, contRefNum: ContenRefNoVal, isNotif: IsNotifVal, workflowProcess: "HaulierApplication",
                    IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), IsRouteSummaryPage: $('#IsRouteSummary_Flag').val()
                };
                initMethods = function () {
                    var IsRouteSummary = false;
                    if ($('#IsRouteSummary_Flag').val() == "1") {
                        IsRouteSummary = true;
                    }
                    MovementRouteInit(IsRouteSummary);
                };
                div = '#select_route_section';
                break;
            case 5:
                if (isNotificationVal == 'True') {
                    url = '../Notification/NotificationRouteAssessment';
                    data = { workflowProcess: 'HaulierApplication' };
                    div = '#route_assessment_section';
                    initMethods = function () {
                        NotificationRouteAssessmentInit();
                    };
                }
                else {
                    if (IsSupplimentarySavedVal == 'True') {
                        type = "POST";
                        url = '../Application/ViewSupplementary';
                        data = { appRevisionId: RevisionIdVal };
                        div = '#supplimentary_info_section';
                        initMethods = function () {
                            ViewSupplementaryInit();
                        };
                    }
                    else {
                        type = "POST";
                        url = '../Application/ApplicationSupplimentaryInfo';
                        data = { appRevisionId: RevisionIdVal };
                        div = '#supplimentary_info_section';
                        initMethods = function () {
                            ApplicationSupplimentaryInfoInit();
                        };
                    }
                }
                break;
            case 5.1:
                if (isNotificationVal == 'True') {
                    url = '../Notification/NotificationRouteAssessment';
                    data = { workflowProcess: 'HaulierApplication' };
                    initMethods = function () {
                        NotificationRouteAssessmentInit();
                    };
                    div = '#route_assessment_section';
                }
                else {
                    type = "POST";
                    url = '../Application/ViewSupplementary';
                    data = { appRevisionId: RevisionIdVal };
                    div = '#supplimentary_info_section';
                    initMethods = function () {
                        ViewSupplementaryInit();
                    };
                }
                break;
            case 6:
                if (IsNotifVal == 'True') {
                    type = "POST";
                    url = '../Notification/SetNotificationGeneralDetails';
                    data = { notificationId: NotificationIdVal, workflowProcess: "HaulierApplication" };
                    div = '#overview_info_section';
                    initMethods = function () {
                        NotificationGeneralDetailsInit();
                    };
                }
                else {
                    type = "POST";
                    url = '../Application/ApplicationOverview';
                    data = { appRevisionId: RevisionIdVal, versionId: VersionIdVal, workflowProcess: "HaulierApplication", isVr1: $('#IsVR1').val() };
                    div = '#overview_info_section';
                    initMethods = function () {
                        ApplicationOverviewInit();
                    };
                }
                break;
            default:
                url = '../Movements/SelectVehicle';
                div = '#select_vehicle_section';
                initMethods = function () {
                    MovementSelectVehicleInit();
                };
                break;
        }
        LoadContentForAjaxCalls(type, url, data, div, '', initMethods);
    }
    else {
        if (isSortAppVal == "True") {
            LoadContentForAjaxCalls("POST", '../SORTApplication/HaulierDetails', { SORTStatus: "CreateSO" }, '#haulier_details_section', '', function () {
                SORTHaulierDetailsInit();
                CreateOrganisationInit();
                $('#save_btn').show();
                $('#confirm_btn').hide();
            });
        }
        else {
            if ($("#IsVSO").val() == 'True' || $("#IsVSO").val() == 'true') {
                LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { IsVSO: true }, '#select_vehicle_section', true, '', function () {
                });
                MovementSelectVehicleInit();
                $('#back_btn').hide();
            }
            else {
                LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', {}, '#select_vehicle_section', false, function () {
                    MovementSelectVehicleInit();
                });
            }
        }
    }
}
function LoadContentForAjaxCalls(Type, Url, Params, ResLoadContnr, isVSO, callBackFn, routeVehicleFlag) {

    $.ajax({
        type: Type,
        url: Url,
        data: Params,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            if (routeVehicleFlag && response.flag) {
                ShowInfoPopup("The movement does not contain the vehicle/route details");
            }
            else {
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
                $('#overview_info_section').html('');
                $('#vehicle_edit_section').html('');
                $('#vehicle_Component_edit_section').html('');
                $('#vehicle_Create_section').html('');
                $('#vehicle_edit_section').hide();
                $('#vehicle_Component_edit_section').hide();
                $('#vehicle_Create_section').hide();
                $('#AllocateUser').hide();
                $("#allocate_btn").hide();
                $('#select_route_section_agredRt').hide();
                $('#divRouteActionsContainer').hide();
                $('#backBtnExistingSOVr1').hide();
                $(ResLoadContnr).show();
                $(ResLoadContnr).html(response);
                if (typeof callBackFn != 'undefined' && callBackFn != null && callBackFn != "") {
                    callBackFn();                 
                }
                
            }
        },
        error: function (response) {
            $(ResLoadContnr).html(response.responseText);
        },
        complete: function () {
            if (typeof callBackFn != 'function' && (typeof callBackFn == 'undefined' || callBackFn == null || callBackFn == '' || callBackFn.length ==0)) {
                stopAnimation();
                CloseWarningPopupDialog();
                CloseWarningPopupRef();
            }
            // Show VSO popup
            if (isVSO == true) {
                notifiedSOAPolice();                
            }
        }
    });
}
function BackToPrevMovm() {
    LoadContentForAjaxCalls("POST", '../SORTApplication/GetPreviousMovemntList', { projectId: PrePrjId, isVehicleImport: $("#IsVehicle").val(), movmntType: PreMovType }, '#select_route_section');
    $('#back_btn_Rt_prv').hide();
    $('#back_btn').show();
    $('#back_btn_Altered').hide();
}
function OnBackButtonClick(index, IsPopUpDisplayed = false, removeLocalObject = false) {
    if (IsPopUpDisplayed) {
        IsRoutePlanned = false;
        CloseWarningPopupRef();
        $('#pop-warning').modal('hide');
    }

    if (removeLocalObject) {
        RemoveLocalObject(StepFlag);
    }

    //if (BackAndForthNavMethods.IsWorkFlowStarted() && StepFlag == NavigationEnum.ROUTE_DETAILS) {
    //    var isUnsavedChange = ValidateUnSavedChange("map");
    //    if (isUnsavedChange) {
    //        ShowWarningPopup("There are unsaved changes in the current route. Do you want to save?", "CloseWarningPopupRef", "OnBackButtonClick",StepFlag, true);
    //        return false;
    //    }
    //}
    var isValidForNav = true;
    if (isValidForNav && !removeLocalObject) {
        isValidForNav = BackAndForthNavMethods.MovementTypeNavBack(index, true);
    }

    if (isValidForNav && BackAndForthNavMethods.IsNotification() == false && $('#supplimentaryinfo').length > 0 && !removeLocalObject) {//edit mode
        isValidForNav = BackAndForthNavMethods.SupplymentaryInfoNavBack(index, true);
    }
    if (isValidForNav && !IsPopUpDisplayed && !removeLocalObject) {
        isValidForNav = BackAndForthNavMethods.OverviewNav(index,true);
    }

    if (!isValidForNav) {
        return false;
    }

    if (StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY) {
        $('#route_assessment_next_btn').hide();
    }

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
        error: function (page) {
            $('#vehicle_import_section').html(page.responseText);
        },
        complete: function () {
        }
    });
    if (SubStepFlag != 5.1 && (StepFlag==0 && SubStepFlag != 0.1)) {
        HideUnloadSections();
    }
    if (SubStepFlag != 5.1 && (StepFlag == 1 && SubStepFlag != 0.1)) {
        HideUnloadSections();
    }
    IsRoutePlanned = false;
    switch (StepFlag) {
        case 0:
            if (SubStepFlag == 0.1 && $('#desc-entry #OrgName').is(":visible")) {
                window.location = '../Home/SORT';
            }
            else if (SubStepFlag == 0.1 && ($('#desc-entry #OrgName').length <= 0 || $('#desc-entry #OrgName').is(":visible")==false)) {
                $('#haulier_details_section').show();
                $('#viewExistingOrganisation').hide();
                $('#viewExistingOrganisation').html('');
                $('#existingOrganisationList').hide();
                $("#createOrganisation").show();
                $("#Go_To_Organisations").show();
                $('#save_btn').show();
                SubStepFlag = 0;
            }
            else if (SubStepFlag == 0.2 || SubStepFlag == 0.3) {
                BackToCreateHaulier();
                $('#haulier_details_section').show();
                $('#save_btn').show();
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
                if ((BackToPreviousMovementList == true || previousList == "true") && (importFrom == 'prevMov') && ($('#div_so_movement_list').length <= 0)) {
                    ImportVehicle('prevMov', 'true');
                    BackToPreviousMovementList = false;
                    break;
                }
                else if (SelectedVehicles.length > 0) {
                    StepFlag = 2;
                    SubStepFlag = 0;
                    CurrentStep = "Vehicle Details";
                    $('#current_step').text(CurrentStep);
                    $('#select_vehicle_section').html('');
                    $('#select_vehicle_section').hide;
                    $('#vehicle_details_section').show();
                    $('#confirm_btn').show();
                    $("#back_btn").hide();
                }
                else {
                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section', '', function () {
                        MovementSelectVehicleInit();
                    });
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
                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section', '', function () {
                        MovementSelectVehicleInit();
                    });
                }
                else {
                    StepFlag = 2;
                    CurrentStep = "Vehicle Details";
                    $('#current_step').text(CurrentStep);
                    $('#vehicle_details_section').show();
                    $('#confirm_btn').show();
                    $("#back_btn").hide();
                }
            }
            else if (SubStepFlag == 2.1) {
                if (SelectedVehicles.length == 0) {
                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section', '', function () {
                        MovementSelectVehicleInit();
                    });
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
                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section', '', function () {
                        MovementSelectVehicleInit();
                    });
                }
            }
            else if (userTypeIDVal == 696008) {
                StepFlag = 0;
                CurrentStep = "Haulier Details";
                $('#current_step').text(CurrentStep);
                $('#haulier_details_section').show();
                $('#confirm_btn').removeClass('blur-button');
                $('#confirm_btn').attr('disabled', false);
                $('#confirm_btn').show();
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
                    LoadContentForAjaxCalls("POST", '../Movements/MovementSelectedVehicles', { isBackCall: true, movementId: $('#MovementId').val() }, '#vehicle_details_section', '', function () {
                        MovementSelectedVehiclesInit();
                        VehicleDetailsInit();
                        ViewConfigurationGeneralInit();
                        GeneralVehicCompInit();
                        MovementAssessDetailsInit();
                    });
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
                        LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section', '', function () {
                            MovementSelectVehicleInit();
                        });
                    }
                }
                else {
                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section', '', function () {
                        MovementSelectVehicleInit();
                    });
                }
            }
            else if (SelectedVehicles.length > 0) {
                if (SubStepFlag == 2.4 || SubStepFlag == 1.3) {
                    $('#vehicle_edit_section').html('');
                    $('#vehicle_Component_edit_section').html('');
                    $('#vehicle_Create_section').html('');
                    $('#vehicle_edit_section').hide();
                    $('#vehicle_Component_edit_section').hide();
                    $('#vehicle_Create_section').hide();
                }
                StepFlag = 2;
                SubStepFlag = 0;
                CurrentStep = "Vehicle Details";
                $('#current_step').text(CurrentStep);
                $('#vehicle_details_section').show();
                $('#component_selection_section').hide();
                $('#confirm_btn').show();
                $("#back_btn").hide();
            }
            break;
        case 3: //Navigate back from mvnt confirmtn page
            StepFlag = 2;
            CurrentStep = "Vehicle Details";
            $('#current_step').text(CurrentStep);
            LoadContentForAjaxCalls("POST", '../Movements/MovementSelectedVehicles', { isBackCall: true, movementId: $('#MovementId').val() }, '#vehicle_details_section', '', function () {
                MovementSelectedVehiclesInit();
                VehicleDetailsInit();
                ViewConfigurationGeneralInit();
                GeneralVehicCompInit();
                MovementAssessDetailsInit();
            });
            $('#save_btn').hide();
            $('#apply_btn').hide();
            $('#confirm_btn').removeClass('blur-button');
            $('#confirm_btn').attr('disabled', false);
            $('#confirm_btn').show();
            $('#backbutton').show();
            break;
        case 4: //Navigate back from Route page
            var previousList = "false";
            if ($('#BackToRoutePreviousList').val() != undefined) {
                previousList = $('#BackToRoutePreviousList').val().toLowerCase();
            }
            if (SubStepFlag == 0) {
                $('#select_route_section').html('');
                LoadContentForAjaxCalls("POST", '../Movements/GetMovementTypeConfirmation', { appRevisionId: $('#AppRevisionId').val(), appVersionId: $('#AppVersionId').val(), notificationId: $('#NotificationId').val(), isBackCall: true }, '#movement_type_confirmation', '', function () {
                    MovementTypeConfirmationInit();
                });
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
                    LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication' }, '#select_route_section', '', function () {
                        MovementRouteInit();
                    });
                }
            }
            else if (SubStepFlag == 4.2) {
                if (isSortAppVal == "True") {
                    $('#select_route_section').show();
                    $('#back_btn_Rt_prv').show();
                    $('#back_btn').hide();
                    $('#back_btn_Altered').hide();
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
                LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication', IsRouteSummaryPage: $('#IsRouteSummary_Flag').val() }, '#select_route_section', '', function () {
                    var IsRouteSummary = false;
                    if ($('#IsRouteSummary_Flag').val() == "1") {
                        IsRouteSummary = true;
                    }
                    MovementRouteInit(IsRouteSummary);
                });
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
                LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication' }, '#select_route_section', '', function () {
                    MovementRouteInit();
                });
            }
            break;
        case 6: //Navigate back from overview page to Supplimetary View / Save for SO/VR1 or to Route Assessment for Notification
            StepFlag = 5;
            if ($('#IsNotif').val().toLowerCase() == 'true') {
                LoadContentForAjaxCalls("POST", '../Notification/NotificationRouteAssessment', { workflowProcess: 'HaulierApplication' }, '#route_assessment_section', '', function () {
                    NotificationRouteAssessmentInit();
                });
            }
            else {
                CurrentStep = "Supplimentary Information";
                if (IsSupplimentarySavedVal == 'True') {
                    LoadContentForAjaxCalls("POST", '../Application/ViewSupplementary', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section', '', function () {
                        ViewSupplementaryInit();
                    });
                }
                else {
                    LoadContentForAjaxCalls("POST", '../Application/ApplicationSupplimentaryInfo', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section', '', function () {
                        ApplicationSupplimentaryInfoInit();
                    });
                }
            }
            break;
    }
}
function OnConfirm() {
    $(".btn-primary").blur();    
    IsRoutePlanned = false;
    AfConArray = [];
    AfStArray = [];
    switch (StepFlag) {
        case 0: //On confirm from haulier details page
            RemoveLocalObject(StepFlag);
            if (SubStepFlag == 0.2) {
                if ($('#AppRevisionId').val() > 0) {
                    // LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', {}, '#select_vehicle_section');
                    StepFlag = 2;
                    CurrentStep = "Vehicle Details";
                    $('#current_step').text(CurrentStep);
                    //$('#vehicle_details_section').show();
                    LoadContentForAjaxCalls("POST", '../Movements/MovementSelectedVehicles', { movementId: $('#MovementId').val() }, '#vehicle_details_section', '', function () {
                        MovementSelectedVehiclesInit();
                        VehicleDetailsInit();
                        ViewConfigurationGeneralInit();
                        GeneralVehicCompInit();
                        MovementAssessDetailsInit();
                    });
                    $('#save_btn').hide();
                    $('#apply_btn').hide();
                    $('#confirm_btn').removeClass('blur-button');
                    $('#confirm_btn').attr('disabled', false);
                    $('#confirm_btn').show();
                    $('#back_btn').show();
                    $('#backbutton').show();
                } else {
                    ValidateHaulierDetails();
                }
            }
            else {
                //LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', {}, '#select_vehicle_section', '', function () {
                //    MovementSelectVehicleInit();
                //});
                var movmtId = $('#MovementId').val();
                if (movmtId != '' && movmtId != '0' && movmtId != undefined) {
                    LoadContentForAjaxCalls("POST", '../Movements/MovementSelectedVehicles', { isBackCall: true, movementId: $('#MovementId').val() }, '#vehicle_details_section', '', function () {
                        MovementSelectedVehiclesInit();
                        VehicleDetailsInit();
                        ViewConfigurationGeneralInit();
                        GeneralVehicCompInit();
                        MovementAssessDetailsInit();
                    });
                } else {
                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section', '', function () {
                        MovementSelectVehicleInit();
                    });
                }
            }
            break;
        case 1: //On confirm from select vehicle page
            RemoveLocalObject(StepFlag);
            NavigationMethods.SelectVehicleConfirm();
            
            break;
        case 2: //On confirm from vehicle details page
            if ($('.imgAmendComponentAxle').length > 0 && $('.imgAmendComponentAxle').is(':visible')) {
                showToastMessage({
                    message: "Please amend axles for tractor before proceeding further.",
                    type: "error"
                });
                var x = $('#IsAddVehicleError').val();
                if ($('#IsAddVehicleError').val() == 10) {
                    ShowErrorPopup("One or more of your vehicles are not categorised as SO, Please edit or remove the vehicles.", 'CloseErrorPopup');
                }
                break;
            }
            RemoveLocalObject(StepFlag);
            NavigationMethods.VehicleDetailsConfirm();
            break;
        case 3: //On confirm from mvmnt confirmtn page'
            NavigationMethods.MovementTypeConfirm();
            break;
        case 4: //On confirm from route details page
            CheckBrokenRouteExistBeforeSubmit(function () {
                RemoveLocalObject(StepFlag);
                NavigationMethods.RouteDetailsConfirm();
            });
            break;
        case 5: 
            //On confirm from supplimentary/route assessment page
            RemoveLocalObject(StepFlag);
            NavigationMethods.RouteAssessment_SupplymentoryConfirm();
            break;
    }
}
var NavigationMethods = {
    SelectVehicleConfirm: function () {
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
            $('#backbutton').show();
        
    },
    VehicleDetailsConfirm: function () {
            if ($('#IsAddVehicleError').val() == 6) {
                if (IsSortAppVal == "True") {
                    ShowErrorPopup("One or more of your vehicles does not require a SO/VR1. Please remove or edit the vehicle before proceeding.", 'CloseErrorPopup');
                }
                else {
                    ShowErrorPopup("One or more of your vehicles does not require a notification. Please remove or edit the vehicle before proceeding. If you need assistance please contact the helpdesk on 0300 470 3733.", 'CloseErrorPopup');
                }
            }
            else if ($('#IsAddVehicleError').val() == 10) {
                ShowErrorPopup("One or more of your vehicles are not categorised as SO, Please edit or remove the vehicles.", 'CloseErrorPopup');
            }
            else if ($('#IsAddVehicleError').val() == 2) {
                ShowErrorPopup("The vehicles in this movement are in different category. So we are unable to proceed further. Please edit the vehicle and proceed.", 'CloseErrorPopup');
            }
            else if ($('#IsAddVehicleError').val() == 3) {
                var msg = $('#hf_IsAddVehicleErrorMsg').val();
                ShowDialogWarningPop(msg, 'Ok', '', 'CloseWarningPopupDialog');
            }
            else if ($('#IsAddVehicleError').val() == 4) {
                ShowErrorPopup("Total axle distance exceeds vehicle length for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');
            }
            
            else if (MovementClassIdVal == 207004 || $('#IsAddVehicleError').val() == 7) {
                if (IsSortAppVal == "True") {
                    ShowErrorPopup("One or more of your vehicles does not require a SO/VR1. Please remove or edit the vehicle before proceeding.", 'CloseErrorPopup');
                }
                else {
                    ShowErrorPopup("One or more of your vehicles does not require a notification. Please remove or edit the vehicle before proceeding. If you need assistance please contact the helpdesk on 0300 470 3733.", 'CloseErrorPopup');
                }
            }
            else if ($('#IsAddVehicleError').val() == 1) {
                localStorage.setItem("isVehicleHasChanges", true);
            }
            else if ($('#IsAddVehicleError').val() != 5) {
                var isBckCall = false;
                var appRevId = $('#AppRevisionId').val();
                var notifId = $('#NotificationId').val()
                if (appRevId > 0 || notifId > 0) {
                    isBckCall = true;
                }
                var VehicleClass = $("#hf_VehicleClassNew").val();
                var VSOType = $("#hf_VSOTypeNew").val();
                var isVSO = false;
                if (VehicleClass == '241001' && (VSOType == '' || VSOType == '0' || VSOType == undefined)) {//If vehicle class is Vehicle Special Order and VSO type is not updated, then popup will display
                    isVSO = true;
                }
                LoadContentForAjaxCalls("POST", '../Movements/GetMovementTypeConfirmation', { isBackCall: isBckCall }, '#movement_type_confirmation', isVSO, function () {
                    MovementTypeConfirmationInit();
                });
            }
            else {
                ShowErrorPopup("Based on the vehicle details you entered, system assess the movement as a Notification and thus cannot proceed further. Please edit the vehicle dimensions to match with an application", 'CloseErrorPopup');
            }
    },
    MovementTypeConfirm: function (NavigateToNextPage = true, NavigateToLink = '') {
        var radioValue = $("input[name='applicationType']:checked").val();
        if (radioValue == "207003") {
            AmendFlag = GetVehicleRegistration();
        }
        else { AmendFlag = 1;}

        if (AmendFlag == 0) {
            CloseWarningPopupRef();
            CloseWarningPopupDialog();
            ShowInfoPopup('The vehicle registration details are missing for the vehicle. Go to vehicle details page and click on amend vehicle button to enter the mandatory registration details');
        }
        else {
            if ($('#NotificationId').val() != 0 || $('#AppRevisionId').val() != 0) {
                if (MovementTypeValidation()) {
                    var existingData = GetLocalObject(StepFlag, false);
                    var addedData = CreateLocalObject(StepFlag, false);
                    existingData = existingData != '' ? JSON.parse(existingData) : undefined;
                    addedData = addedData != '' ? JSON.parse(addedData) : undefined;
                    //check movent type or date changes, then user should move forward using below button
                    if ((existingData != null && addedData != null) && BackAndForthNavMethods.IsNotification() == false &&
                        (existingData.ApplicationRadio != addedData.ApplicationRadio || existingData.NotificationRadio != addedData.NotificationRadio)) {
                        localStorage.setItem('isMovementTypeHasMajorChanges', 1);
                        RemoveLocalObject(StepFlag);
                    } else if ((existingData != null && addedData != null) && BackAndForthNavMethods.IsNotification() == true &&
                        (existingData.FromDate != addedData.FromDate || existingData.ToDate != addedData.ToDate
                            || existingData.ApplicationRadio != addedData.ApplicationRadio || existingData.NotificationRadio != addedData.NotificationRadio)) {
                        localStorage.setItem('isMovementTypeHasMajorChanges', 1);
                        RemoveLocalObject(StepFlag);
                    }
                    UpdateMovementType(NavigateToNextPage, NavigateToLink);
                }
                else {
                    if (NavigateToLink != undefined && NavigateToLink != null && NavigateToLink != "")
                        UpdateMovementType(NavigateToNextPage, NavigateToLink);// Save the changes to DB and return to Navigate Url
                }
            }
            else {
                if (MovementTypeValidation()) {
                    if ($('#ApplicationRadio').is(':visible') && $('#NotificationRadio').is(':checked')) {
                        $('#Vr1SoExistingPopUp').modal({ keyboard: false, backdrop: 'static' });
                        $('#Vr1SoExistingPopUp').modal('show');
                        return false;
                    }
                    else {
                        InsertMovementType();
                    }
                }
            }
        }
        RemoveLocalObject(StepFlag);
    },
    RouteDetailsConfirm: function () {
        isVehicleChanged = false;//This will be used for top navigation validation
            if (SubStepFlag != 4.3) {
                if (AutoAssignRouteVehicle) {//if one route or one vehicle
                    AutoAssignVehiclesToRoute();
                }
                else {//multiple routes
                    if (typeof btnContinueToRouteAssessmentClicked != 'undefined' && btnContinueToRouteAssessmentClicked) {
                        VehicleAssigned = Boolean($('#hf_VehicleAssignedGlobal').val().toLowerCase() == 'true');
                        if (!VehicleAssigned) {
                            LoadContentForAjaxCalls("POST", '../Movements/GetVehicleAssignmentList', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), workflowProcess: 'HaulierApplication' }, '#route_vehicle_assign_section', '', function () {
                                MovementRouteVehAssignInit();
                            });
                        }
                        else {
                            NavigateToRouteAssessmnetSupply();//If vehicle-route is auto assigned or already assigned, we don't need to show Assignment page.
                        }
                    }
                    else {
                        LoadContentForAjaxCalls("POST", '../Movements/GetVehicleAssignmentList', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), workflowProcess: 'HaulierApplication' }, '#route_vehicle_assign_section', '', function () {
                            MovementRouteVehAssignInit();
                        });
                    }
                    btnContinueToRouteAssessmentClicked = false;
                }
            }
            else {
                AssignVehiclesToRoute();  //On confirm from route assignment page
            }
    },
    RouteAssessment_SupplymentoryConfirm: function () {
        if (BackAndForthNavMethods.IsNotification()) {
                IsRouteAnalysisComplete();
            }
            else {
            LoadContentForAjaxCalls("POST", '../Application/ApplicationOverview', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), workflowProcess: "HaulierApplication", isVr1: $('#IsVR1').val() }, '#overview_info_section', '', function () {
                    ApplicationOverviewInit();
                    localStorage.removeItem("isVehicleHasChanges");
                    localStorage.removeItem("isMovementTypeHasMajorChanges");
                    isVehicleHasChanged = false;
                });
            }
    }
}
var NavigationEnum = {
    HAULIER_DETAILS: 0,
    SELECT_VEHICLE: 1,
    VEHICLE_DETAILS: 2,
    MOVEMENT_TYPE: 3,
    ROUTE_DETAILS: 4,
    ROUTEASSESSMNT_SUPPLY: 5,
    OVERVIEW: 6
}
var existingVehicleIds;
var isTopNavClicked = false;
var isMovementTypeHasChanges = false;
var isVehicleHasChanged = false;
var isTopNavOverviewClicked = false;
var isVehicleChanged = false;
var isMovementTypeHasMajorChanges = false;
function NavigationForWorkflowSequenceOnTopNavClick(NavStep, IsPopUpDisplayed = false, CheckMovementType = true, CheckVehicle = true, CheckOverview = true, checkRoute = true) {
    CloseWarningPopupRef();
    CloseWarningPopupDialog();
    if (IsPopUpDisplayed) {
        IsRoutePlanned = false;
    }
    var isValidForNav = true;
    isTopNavClicked = true;
    //if (($('#MvmtConfirmFlag').val() == 'true') && (NavStep == 0 || NavStep == 1 || NavStep == 2))
    //RemoveNavForWrkflwSequence();   //remove work flow sequence nav if the user comes back to the vehicle page to edit

    if (NavStep == StepFlag || (StepFlag == NavigationEnum.VEHICLE_DETAILS && NavStep == NavigationEnum.SELECT_VEHICLE)) {
        return false;
    }
    isValidForNav = BackAndForthNavMethods.VehiclePageNav(NavStep);
    if (isValidForNav && CheckMovementType) {
        isValidForNav = BackAndForthNavMethods.MovementTypeNav(NavStep);
    }
    if (checkRoute && isValidForNav) {
        isValidForNav = BackAndForthNavMethods.RoutePageNav(NavStep);
    }

    if (isValidForNav && BackAndForthNavMethods.IsNotification() == false && $('#supplimentaryinfo').length > 0) {//edit mode
        isValidForNav = BackAndForthNavMethods.SupplymentaryInfoNav(NavStep);
    }

    if (CheckVehicle && isValidForNav && (StepFlag == NavigationEnum.VEHICLE_DETAILS || StepFlag == NavigationEnum.SELECT_VEHICLE)) {
        if (isVehicleHasChanged) {
            ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "SaveCheckVehicleOnForthNav", "CloseWarningPopupRef", NavStep);
            return false;
        }
    }
    isMovementTypeHasMajorChanges = localStorage.getItem('isMovementTypeHasMajorChanges');
    if (isMovementTypeHasMajorChanges && isValidForNav &&
        BackAndForthNavMethods.IsNotification() && (NavStep == NavigationEnum.ROUTEASSESSMNT_SUPPLY || NavStep == NavigationEnum.OVERVIEW) && StepFlag != NavigationEnum.OVERVIEW) {
        var mvtTypeWarningMsg = "There are changes in movement type details. Use CONFIRM AND CONTINUE button to proceed further.";
        if (StepFlag == NavigationEnum.ROUTE_DETAILS && $('#btn_continue_to_route_assessment').is(':visible')) {
            mvtTypeWarningMsg = "There are changes in movement type details. Use CONTINUE TO ROUTE ASSESSMENT button to proceed further.";
        }
        if (StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY && $('#route_assessment_next_btn').is(':visible')) {
            mvtTypeWarningMsg = "There are changes in movement type details. Use NEXT button to proceed further.";
        }
        showToastMessage({
            message: mvtTypeWarningMsg,
            type: "error"
        });
        return false;
    }

    //Reopen WIP - Route assessment
    if (isRouteAssessmentRequiredForWIP && isValidForNav && NavStep == NavigationEnum.OVERVIEW) {
        var mvtTypeWarningMsg = "Route assessment need to be done. Use CONFIRM AND CONTINUE button to proceed further.";
        if (StepFlag == NavigationEnum.ROUTE_DETAILS && $('#btn_continue_to_route_assessment').is(':visible')) {
            mvtTypeWarningMsg = "Route assessment need to be done. Use CONTINUE TO ROUTE ASSESSMENT button to proceed further.";
        }
        if (StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY && $('#route_assessment_next_btn').is(':visible')) {
            mvtTypeWarningMsg = "Route assessment need to be done. Use NEXT button to proceed further.";
        }
        showToastMessage({
            message: mvtTypeWarningMsg,
            type: "error"
        });
        return false;
    }
    

    if (isValidForNav) {
        isValidForNav = BackAndForthNavMethods.OverviewNav(NavStep);
    }
    
    if (isValidForNav) {
        if (StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY) {
            $('#route_assessment_next_btn').hide();
        }
        //HideUnloadSections();
    IsRoutePlanned = false;
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
                GetLocalObject(StepFlag, updateControls = true);

                $('#select_vehicle_section').hide();
                $('#vehicle_import_section').hide();
                $('#vehicle_details_section').hide();
                $('#movement_type_confirmation').hide();
                $('#select_route_section').hide();
                $('#route').html('');
                $('#route').hide();
                $('#route_vehicle_assign_section').hide();
                $('#supplimentary_info_section').hide();
                $('#overview_info_section').hide();
                $('#vehicle_edit_section').html('');
                $('#vehicle_Component_edit_section').html('');
                $('#vehicle_Create_section').html('');
                $('#vehicle_edit_section').hide();
                $('#vehicle_Component_edit_section').hide();
                $('#vehicle_Create_section').hide();

                $('#haulier_details_section').show();
            }
            SetWorkflowProgress(NavigationEnum.HAULIER_DETAILS);
            $('#back_btn').hide();
            $('#confirm_btn').removeClass('blur-button');
            $('#confirm_btn').attr('disabled', false);
            $('#confirm_btn').show();
            $('#backbutton').show();
            break;
        case 1:  //Select Vehicle
        case 2:  //Vehicle Details
            StepFlag = 2;
            CurrentStep = "Vehicle Details";
            $('#current_step').text(CurrentStep);
            var movmtId = $('#MovementId').val();
            if (movmtId != '' && movmtId != '0' && movmtId != undefined) {
                LoadContentForAjaxCalls("POST", '../Movements/MovementSelectedVehicles', { isBackCall: true, movementId: $('#MovementId').val() }, '#vehicle_details_section', '', function () {
                    MovementSelectedVehiclesInit();
                    VehicleDetailsInit();
                    ViewConfigurationGeneralInit();
                    GeneralVehicCompInit();
                    MovementAssessDetailsInit();
                });
            } else {
                LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section', '', function () {
                    MovementSelectVehicleInit();
                });
            }
            $('#save_btn').hide();
            $('#apply_btn').hide();
            $('#confirm_btn').removeClass('blur-button');
            $('#confirm_btn').attr('disabled', false);
            $('#confirm_btn').show();
            $('#backbutton').show();
            break;
        case 3:  //Movement Type
            var VehicleClass = $("#hf_VehicleClassNew").val();
            var VSOType = $("#hf_VSOTypeNew").val();
            var isVSO = false;
            if (VehicleClass == '241001' && (VSOType == '' || VSOType == '0' || VSOType == undefined)) {//If vehicle class is Vehicle Special Order and VSO type is not updated, then popup will display
                isVSO = true;
            }
            LoadContentForAjaxCalls("POST", '../Movements/GetMovementTypeConfirmation', { isBackCall: true }, '#movement_type_confirmation', isVSO, function () {
                MovementTypeConfirmationInit();
            });
            break;
        case 4:  //Route Details
            LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication' }, '#select_route_section', '', function () {
                MovementRouteInit();
            });
            break;
        case 5:  //Supplimentary info/Route Assessmnt

            
            VehicleAssigned = Boolean($('#hf_VehicleAssignedGlobal').val().toLowerCase() == 'true');
            if (VehicleAssigned || NavStep < StepFlag) {//NavStep 5, StepFlag 6
                if (StepFlag == NavigationEnum.ROUTE_DETAILS) {
                    CheckBrokenRouteExistBeforeSubmit(function () {
                        StepFlag = 5;
                        NavigateToRouteAssessmnetSupply();
                    });
                } else {
                    StepFlag = 5;
                    NavigateToRouteAssessmnetSupply();
                }
            }
            else {
                var msg = "All vehicles have not been assigned to routes.";
                showToastMessage({
                    message: msg,
                    type: "error"
                });
            }
            break;
        case 6:
            if (StepFlag == NavigationEnum.ROUTE_DETAILS) {
                var input = CheckBrokenRouteExistBeforeSubmit();
                CheckIsBroken(input, function (response) {
                    if (response && response.brokenRouteCount > 0) {//Check broken route exist and show message
                        ShowWarningPopupMapupgarde(BROKEN_ROUTE_MESSAGES.PLAN_MOV_ON_MAP_PAGE_CONFIRM, function () {
                            $('#WarningPopup').modal('hide');
                        });
                        return false;
                    } else {
                        isTopNavOverviewClicked = true;
                        if (BackAndForthNavMethods.IsNotification()) {
                            IsRouteAnalysisComplete();
                        }
                        else {
                            LoadContentForAjaxCalls("POST", '../Application/ApplicationOverview', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), workflowProcess: "HaulierApplication", isVr1: $('#IsVR1').val() }, '#overview_info_section', '', function () {
                                ApplicationOverviewInit();
                            });
                        }
                    }
                });
            } else {
                isTopNavOverviewClicked = true;
                if (BackAndForthNavMethods.IsNotification()) {
                    IsRouteAnalysisComplete();
                }
                else {
                    LoadContentForAjaxCalls("POST", '../Application/ApplicationOverview', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), workflowProcess: "HaulierApplication", isVr1: $('#IsVR1').val() }, '#overview_info_section', '', function () {
                        ApplicationOverviewInit();
                    });
                }
            }
            break;
        }
    }
}
var BackAndForthNavMethods = {
    VehiclePageNav: function (NavStep,IsBack=false) {
        var isValidForNav = true;
        if (SelectedVehicles == null || SelectedVehicles == undefined || SelectedVehicles.length <= 0) {
            console.log('No Vehicles Added/ Page refresh');
        }
        //Check Vehicle details is changed
        var isVehicleHasChanges = localStorage.getItem("isVehicleHasChanges");
        var msg = "There are changes in vehicle. Use CONFIRM AND CONTINUE button to proceed further.";
        if (StepFlag == NavigationEnum.VEHICLE_DETAILS || StepFlag == NavigationEnum.SELECT_VEHICLE) {
            if (existingVehicleIds && JSON.stringify(existingVehicleIds) != JSON.stringify(SelectedVehicles)) {
                console.log('Vehicle Changed');
                isVehicleChanged = true;
                // Check Route assessment/Overview
                if ((NavStep == NavigationEnum.MOVEMENT_TYPE || NavStep == NavigationEnum.ROUTE_DETAILS || NavStep == NavigationEnum.ROUTEASSESSMNT_SUPPLY ||
                    NavStep == NavigationEnum.OVERVIEW))
                {
                    msg = $('#IsAddVehicleError').val() == 1 && $('#vehicleassesserror').length > 0 ? $('#vehicleassesserror').text() :
                        BackAndForthNavMethods.IsNotification() ? "There are changes in vehicle. Route assessment need to be done. Use CONFIRM AND CONTINUE button to proceed further." :
                            msg;
                    if (StepFlag == NavigationEnum.ROUTE_DETAILS && BackAndForthNavMethods.IsNotification() && $('#btn_continue_to_route_assessment').is(':visible')) {
                        msg = "There are changes in vehicle. Use CONTINUE TO ROUTE ASSESSMENT button to proceed further.";
                    }
                    if (StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY && $('#route_assessment_next_btn').is(':visible')) {
                        msg = "There are changes in vehicle. Use NEXT button to proceed further.";
                    }
                    isValidForNav = false;
                    showToastMessage({
                        message: msg,
                        type: "error"
                    });
                    return isValidForNav;
                } 
            }
            else if (isVehicleHasChanges && isVehicleHasChanges == 'true' &&
                (NavStep == NavigationEnum.MOVEMENT_TYPE || NavStep == NavigationEnum.ROUTE_DETAILS || NavStep == NavigationEnum.ROUTEASSESSMNT_SUPPLY ||
                    NavStep == NavigationEnum.OVERVIEW || (BackAndForthNavMethods.IsNotification() == false && NavStep == NavigationEnum.ROUTEASSESSMNT_SUPPLY)))
            {
                isValidForNav = false;
                isVehicleChanged = true;
                msg = $('#IsAddVehicleError').val() == 1 && $('#vehicleassesserror').length > 0 ? $('#vehicleassesserror').text() :
                    BackAndForthNavMethods.IsNotification() ? "There are changes in vehicle configuration. Route assessment need to be done. Use CONFIRM AND CONTINUE button to proceed further." :
                        "There are changes in vehicle configuration. Use CONFIRM AND CONTINUE button to proceed further.";
                if (StepFlag == NavigationEnum.ROUTE_DETAILS && BackAndForthNavMethods.IsNotification() && $('#btn_continue_to_route_assessment').is(':visible')) {
                    msg = "There are changes in vehicle configuration. Use CONTINUE TO ROUTE ASSESSMENT button to proceed further.";
                }
                if (StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY && $('#route_assessment_next_btn').is(':visible')) {
                    msg = "There are changes in vehicle configuration. Use NEXT button to proceed further.";
                }
                showToastMessage({
                    message: msg,
                    type: "error"
                });
            }
        }
        return isValidForNav;
    },
    MovementTypeNav: function (NavStep, IsBack = false) {
        var isValidForNav = true;
        if (StepFlag == NavigationEnum.MOVEMENT_TYPE) {
            isMovementTypeHasMajorChanges = localStorage.getItem('isMovementTypeHasMajorChanges');
            if (isMovementTypeHasMajorChanges != 1 && isMovementTypeHasMajorChanges != 2)
                localStorage.removeItem('isMovementTypeHasMajorChanges');
            else if (isMovementTypeHasMajorChanges == 1) {
                isValidForNav = false;
                showToastMessage({
                    message: "There are changes in movement type details. Use CONFIRM AND CONTINUE button to proceed further.",
                    type: "error"
                });
            }
            if (BackAndForthNavMethods.IsWorkFlowStarted() && !MovementTypeValidation()) {
                return true;
            }
            if (isVehicleChanged && NavStep > StepFlag) {
                var message = "There are changes in vehicle. Use CONFIRM AND CONTINUE button to proceed further.";
                showToastMessage({
                    message: message,
                    type: "error"
                });
                return false;
            }
            var existingData = GetLocalObject(StepFlag, false);
            var addedData = CreateLocalObject(StepFlag, false);
            existingData = existingData != '' ? JSON.parse(existingData) : undefined;
            addedData = addedData != '' ? JSON.parse(addedData) : undefined;
            if (BackAndForthNavMethods.IsWorkFlowStarted() && (NavStep == NavigationEnum.OVERVIEW || NavStep == NavigationEnum.ROUTE_DETAILS || NavStep == NavigationEnum.ROUTEASSESSMNT_SUPPLY)) {
                if (MovementTypeValidation()) {
                    //check movent type or date changes, then user should move forward using below button
                    if ((existingData != null && addedData != null) && BackAndForthNavMethods.IsNotification() == false &&
                        (existingData.ApplicationRadio != addedData.ApplicationRadio || existingData.NotificationRadio != addedData.NotificationRadio)) {
                        isValidForNav = false;
                        localStorage.setItem('isMovementTypeHasMajorChanges', 1);
                        showToastMessage({
                            message: "There are changes in movement type details. Use CONFIRM AND CONTINUE button to proceed further.",
                            type: "error"
                        });
                    } else if ((existingData != null && addedData != null) && BackAndForthNavMethods.IsNotification() == true &&
                        (existingData.FromDate != addedData.FromDate || existingData.ToDate != addedData.ToDate
                        || existingData.ApplicationRadio != addedData.ApplicationRadio || existingData.NotificationRadio != addedData.NotificationRadio)) {
                        isValidForNav = false;
                        localStorage.setItem('isMovementTypeHasMajorChanges', 1);
                        showToastMessage({
                            message: "There are changes in movement type details. Use CONFIRM AND CONTINUE button to proceed further.",
                            type: "error"
                        });
                    } else if ((existingData != null && addedData != null)) {
                        //check other values has difference
                        if (JSON.stringify(existingData) != JSON.stringify(addedData)) {
                            ShowDialogWarningPop('There are unsaved changes. Do you want to save?', 'Save & Continue', 'Discard Changes & Continue', 'SaveMovementTypeDataOnForthNav',
                                'UnSaveMovementTypeDataOnForthNav', 1, '', NavStep);

                            return false;
                        }
                    }
                }
            }

            if (BackAndForthNavMethods.IsWorkFlowStarted() && (NavStep == NavigationEnum.VEHICLE_DETAILS || NavStep == NavigationEnum.SELECT_VEHICLE || NavStep == NavigationEnum.HAULIER_DETAILS)) {
                if (MovementTypeValidation()) {
                    //check movent type or date changes, then user should move forward using below button
                    if ((existingData != null && addedData != null) && BackAndForthNavMethods.IsNotification() == false &&
                        (existingData.ApplicationRadio != addedData.ApplicationRadio || existingData.NotificationRadio != addedData.NotificationRadio)) {
                        isValidForNav = false;
                        localStorage.setItem('isMovementTypeHasMajorChanges', 1);
                        showToastMessage({
                            message: "There are changes in movement type details. Use CONFIRM AND CONTINUE button to proceed further.",
                            type: "error"
                        });
                    } else if ((existingData != null && addedData != null) && BackAndForthNavMethods.IsNotification() == true && (existingData.FromDate != addedData.FromDate || existingData.ToDate != addedData.ToDate
                        || existingData.ApplicationRadio != addedData.ApplicationRadio || existingData.NotificationRadio != addedData.NotificationRadio)) {
                        isValidForNav = false;
                        localStorage.setItem('isMovementTypeHasMajorChanges', 1);
                        showToastMessage({
                            message: "There are changes in movement type details. Use CONFIRM AND CONTINUE button to proceed further.",
                            type: "error"
                        });
                    } else if ((existingData != null && addedData != null)) {
                        //check other values has difference
                        if (JSON.stringify(existingData) != JSON.stringify(addedData)) {
                            ShowDialogWarningPop('There are unsaved changes. Do you want to save?', 'Save & Continue', 'Discard Changes & Continue', 'SaveMovementTypeDataOnForthNav',
                                'UnSaveMovementTypeDataOnForthNav', 1, '', NavStep);
                            return false;
                        }
                    }
                }
            }
        }
        return isValidForNav;
    },
    RoutePageNav: function (NavStep, IsBack = false) {
        var isValidForNav = true;
        if (BackAndForthNavMethods.IsWorkFlowStarted() && StepFlag == NavigationEnum.ROUTE_DETAILS && (NavStep == NavigationEnum.OVERVIEW || NavStep == NavigationEnum.ROUTEASSESSMNT_SUPPLY)) {
            var msg = "";
            if (isVehicleChanged && NavStep > StepFlag) {
                msg = "There are changes in vehicle. Use CONFIRM AND CONTINUE button to proceed further.";
                showToastMessage({
                    message: msg,
                    type: "error"
                });
                return false;
            }
            if (existingRouteIds && existingRouteIds != JSON.stringify(routeIdArr)) {
                isValidForNav = false;
                if ($('#confirm_btn').is(':visible')) {
                    msg = "There are changes in routes. Use CONFIRM AND CONTINUE button to proceed further.";
                }
                else if ($('#assignVehicleToRoute').is(':visible')) {
                    msg = "There are changes in routes. ASSIGN VEHICLE(S) TO ROUTE PART button to proceed further.";
                }
                else {
                    msg = "There are changes in routes. Please view the Manage Routes page.";
                }
                showToastMessage({
                    message: msg,
                    type: "error"
                });
                return isValidForNav;
            }
        }
        if (BackAndForthNavMethods.IsWorkFlowStarted() && StepFlag == NavigationEnum.ROUTE_DETAILS) {
            var isUnsavedChange = ValidateUnSavedChange("map") || CheckMapInputHasChanges();
            if (isUnsavedChange) {
                ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "NavigationForWorkflowSequenceOnTopNavClick", "CloseWarningPopupRef", NavStep,true);
                return false;
            }
        }
        return isValidForNav;
    },
    SupplymentaryInfoNav: function (NavStep, IsBack = false, NavigateToLink = '', callback = '', validateData = true) {
        var isValidForNav = true;
        if (StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY && BackAndForthNavMethods.IsNotification() == false) {
            if (BackAndForthNavMethods.IsWorkFlowStarted() && $('#supplimentaryinfo').length > 0 && !checknoncomplusaryfield()) {//edit mode
                if (validateData)
                    return true;
                else {
                    location.href = NavigateToLink;
                    return true;
                }
            }

            var existingData = GetLocalObject(StepFlag, false);
            var addedData = CreateLocalObject(StepFlag, false);
            existingData = existingData != '' ? JSON.parse(existingData) : undefined;
            addedData = addedData != '' ? JSON.parse(addedData) : undefined;
            if (BackAndForthNavMethods.IsWorkFlowStarted()) {
                var isinputValid = validateData ? checknoncomplusaryfield():true;
                if (isinputValid) {
                    if ((existingData != null && addedData != null)) {
                        if (JSON.stringify(existingData) != JSON.stringify(addedData)) {
                            if (callback != '' && callback != undefined && callback != null) {
                                callback();
                            } else {
                                ShowDialogWarningPop("There are unsaved changes. Do you want to save?", "Save & Continue", "Discard Changes & Continue", "SaveSupplymentaryDataOnForthNav",
                                    "UnSaveSupplymentaryDataOnForthNav", 0, false, NavStep);
                            }
                            return false;
                        } else if (NavigateToLink != '' && NavigateToLink != null) {
                            location.href = NavigateToLink;
                        }
                    } else if (NavigateToLink != '' && NavigateToLink != null) {
                        location.href = NavigateToLink;
                    }
                }
            }
        } else if (StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY && BackAndForthNavMethods.IsNotification() == true && NavigateToLink != '' && NavigateToLink != null) {
            location.href = NavigateToLink;
        }
        return isValidForNav;
    },
    OverviewNav: function (NavStep, IsBack = false, NavigateToLink = '', callback = '') {
        var isValidForNav = true;
        if (StepFlag == NavigationEnum.OVERVIEW) {
            isMovementTypeHasMajorChanges = localStorage.getItem('isMovementTypeHasMajorChanges');
            if (isMovementTypeHasMajorChanges != 2)
                localStorage.removeItem('isMovementTypeHasMajorChanges');
            var existingData = GetLocalObject(StepFlag, false);
            var addedData = CreateLocalObject(StepFlag, false);
            existingData = existingData != '' ? JSON.parse(existingData) : undefined;
            addedData = addedData != '' ? JSON.parse(addedData) : undefined;
            if (BackAndForthNavMethods.IsWorkFlowStarted()) {
                if ((existingData != null && addedData != null)) {
                    if (JSON.stringify(existingData) != JSON.stringify(addedData)) {
                        if (callback != '' && callback != undefined && callback != null) {
                            callback();
                        } else {
                            ShowDialogWarningPop('There are unsaved changes. Do you want to save?', 'Save & Continue', 'Discard Changes & Continue', 'SaveOverviewOnForthNav', 'UnSaveOverviewOnForthNav', 1, '', NavStep, IsBack);
                        }
                        return false;
                    } else if (NavigateToLink != '' && NavigateToLink != null) {
                        location.href = NavigateToLink;
                    }
                } else if (NavigateToLink != '' && NavigateToLink != null) {
                    location.href = NavigateToLink;
                }
            }
        }
        return isValidForNav;
    },
    MovementTypeNavBack: function (NavStep) {
        var isValidForNav = true;
        if (StepFlag == NavigationEnum.MOVEMENT_TYPE) {
            var existingData = GetLocalObject(StepFlag, false);
            var addedData = CreateLocalObject(StepFlag, false);
            existingData = existingData != '' ? JSON.parse(existingData) : undefined;
            addedData = addedData != '' ? JSON.parse(addedData) : undefined;
            if (BackAndForthNavMethods.IsWorkFlowStarted()) {
                if (MovementTypeValidation()) {
                    //check movent type or date changes, then user should move forward using below button

                    if ((existingData != null && addedData != null) && BackAndForthNavMethods.IsNotification() == false &&
                        (existingData.ApplicationRadio != addedData.ApplicationRadio || existingData.NotificationRadio != addedData.NotificationRadio)) {
                        isValidForNav = false;
                    } else if ((existingData != null && addedData != null) && BackAndForthNavMethods.IsNotification() == true &&
                        (existingData.FromDate != addedData.FromDate || existingData.ToDate != addedData.ToDate
                        || existingData.ApplicationRadio != addedData.ApplicationRadio || existingData.NotificationRadio != addedData.NotificationRadio)) {
                        isValidForNav = false;
                    } else if ((existingData != null && addedData != null)) {
                        //check other values has difference
                        if (JSON.stringify(existingData) != JSON.stringify(addedData)) {
                            ShowDialogWarningPop('There are unsaved changes. Do you want to save?', 'Save & Continue', 'Discard Changes & Continue', 'SaveMovementTypeDataOnForthNav', 'UnSaveMovementTypeDataOnForthNav', 1, '', NavStep, true);
                            return false;
                        }
                    }
                }
            } else if ((existingData != null && addedData != null)) {
                CreateLocalObject(StepFlag, true);
            }
        }
        return isValidForNav;
    },
    SupplymentaryInfoNavBack: function (NavStep, IsBack = false) {
        var isValidForNav = true;
        if (StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY && BackAndForthNavMethods.IsNotification() == false) {
            if (BackAndForthNavMethods.IsWorkFlowStarted() && $('#supplimentaryinfo').length > 0 && !checknoncomplusaryfield()) {//edit mode
                return true;
            }

            var existingData = GetLocalObject(StepFlag, false);
            var addedData = CreateLocalObject(StepFlag, false);
            existingData = existingData != '' ? JSON.parse(existingData) : undefined;
            addedData = addedData != '' ? JSON.parse(addedData) : undefined;
            if (BackAndForthNavMethods.IsWorkFlowStarted()) {
                if (checknoncomplusaryfield()) {
                    if ((existingData != null && addedData != null)) {
                        if (JSON.stringify(existingData) != JSON.stringify(addedData)) {
                            ShowDialogWarningPop('There are unsaved changes. Do you want to save?', 'Save & Continue', 'Discard Changes & Continue', 'SaveSupplymentaryDataOnForthNav', 'UnSaveSupplymentaryDataOnForthNav', 1, '', NavStep, IsBack);
                            return false;
                        }
                    }
                }
            }
        }
        return isValidForNav;
    },
    IsWorkFlowStarted: function () {
        return ($('#AppRevisionId').val() != undefined && $('#AppRevisionId').val() != '' && $('#AppRevisionId').val() != '0') ||
            ($('#NotificationId').val() != undefined && $('#NotificationId').val() != '' && $('#NotificationId').val() != '0');
    },
    IsNotification: function () {
        return $('#IsNotif').val() == 'true' || $('#IsNotif').val() == 'True';
    }
};

function SaveCheckVehicleOnForthNav(NavStep, IsBack = false) {
    //if yes save the values and continue to next step
    CloseWarningPopupRef();
    IsVehicleComponentTypeChanged = false;
    isVehicleHasChanged = false;
    NavigationForWorkflowSequenceOnTopNavClick(NavStep, true, false, false);
}
function SaveRouteDataOnForthNav(NavStep, IsBack = false, NavigateToLink = '') {
    if (NavigateToLink != '' && NavigateToLink != null) {
        location.href = NavigateToLink;
    }
}
function SaveMovementTypeDataOnForthNav(NavStep, IsBack = false,NavigateToLink='') {
    //if yes save the values and continue to next step
    NavigationMethods.MovementTypeConfirm(NavigateToNextPage = false, NavigateToLink);
    if (NavigateToLink == '' || NavigateToLink == null) {
        RemoveLocalObject(NavigationEnum.MOVEMENT_TYPE);
        RemoveLocalObject(NavigationEnum.OVERVIEW);
        CloseWarningPopupRef();
        CloseWarningPopupDialog();
        if (IsBack) {
            OnBackButtonClick(NavStep, true);
        } else {
            NavigationForWorkflowSequenceOnTopNavClick(NavStep, false, false);
        }
    }
}
function UnSaveMovementTypeDataOnForthNav(NavStep, IsBack = false, NavigateToLink = '') {
    RemoveLocalObject(NavigationEnum.MOVEMENT_TYPE);
    RemoveLocalObject(NavigationEnum.OVERVIEW);
    CloseWarningPopupRef();
    CloseWarningPopupDialog();
    if (NavigateToLink == ''||NavigateToLink == null) {
        if (IsBack) {
            OnBackButtonClick(NavStep, true,true);
        } else {
            NavigationForWorkflowSequenceOnTopNavClick(NavStep, false, false);
        }
    } else if (NavigateToLink != '' && NavigateToLink != null){
        location.href = NavigateToLink;
    }
}
function SaveSupplymentaryDataOnForthNav(NavStep, IsBack = false, NavigateToLink = '') {
    //if yes save the values and continue to next step
    SaveSupplimentaryInfo(showPopUp = false, NavigateToLink);
    if (NavigateToLink == ''||NavigateToLink == null) {
        RemoveLocalObject(NavigationEnum.ROUTEASSESSMNT_SUPPLY);
        RemoveLocalObject(NavigationEnum.OVERVIEW);
        CloseWarningPopupRef();
        CloseWarningPopupDialog();
        if (IsBack) {
            OnBackButtonClick(NavStep, true);
        } else {
            NavigationForWorkflowSequenceOnTopNavClick(NavStep, false, false);
        }
    }
}
function UnSaveSupplymentaryDataOnForthNav(NavStep, IsBack = false, NavigateToLink = '') {
    RemoveLocalObject(NavigationEnum.ROUTEASSESSMNT_SUPPLY);
    RemoveLocalObject(NavigationEnum.OVERVIEW);
    CloseWarningPopupRef();
    CloseWarningPopupDialog();
    if (NavigateToLink == ''||NavigateToLink == null) {
        if (IsBack) {
            OnBackButtonClick(NavStep, true);
        } else {
        NavigationForWorkflowSequenceOnTopNavClick(NavStep, false, false);
        }
    } else if (NavigateToLink != '' && NavigateToLink != null){
        location.href = NavigateToLink;
    }
}
function SaveOverviewOnForthNav(NavStep, IsBack = false, NavigateToLink = '') {
    //if yes save the values and continue to next step
    onApply(IsCompleted = false,NavigateToLink);
    RemoveLocalObject(NavigationEnum.OVERVIEW);
    CloseWarningPopupRef();
    CloseWarningPopupDialog();
    if (NavigateToLink == ''||NavigateToLink == null) {
        if (IsBack) {
            OnBackButtonClick(NavStep, true);
        } else {
            NavigationForWorkflowSequenceOnTopNavClick(NavStep, false, false, false, false);
        }
    }
}
function UnSaveOverviewOnForthNav(NavStep, IsBack = false, NavigateToLink = '') {
    RemoveLocalObject(NavigationEnum.OVERVIEW);
    CloseWarningPopupRef();
    CloseWarningPopupDialog();
    if (NavigateToLink == ''||NavigateToLink == null) {
        if (IsBack) {
            OnBackButtonClick(NavStep, true);
        } else {
            NavigationForWorkflowSequenceOnTopNavClick(NavStep, false, false, false, false);
        }
    } else if (NavigateToLink != '' && NavigateToLink != null){
        location.href = NavigateToLink;
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
    }
}
function onApply(IsCompleted = true, NavigateToLink = '') {
    if (BackAndForthNavMethods.IsNotification()) {
        ValidateGeneralDetails(IsCompleted,NavigateToLink);
    }
    else {
        OverViewValidation(IsCompleted,NavigateToLink);
    }
}
function RedirectToMovementInbox() {
    if (userTypeIDVal == 696008) {
        window.location.href = '/SORTApplication/SORTInbox';
    }
    else {
        window.location.href = '/Movements/MovementList';
    }
}
function RedirectToNotificationOverview(notificationId, notificationCode) {
    window.location.href = '/Notification/DisplayNotification' + EncodedQueryString('notificationId=' + notificationId + '&notificationCode=' + notificationCode);
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
            ApplicationSupplimentaryInfoInit();
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
        $('#backbutton').show();
        CloseSuccessModalPopup();
        LoadContentForAjaxCalls("POST", '../Application/ViewSupplementary', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section', '', function () {
            ViewSupplementaryInit();
        });
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
            $('#viewsupplimentary').show();
            $('#viewsupplimentary').html('');
            $('#viewsupplimentary').html(response);
            $('#btnEditSupplementary').show();
            $('#btnSaveSupplementary').hide();
            $('#back_btn').show();
            $('#save_btn').hide();
            $('#confirm_btn').show();
            $('#backbutton').show();
            ViewSupplementaryInit();
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
    $('#back_btn_Altered').hide();
    $('#select_route_section_agredRt').hide();
    $('#route_assessment_next_btn').hide();
    $('#divRouteActionsContainer').hide();
}
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
        data: {
            vehicleIds: SelectedVehicles, notificationId: NotificationIdVal
        },
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
function ConfirmFromMapPage() {
    CloseWarningPopupRef();
    VehicleAssigned = Boolean($('#hf_VehicleAssignedGlobal').val().toLowerCase() == 'true');
    if (!AutoAssignRouteVehicle && !VehicleAssigned) {
        $('#IsRouteSummary_Flag').val(1);
        ShowDialogWarningPop('All vehicles have not been assigned to routes. Please view the Manage Routes page.', 'Cancel', 'Manage Routes', 'CloseWarningPopupDialog', 'RedirectToRouteListNew');
    }
    else {
        OnConfirm();
    }
}
function CheckBrokenRouteExistBeforeSubmit(callBackFn) {
    var isApp = $('#hf_IsApp').val();
    var isNotif = $('#IsNotif').length > 0 ? $('#IsNotif').val() : $('#hf_IsNotif').val();
    var isVr1App = $('#hf_IsVr1App').val();
    var appRevId = $('#AppRevisionId').val() || 0;
    var contrefno = $('#CRNo').val() == undefined ? $('#VR1ContentRefNo').val() : $('#CRNo').val();
    var versionid = $('#AppVersionId').val();
    var input;
    if (isApp.toLowerCase() == 'true') {
        input = { AppRevisonId: appRevId, IsReplanRequired: false };
    } else if (isNotif.toLowerCase() == 'true') {
        input = { ConteRefNo: contrefno, IsReplanRequired: false };
    } else if (isVr1App.toLowerCase() == 'true') {
        input = { VersionId: versionid, IsReplanRequired: false };
    }
    //If no callback function is there, we just need to return the input data and the broken check will be handled there(we may need to pass params and it will be difficult for that)
    if (typeof callBackFn != 'undefined' && callBackFn != null && callBackFn != "") {
        CheckIsBroken(input, function (response) {
            if (response && response.brokenRouteCount > 0) {//Check broken route exist and show message
                var msg = "";
                if (isNotif.toLowerCase() == 'true') { // notification
                    msg = BROKEN_ROUTE_MESSAGES.PLAN_MOV_ON_MAP_PAGE_CONFIRM;
                }
                else {//application
                    msg = isVr1App.toLowerCase() == 'true' ? BROKEN_ROUTE_MESSAGES.PLAN_MOV_ON_MAP_PAGE_CONFIRM_VR1_APPLICATION: BROKEN_ROUTE_MESSAGES.PLAN_MOV_ON_MAP_PAGE_CONFIRM_APPLICATION;
                }
                ShowWarningPopupMapupgarde(msg, function () {
                    $('#WarningPopup').modal('hide');
                });
                return false;
            } else {
                if (typeof callBackFn != 'undefined' && callBackFn != null && callBackFn != "") {
                    callBackFn();
                }
            }
        });
    } else {
        return input;
    }
}

function RedirectToRouteListNew() {
    $('#IsRouteSummary_Flag').val(1);
    $('#pop-warning').modal('hide');
    CloseWarningPopupRef();
    RedirectToRouteList();
    //isUnsavedChange = true;
}
function RedirectToNavigateFlow(stepFlag, backButton = true) {
    CloseWarningPopupDialog();
    CloseWarningPopupRef();
    if (StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY) {
        var IsSupplimentaryAlreadySaved = $('#hf_IsSupplimentarySaved').val();
        if (IsSupplimentaryAlreadySaved == "true" || IsSupplimentaryAlreadySaved == "True") {
            SaveSupplimentaryInfo(showPopUp = false);
        } else {
            CreateLocalObject(StepFlag);
        }
    } else if (StepFlag == NavigationEnum.MOVEMENT_TYPE) {
        var isLocalObjExist = CheckLocalStorageValExist(NavigationEnum.MOVEMENT_TYPE);
        if (!isLocalObjExist)
            CreateLocalObject(StepFlag);
        else if (BackAndForthNavMethods.IsWorkFlowStarted() == false)
            CreateLocalObject(StepFlag);
    } else {
        CreateLocalObject(StepFlag);
    }
    if (backButton)
        OnBackButtonClick(stepFlag);
    else
        NavigationForWorkflowSequenceOnTopNavClick(stepFlag);
}
function PlanMoveNavigateFlow(index, backButton = true, checkUnsaved = true) {   

    var navigatefunctionName = 'NavigationForWorkflowSequenceOnTopNavClick';
    if (backButton) {
        navigatefunctionName = 'OnBackButtonClick';
    }

    checkUnsaved = checkUnsaved && StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY ? false : checkUnsaved;
    if (StepFlag == NavigationEnum.ROUTEASSESSMNT_SUPPLY && BackAndForthNavMethods.IsWorkFlowStarted() && $('#supplimentaryinfo').length > 0 && !checknoncomplusaryfield()) {//checknoncomplusaryfield
        //showToastMessage({
        //    message: "Please fill required fields.",
        //    type: "error"
        //});
        //return false;
        checkUnsaved = false;
    }
    checkUnsaved = checkUnsaved && (StepFlag == NavigationEnum.HAULIER_DETAILS && $('#form_Organisation #desc-entry').length <= 0) ? false : checkUnsaved;

    checkUnsaved = checkUnsaved && (index > StepFlag && StepFlag == NavigationEnum.MOVEMENT_TYPE) ? false : checkUnsaved;
    if (BackAndForthNavMethods.IsWorkFlowStarted()) {
        if (backButton && StepFlag == NavigationEnum.MOVEMENT_TYPE) {
            var isUnsavedMT = CompareLocalObject(StepFlag);
            if (isUnsavedMT) {
                ShowWarningPopup('There are unsaved changes. Do you want to continue without saving?', 'OnBackButtonClick', 'CloseWarningPopupRef', index,true,true);
                return false;
            }
        }
        checkUnsaved = checkUnsaved && (StepFlag == NavigationEnum.MOVEMENT_TYPE) ? false : checkUnsaved;
    } else {
        checkUnsaved = checkUnsaved && (StepFlag == NavigationEnum.MOVEMENT_TYPE) ? checkUnsaved : false;
    }
    if (StepFlag == NavigationEnum.MOVEMENT_TYPE && BackAndForthNavMethods.IsWorkFlowStarted() && !MovementTypeValidation()) {
        //showToastMessage({
        //    message: "Please fill required fields in movement type details.",
        //    type: "error"
        //});
        //return false;
        console.log('Please fill required fields in movement type details.');
        checkUnsaved = false;
    }

    if (backButton && StepFlag == NavigationEnum.HAULIER_DETAILS && BackAndForthNavMethods.IsWorkFlowStarted() == false && $('#ddlHaulierContactName').length > 0 && $('#ddlHaulierContactName').is(':visible')) {
        ShowWarningPopup('There are unsaved changes. Do you want to continue without saving?', 'NavigateToHaulierSelectMainPage', 'CloseWarningPopupRef');
        return false;
    }

    var isUnsavedChangeMap = ValidateUnSavedChange("map") || CheckMapInputHasChanges();
    if (StepFlag == NavigationEnum.ROUTE_DETAILS && BackAndForthNavMethods.IsWorkFlowStarted() && isUnsavedChangeMap &&
        SubStepFlag == 0) {
        if (backButton)
            ShowWarningPopup('There are unsaved changes. Do you want to continue without saving?', 'OnBackButtonClick', 'CloseWarningPopupRef', index,true);
        else
            ShowWarningPopup('There are unsaved changes. Do you want to continue without saving?', 'UnSaveRouteAndProceed', 'CloseWarningPopupRef', index);
        return false;
    }

    if (StepFlag == NavigationEnum.ROUTE_DETAILS && BackAndForthNavMethods.IsWorkFlowStarted() == true && typeof isUnsavedRouteExist != 'undefined' && isUnsavedRouteExist &&
        SubStepFlag == 0) {
        if (backButton)
            ShowWarningPopup('There are unsaved changes. Do you want to continue without saving?', 'OnBackButtonClick', 'CloseWarningPopupRef', index,true);
        else
            ShowWarningPopup('There are unsaved changes. Do you want to continue without saving?', 'UnSaveRouteAndProceed', 'CloseWarningPopupRef', index);
        return false;
    }

    checkUnsaved = checkUnsaved && (StepFlag == NavigationEnum.OVERVIEW) ? false : checkUnsaved;


    var isUnsaved = CompareLocalObject(StepFlag);
    if (checkUnsaved && isUnsaved) {
        ShowDialogWarningPop('There are unsaved changes. Do you want to save?', 'Save & Continue', 'Discard Changes & Continue', 'RedirectToNavigateFlow', navigatefunctionName, 1, '', index, backButton, true);
    }
    else {
        if (backButton)
            OnBackButtonClick(index);
        else
            NavigationForWorkflowSequenceOnTopNavClick(index);
    }
}
function UnSaveRouteAndProceed(nextStep) {
    CloseWarningPopupRef();
    NavigationForWorkflowSequenceOnTopNavClick(nextStep,false, true, true, true, false);
    isUnsavedRouteExist = false;
}
function NavigateToHaulierSelectMainPage() {
    RemoveLocalObject(NavigationEnum.HAULIER_DETAILS);
    CloseWarningPopupRef();
    $('#ddlHaulierContactName').remove();
    PlanMoveNavigateFlow(NavigationEnum.HAULIER_DETAILS);
}
function NavigateToRouteAssessmnetSupply() {
    if ($('#IsNotif').val() == 'true' || $('#IsNotif').val() == 'True') {
        AfStArray = [];
        AfConArray = [];
        $('#IsRouteAssessment').val(true);
        LoadContentForAjaxCalls("POST", '../Notification/NotificationRouteAssessment', { workflowProcess: 'HaulierApplication' }, '#route_assessment_section', '', function () {
            NotificationRouteAssessmentInit();
        });
    }
    else {
        if (IsSupplimentarySaved == 'True') {
            LoadContentForAjaxCalls("POST", '../Application/ViewSupplementary', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section', '', function () {
                ViewSupplementaryInit();
            });
        }
        else {
            LoadContentForAjaxCalls("POST", '../Application/ApplicationSupplimentaryInfo', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section', '', function () {
                ApplicationSupplimentaryInfoInit();
            });
        }
    }
}
function LeavePage(href, isLogout, _this) {
    if (href == null || isLogout)
        location.href = '../Account/LogOut';
    else if (href != '#')
        window.location = href;
}
function CheckSaveAndLeavePageOnHeaderLinkClick(href, isLogout, _this) {
    switch (StepFlag) {
        case 0: //haulier details page

            break;
        case 1: //select vehicle page
            break;
        case 2: //vehicle details page
            if (isVehicleHasChanged) {
                ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "LeavePage", "CloseWarningPopupRef", href, isLogout, _this);
                return false;
            } else {
                if (href == null || isLogout)
                    href = '../Account/LogOut';
                location.href = href;
            }
            break;
        case 3: //mvmnt confirmtn page'
            var existingData = GetLocalObject(StepFlag, false);
            var addedData = CreateLocalObject(StepFlag, false);
            existingData = existingData != '' ? JSON.parse(existingData) : undefined;
            addedData = addedData != '' ? JSON.parse(addedData) : undefined;
            if (JSON.stringify(existingData) != JSON.stringify(addedData)) {
                var message = 'There are unsaved changes. Do you want to save?';
                ShowDialogWarningPop(message, "Save & Continue", "Discard Changes & Continue", "SaveMovementTypeDataOnForthNav", "UnSaveMovementTypeDataOnForthNav", 0, false, 0, false, href);
                return false;
            } else {
                if (href == null || isLogout)
                    href = '../Account/LogOut';
                location.href = href;
            }
            break;
        case 4: //route details page
            if (typeof isUnsavedRouteExist != 'undefined' && isUnsavedRouteExist) {
                ShowWarningPopup('There are unsaved changes. Do you want to continue without saving?', 'LeavePage', 'CloseWarningPopupRef', href, isLogout);
                return false;
            }
            var isUnsavedChange = ValidateUnSavedChange("map") || CheckMapInputHasChanges();
            if (isUnsavedChange) {
                ShowWarningPopup('There are unsaved changes. Do you want to continue without saving?', 'LeavePage', 'CloseWarningPopupRef', href, isLogout);
                return false;
            }
            if (href == null || isLogout)
                href = '../Account/LogOut';
            location.href = href;
            break;
        case 5:
            if ($('#TotalDistanceOfRoad').length > 0) {
                if (isLogout)
                    href = '../Account/LogOut';
                BackAndForthNavMethods.SupplymentaryInfoNav(StepFlag, false, href, function () {
                    var message = 'There are unsaved changes. Do you want to save?';
                    ShowDialogWarningPop(message, "Save & Continue", "Discard Changes & Continue", "SaveSupplymentaryDataOnForthNav", "UnSaveSupplymentaryDataOnForthNav", 0, false, 0, false, href);
                },false);
            } else {
                if (href==null || isLogout)
                    href = '../Account/LogOut';
                location.href = href;
            }
            break;
        case 6:
            //overview page
            if (isLogout)
                href = '../Account/LogOut';
            BackAndForthNavMethods.OverviewNav(StepFlag, false, href, function () {
                var message = 'There are unsaved changes. Do you want to save?';
                ShowDialogWarningPop(message, "Save & Continue", "Discard Changes & Continue", "SaveOverviewOnForthNav", "UnSaveOverviewOnForthNav", 0, false, 0, false, href);
            });
            break;
    }
}

var mapInputValuesGlobal = [];
function CheckMapInputHasChanges() {
    var oldValString = JSON.stringify(mapInputValuesGlobal);
    var newInputVals = getMapInputValues();
    var newValString = JSON.stringify(newInputVals);
    if ($('#map').length > 0 && oldValString != newValString) {
        return true;
    }
    return false;
}

function getMapInputValues() {
   return Array.from($('#route-content-2 :input[type="text"]:visible').map(function () {
        return $(this).val() == '' ? null : $(this).val();;
    }));
}
function GetVehicleClassErroDescription(vehicleClass, moveType) {
    var classDesc = null;
    switch (parseInt(vehicleClass)) {
        case 241001:
            classDesc = "Vehicle Special Order";
            break;
        case 241002:
            classDesc = "Special Order";
            break;
        case 241003:
            classDesc = "Stgo-ail Cat1";
            break;
        case 241004:
            classDesc = "Stgo-ail Cat2";
            break;
        case 241005:
            classDesc = "Stgo-ail Cat3";
            break;
        case 241006:
            classDesc = "stgo mobile crane cat a";
            break;
        case 241007:
            classDesc = "stgo mobile crane cat b";
            break;
        case 241008:
            classDesc = "stgo mobile crane cat c";
            break;
        case 241009:
            classDesc = "stgo engineering plant";
            break;
        case 241010:
            classDesc = "stgo road recovery vehicle";
            break;
        case 241011:
            classDesc = "Wheeled construction and use";
            break;
        case 241012:
            classDesc = "Tracked";
            break;
        case 241013:
            classDesc = "stgo engineering plant wheeled";
            break;
        case 241014:
            classDesc = "stgo engineering plant tracked";
            break;
        case 241015:
            classDesc = "No Vehicle Classification";
            break;
        case 241016:
            classDesc = "No Crane";
            break;
        case 241017:
            classDesc = "stgo cat 1 engineering plant wheeled";
            break;
        case 241018:
            classDesc = "stgo cat 2 engineering plant wheeled";
            break;
        case 241019:
            classDesc = "stgo cat 3 engineering plant wheeled";
            break;
        case 241020:
            classDesc = "stgo cat 1 engineering plant tracked";
            break;
        case 241021:
            classDesc = "stgo cat 2 engineering plant tracked";
            break;
        case 241022:
            classDesc = "stgo cat 3 engineering plant tracked";
            break;
        case 241023:
            classDesc = "stgo cat 1 road recovery";
            break;
        case 241024:
            classDesc = "stgo cat 2 road recovery";
            break;
        case 241025:
            classDesc = "stgo cat 3 road recovery";
            break;
        case 241026:
            classDesc = "stgo vr-1 cat 1 engineering plant wheeled";
            break;
        case 241027:
            classDesc = "stgo vr-1 cat 2 engineering plant wheeled";
            break;
        case 241028:
            classDesc = "stgo vr-1 cat 3 engineering plant wheeled";
            break;
        case 241029:
            classDesc = "stgo vr-1 cat 1 engineering plant tracked";
            break;
        case 241030:
            classDesc = "stgo vr-1 cat 2 engineering plant tracked";
            break;
        case 241031:
            classDesc = "stgo vr-1 cat 3 engineering plant tracked";
            break;
        case 241032:
            classDesc = "stgo vr-1 cat 1 road recovery";
            break;
        case 241033:
            classDesc = "stgo vr-1 cat 2 road recovery";
            break;
        case 241034:
            classDesc = "stgo vr-1 cat 3 road recovery";
            break;
        default:
            classDesc = null;
            break;
    }
    if (moveType == 207002) {
        classDesc = "VR1(" + classDesc + ")";
    }
    if (classDesc != null)
        return classDesc.toUpperCase();
    else
        return classDesc;
}

function showImminentMovementBanner(moveStartDate, bannerContainer) {
    var bannerId = bannerContainer + " #imminentBanner";
    var bannertxtId = bannerId + " #imminentBannerMsg";
    var isNotification = true;
    var mvmtTypeRadioVisible = $('input[name="applicationType"]').is(":visible");
    if (mvmtTypeRadioVisible) {
        var mvmtType = $('input[name="applicationType"]:checked').val();
        if (mvmtType == 207001)
            isNotification = false;
    } else if ($('#pm_movementType').val() == "207003") {
        isNotification = true;
    } else if (BackAndForthNavMethods.IsNotification()==false) {
        isNotification = false;
    }
    var vehicleclass = $('#pm_vehicleClass').val();
    var NotifID = $('#NotificationId').val();
    ContentRefno = $('#CRNo').val();
    var imminentMsg = '';   
    if (vehicleclass != VEHICLE_CLASSIFICATION_TYPE_CONFIG.VEHICLE_SPECIAL_ORDER && isNotification) {
        $.ajax
            ({
                type: "POST",
                url: "../Notification/ShowImminentMovement",
                data: { moveStartDate: moveStartDate, contentRefNo: ContentRefno, notificationId: NotifID, vehicleClass: vehicleclass },
                beforeSend: function () {
                    openContentLoader(bannerId);
                },
                success: function (data) {
                    var ids = data.result.strContryID;//country containing the route
                    //1 - Imminent movement.
                    //2 - Imminent movement for police.
                    //3 - Imminent movement for SOA.
                    //4 - Imminent movement for SOA and police.
                    //5 - No imminent movement.

                    if (data.result.imminentStatus == 5) {
                        $(bannerId).fadeOut();
                        $(bannertxtId).text('');
                    }
                    else {
                        if (data.result.imminentStatus == 1) { imminentMsg = "Imminent movement." }
                        else if (data.result.imminentStatus == 2) { imminentMsg = "Imminent movement for police." }
                        else if (data.result.imminentStatus == 3) { imminentMsg = "Imminent movement for SOA." }
                        else if (data.result.imminentStatus == 4) { imminentMsg = "Imminent movement for SOA and police." }

                        $(bannertxtId).text(imminentMsg);
                        $(bannerId).fadeIn();
                    }
                },
                error: function (xhr, status, error) {
                    console.error("error", error);
                },
                complete: function () {
                    closeContentLoader(bannerId);
                }
            });
    }
    else {
        $(bannerId).fadeOut();
        $(bannertxtId).text('');
    }
}