    SelectMenu(5);

    $(document).ready(function () {

        //loadGeneralPage(270002, 244002);
        showNextButton($('#ComponentCount').val());
        LoadSelectVehicleComponent();
        
        @if (IsEdit != 1) {            
            @:SubStepFlag = 2.1;
        }

        $(".btn_ComponentEdit").on('click', EnableEditComponentFn);
        $(".btn_comp_save").on('click', EditVehicleComponentFn); 
        $(".btn_deleteComponent").on('click', DeleteConfigComponentFn); 
        $(".btn_ComponentAddToFleet").on('click', AddToFleetFn);
        $(".btn_comp_cancel").on('click', CancelClick);
        $("#btn_Next").on('click', LoadConfigDetailsFn);
        
    });


    function EnableEditComponent(divname) {
        $('#divComponentData-' + divname).removeClass("divComponentData");
        $('#btn_ComponentEdit-' + divname).hide();
        $('.btn_comp_finish-' + divname).show();
        $('btn_ComponentDelete - ' + divname).hide();
        $('#btn_ComponentAddToFleet-' + divname).hide();
        $('#btn_comp_cancel-' + divname).show();
    }


    function LoadSelectVehicleComponent() {
        var IsCandidate = false;
        if ($('#IsCandVersion').val() == "True") {
            IsCandidate = true;
        }
        $.ajax({
            type: "POST",
            url: '../VehicleConfig/SelectVehicleComponent',
            data: { isCandidate: IsCandidate},
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                $('#selectComponent').html('');
                $('#selectComponent').html(response);
            },
            error: function (result) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }

    function CancelClick() {
        if ($('div').find('#vehicle_Create_section').length != 0) {
            var vehicleId = $('#vehicleId').val();
            var GUID = $('#GUID').val();
            ShowWarningPopup('You have unsaved changes. Do you want to cancel?', "FillComponentDetailsForConfig('" + GUID + "','" + vehicleId + "')");            
        }
        else {
            location.reload();
        }
    }

    function OnBackBtnClick() {
        $.ajax({
            url: '../Vehicle/BackButtonToConfig',
            type: 'POST',
            success: function (response) {

                    if ($('div').find('#vehicle_Create_section').length != 0) {
                        $.ajax({
                            type: "GET",
                            url: '../VehicleConfig/CreateConfiguration',
                            data: { Guid: response, isMovement: true },
                            beforeSend: function () {
                                startAnimation();
                            },
                            success: function (response) {
                                $('#vehicle_Create_section').html('');
                                $('#vehicle_Create_section').html(response);
                                $('.createConfig').unwrap('#banner-container');
                                $('.createConfig').attr("style", "padding-left:0px !important");
                                $('#vehicle_Create_section').show();
                                SelectMenu(2);
                            },
                            error: function (result) {
                            },
                            complete: function () {
                                stopAnimation();
                            }
                        });
                    }
                    else {
                        window.location = "../vehicleConfig/CreateConfiguration" + EncodedQueryString("Guid=" + response);
                    }
                }

        });

    }

    function OnBackEditBtnClick() {
        $.ajax({
            url: '../Vehicle/BackButtonToConfig',
            type: 'POST',
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                
                var vehicleId = $('#vehicleConfigId').val();
                if (vehicleId == undefined || vehicleId == "") {
                    vehicleId = $('#vehicleId').val();
                }
                /*if (response != "") {*/
                if ($('div').find('#vehicle_Component_edit_section').length != 0) {
                    $.ajax({
                        type: "GET",
                        url: '../VehicleConfig/EditConfiguration',
                        data: { vehicleId: vehicleId, isApplication: true },
                        beforeSend: function () {
                            startAnimation();
                        },
                        success: function (response) {

                            $('#vehicle_edit_section').html('');
                            $('#vehicle_Component_edit_section').html('');
                            $('#vehicle_Component_edit_section').html(response);
                            $('#vehicle_Component_edit_section').show();
                            $('.createConfig').unwrap('#banner-container');
                            $('.createConfig').attr("style", "padding-left:0px !important");
                            SelectMenu(2);
                        },
                        error: function (result) {
                        },
                        complete: function () {
                            stopAnimation();
                        }
                    });
                }
                else {
                    window.location = "../vehicleConfig/EditConfiguration" + EncodedQueryString("vehicleId=" + vehicleId);
                }
            },
            complete: function () {
                stopAnimation();
            }

        });
    }

    function LoadCreateConfigPage() {
        var GUID = $('#GUID').val();
        $.ajax({
            type: "GET",
            url: '../VehicleConfig/CreateConfiguration',
            data: { Guid: GUID, isMovement: true },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                stopAnimation();
                SubStepFlag = 2.1;
                $('#vehicle_Create_section').html('');
                $('#vehicle_Create_section').html(response);
                $('.createConfig').unwrap('#banner-container');
                $('.createConfig').attr("style", "padding-left:0px !important");
                SelectMenu(2);
            },
            error: function (result) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }

    function EnableEditComponentFn(e) {
        var componentId = e.currentTarget.dataset.ComponentId;
        EnableEditComponent(componentId);
    }
    function EditVehicleComponentFn(e) {
        var componentId = e.currentTarget.dataset.component;
        EditVehicleComponent(componentId);
    }
    function DeleteConfigComponentFn(e) {
        var componentId = e.currentTarget.dataset.ComponentId;
        DeleteConfigComponent(componentId);
    }
    function AddToFleetFn(e) {
        var componentId = e.currentTarget.dataset.ComponentId;
        AddToFleet(componentId);
    }
    function LoadConfigDetailsFn(e) {
        LoadConfigDetails(e);
    }
