var componentIdsVal;
var isVehicleHasChanged = false;
function VehicleConfigCreateVehicleInit() {
    LoadSelectVehicleComponentV1();
    componentIdsVal = $('#hf_ComponentIds').val();
    //startAnimation();
    if (componentIdsVal != undefined && componentIdsVal != null) {
        componentIdsVal = JSON.parse(componentIdsVal);
        for (var i = 0; i < componentIdsVal.length; i++) {
            var id = componentIdsVal[i];
            ImportComponentToConfig(id);
        }
        isPlanMovementEditStarted = false;
    } else {
        isPlanMovementEditStarted = false;
    }

    //stopAnimation();
}
$(document).ready(function () {
    if ($('#hf_IsPlanMovmentGlobal').length == 0) {//To check the page not loaded from plan movement
        SelectMenu(5);
        VehicleConfigCreateVehicleInit();
    }
   
    $('body').on('click', '#back_btn_inVehiclePage', function (e) {
        e.preventDefault();
        if (isVehicleHasChanged && $('#hf_IsPlanMovmentGlobal').length > 0) {
            ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "BackButtonVehiclePagefn", "CloseWarningPopupRef", this);
        } else {            
            BackButtonVehiclePagefn(this);
        }
    });
    $('body').on('click', '#vehicle_back_btn', function (e) {
        e.preventDefault();
        SaveVehicleTempData();//Save entered data to local storage

        //if ($('#hf_IsPlanMovmentGlobal').length > 0) {//isVehicleHasChanged &&
        //    ////ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "PreviousPage", "CloseWarningPopupRef", this);
        //    //ShowDialogWarningPopWithCallback('There are unsaved changes. Do you want to save?', 'Save & Continue', 'Discard Changes & Continue',
        //    //    function () {
                    
                    
        //    //    }, function () {
        //    //        CloseWarningPopupRef();
        //    //        CloseWarningPopupDialog();
        //    //        PreviousPage(this);
        //    //    }, 1, '', this);
        //} else {
        //    PreviousPage(this);
        //}
    });

    function SaveVehicleTempData() {
        var vehicleData = {};
        vehicleData.configDetails = GetDivObjectJson('#div_config_general', false);
        vehicleData.configTypeId = $('#ConfigTypeId').val();
        vehicleData.registrationDetails = [];
        $('#div_reg_config_vehicle .tbl_registration tbody tr').each(function () {
            var regObj = {};
            regObj.RegNo = $(this).find('input.txt_register_config').length > 0 ? $(this).find('input.txt_register_config').val() : $(this).find('.cls_regId').text();
            regObj.FleetNo = $(this).find('input.txt_fleet_config').length > 0 ? $(this).find('input.txt_fleet_config').val() : $(this).find('.cls_fleetId').text();
            if (regObj.RegNo != '' || regObj.FleetNo != '')
                vehicleData.registrationDetails.push(regObj);
        });
        vehicleData.AxlesArr = JSON.parse(JSON.stringify(AxleTableMethods.AxlesArr));
        vehicleData.componentIds = [];
        vehicleData.components = [];
        //component-item-axle-section
        $(".comp").each(function (index) {
            var compId = $(this).attr('id');
            vehicleData.componentIds.push(compId);
            var compData = { compId: compId };
            compData.componentData = GetDivObjectJson(".comp#" + compId, false);
            //AxleTableMethods.CreateAxlesObj(compId);

            compData.registrationDetails = [];
            $(this).find('.div_reg_component_vehicle .tbl_registration tbody tr').each(function () {
                var regObj = {};
                regObj.RegNo = $(this).find('input.txt_register').length > 0 ? $(this).find('input.txt_register').val() : $(this).find('.cls_regId').text();
                regObj.FleetNo = $(this).find('input.txt_fleet').length > 0 ? $(this).find('input.txt_fleet').val() : $(this).find('.cls_fleetId').text();
                if (regObj.RegNo != '' || regObj.FleetNo != '')
                    compData.registrationDetails.push(regObj);
            });

            vehicleData.components.push(compData);
        }).promise().done(function () {
            localStorage.setItem('ComponentTempData', JSON.stringify(vehicleData));
            CloseWarningPopupRef();
            CloseWarningPopupDialog();
            PreviousPage(this);
        });
    }
    
    var vehicleConfigAssessmentBtnCLicked = false;
    $('body').on('click', '#vehicle_config_assessment_btn', function (e) {
        e.preventDefault();
        if (typeof AxleTableMethods != 'undefined') {
            AxleTableMethods.AxlesArr = [];
        }
        vehicleConfigAssessmentBtnCLicked = true;        
        VehicleConfigPage(this);        
    });
    //$('body').on('click', '#vehicle_save_btn', function (e) {
    //    e.preventDefault();
    //    SaveVehicle(this);
    //});
    $('body').on('click', '#scoll-right-btn', function (e) {
        e.preventDefault();
        $('#widgetsContent').animate({ scrollLeft: "+=300px" }, "medium");
    });
    $('body').on('click', '#scoll-left-btn', function (e) {
        e.preventDefault();
        $('#widgetsContent').animate({ scrollLeft: "-=300px" }, "medium");
    });

    $('body').on('click', '.openCompFilter', function () { window['openFilterInConfig'](); });
    $('body').on('click', '.btnClearFleet', function () { window['clearComponentSearch'](); });
    $('body').on('click', '.closeComponentFilter', function () { window['closeFilters'](); });
    $('body').on('click', '.closeCompFilter', function () { window['clearComponentSearch'](); });
    $('body').on('click', '.btnFleetSearch', function () { window['SearchVehicleComponentInConfig'](); });

    $('body').on('click', '.useComponent', function (e) {
        e.preventDefault();
        ImportFleetComponent(this);
    });
});

function ImportFleetComponent(e) {
    var componentId = $(e).attr("componentid");
    ImportComponentToConfig(componentId,true);
}