$(document).ready(function () {
    var isMovement = $('#IsMovement').val();
    if (isMovement == "true" || isMovement == "True") {
        $('#btn_back_to_Config_list').hide();
    }
    else {
        SelectMenu(5);
    }
    var cloneCount = 0;
    var componentIdList = [];
    FillComponentFav();

    $('.card-image li.components').click(function () {
        var imageName = $(this).attr("value");
        var componentId = $(this).attr("id");
        var componentName = $(this).attr("name");
        var componentHolder = $(this).parents('div.card-image');
        var componentIndex = $(componentHolder).attr('indx');
        var imageHolder = $(componentHolder).find('div.card');
        var componentNameHolder = $(componentHolder).find('div.filters');
        let clone = $("div[id^='add-component-template']:last").clone(true, true);

        if (componentIdList.length == 0) {
            $("div[id^='add-component-template']:nth-child(2)").find('div.filters').removeClass('disabled');
        }

        componentIdList[componentIndex] = componentId;
        $(imageHolder).find('img').attr('src', '/Content/Images/Common/MasterPage/componet_icons/' + imageName + '.jpg');
        $(componentNameHolder).find('label').text(componentName);

        $(this).parent().find('a.dropdown-item').removeClass('dropdown-active');
        $(this).find('a.dropdown-item').addClass('dropdown-active');

        if (componentIdList.length == (2 + cloneCount)) {
            clone.find('div.filters').attr('aria-expanded', "false");
            clone.find('div.filters').removeClass('show');
            clone.find('ul').removeClass('show');
            clone.attr('indx', cloneCount + 2);
            clone.attr('id', 'add-component-template' + (++cloneCount))
                .appendTo("#component-container");
        }
    });

    $('body').on('click', '.btn_back_to_Config_list', function (e) {
        e.preventDefault();
        BackToConfigList(this);
    });
    $('body').on('click', '.ImportOption', function (e) {
        e.preventDefault();
        ImportVehicleComponentOption(this);
    });
    $('body').on('click', '.ImportComponentOption', function (e) {
        e.preventDefault();
        ImportComponentOption(this);
    });

});

function NavigateBackToHome() {
    window.location = '../Home/Hauliers';
}
function ImportVehicleComponent(source) {
    $("#IsVehicle").val('');
    $.ajax({
        type: "POST",
        url: '../VehicleConfig/VehicleComponentDetails',
        data: { importFrm: source },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#selectComponent').html('');
            $('#selectComponent').html(response);
            $('.configComponent').hide();
            $('.createConfig').attr("style", "padding-left:3% !important");
        },
        error: function (result) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
/// import component (clone component)
function ImportComponentToConfig(componentId) {
    var IsApplication = false;
    var vehicleConfigId = 0;
    if ($('#IsApplication').val() == "true" || $('#IsApplication').val() == "True") {
        IsApplication = true;
    }
    if ($('#vehicleConfigId').val() != undefined) {
        vehicleConfigId = $('#vehicleConfigId').val();
    }
    $.ajax({
        async: false,
        type: "POST",
        url: '../Vehicle/ImportComponent',
        dataType: "json",
        data: JSON.stringify({ componentId: componentId, isMovement: IsApplication, vehicleId: vehicleConfigId }),
        processdata: true,
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            FillComponentDetailsForConfig(result.Guid, result.ConfigId);
        },
        error: function (result) {

        },
        complete: function () {
            stopAnimation();
        }
    });
}

function FillComponentDetailsForConfig(Guid, ConfigId, IsVehicleConfigurationEdit = false) {

    CloseSuccessModalPopup();
    WarningCancelBtn();

    if ($.trim($('#vehicle_Create_section').html())) {
        $.ajax({
            type: "GET",
            url: '../VehicleConfig/CreateConfiguration',
            data: { Guid: Guid, isMovement: true },
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
    else if ($.trim($('#vehicle_Component_edit_section').html())) {
        $.ajax({
            type: "GET",
            url: '../VehicleConfig/EditConfiguration',
            data: { vehicleId: ConfigId, isApplication: true },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                stopAnimation();
                SubStepFlag = 2.5;
                $('#vehicle_Component_edit_section').html('');
                $('#vehicle_Component_edit_section').html(response);
                $('.createConfig').unwrap('#banner-container');
                SelectMenu(2);
            },
            error: function (result) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    else if (IsVehicleConfigurationEdit == "true") {
        window.location = '../VehicleConfig/EditConfiguration' + EncodedQueryString('vehicleId=' + ConfigId);
    }
    else {
        window.location = '/VehicleConfig/CreateConfiguration' + EncodedQueryString('Guid=' + Guid + '&vehicleConfigId=' + ConfigId);
    }
}

function FillComponentFav() {

    $.ajax({
        async: false,
        type: "POST",
        url: '../VehicleConfig/GetComponentFavourites',
        dataType: "json",
        processdata: true,
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {

            var li = "";
            var favList = result;
            $.each(favList, function (key, value) {
                li += '<li id="table-head" class="pr-2 pl-2 ImportComponentOption" compId=' + value.ComponentId + ' ><span><a class="dropdown-item edit-normal" href="#">' + value.ComponentName + '</a></span><span><img src="../Content/assets/images/star-enabled.svg" width="20" alt="vehicle-1"></span></li><hr class="pl-0">';
            });

            $("ul #componentFav").prepend(li);

        },
        error: function (result) {

        },
        complete: function () {
            stopAnimation();
        }
    });
}

function BackToConfigList() {
    window.location.href = '../VehicleConfig/VehicleConfigList';
}
function ImportVehicleComponentOption(e) {
    var flag = $(e).attr("flag");
    ImportVehicleComponent(flag);
}
function ImportComponentOption(e) {
    var compId = $(e).attr("compid");
    ImportComponentToConfig(compId);
}
