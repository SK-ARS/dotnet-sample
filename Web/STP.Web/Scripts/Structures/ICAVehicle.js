var numberValidPattern = new RegExp(/^[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)$/);
var structName = $('#hf_structName').val();
var ESRN = $('#hf_ESRN').val();
var structureId = $('#hf_structureId').val();
var sectionId = $('#hf_sectionId').val();

function ICAVehicleInit() {
    structName = $('#hf_structName').val();
    ESRN = $('#hf_ESRN').val();
    structureId = $('#hf_structureId').val();
    sectionId = $('#hf_sectionId').val();
    if ($('#hf_Helpdest_redirect').val() == "true") {
        $("#MovementClassConfig").attr("disabled", "disabled");
        $("#ConfigType").attr("disabled", "disabled");

        $("#GrossWeight").attr("readonly", "readonly");
        $("#OverallLength").attr("readonly", "readonly");
        $("#Width").attr("readonly", "readonly");
        $("#AxleWeight").attr("readonly", "readonly");
        $("#MinAxleSpacing").attr("readonly", "readonly");

        $("#CompNum").attr("disabled", "disabled");
    }
    document.getElementById('div_component_details').style.display = 'none';
    $('#tractor').hide(); $('#DDtractor').hide();
    $('#trailer').hide(); $('#DDtrailer').hide();
    var result = $('#hf_ICAResult').val();
    if (result != '') {
        DisplayResult();
        var compNum = $('#CompNum').val();
        switch (compNum) {
            case '1': $('#tractor').show(); $('#DDtractor').show(); $('#trailer').hide(); $('#DDtrailer').hide(); break;
            case '2': $('#tractor').show(); $('#DDtractor').show(); $('#trailer').show(); $('#DDtrailer').show(); break;
            default: $('#tractor').hide(); $('#DDtractor').hide(); $('#trailer').hide(); $('#DDtrailer').hide(); break;
        }

        var configType = $('#ConfigType').val();
        if (configType == 244001) {
            $('#div_component_details').show();
            // document.getElementById('div_component_details').style.display = 'block';
            $('#table_buttons').hide();
        }
    }
}

$(document).ready(function () {
    $('body').on('click', '#hideerr', function (e) {
        e.preventDefault();
        hideError(this);
    });
    $('body').on('click', '#hideerr', validateGross);

    $('body').on('click', '#PerformBtn', function (e) {
        e.preventDefault();
        saveICAData(this);
    });
    $('body').on('mouseover', '#PerformBtn', mouseoverIca);
    $('body').on('mouseout', '#PerformBtn', mouseoutIca);

    $('body').on('click', '#btnCancel', function (e) {
        e.preventDefault();
        Success(this);
    });
    $('body').on('mouseover', '#btnCancel', mouseover);
    $('body').on('mouseout', '#btnCancel', mouseout);
    $('body').on('click', '#PerformB', function (e) {
        e.preventDefault();
        saveICADataConfigType(this);
    });
    $('body').on('mouseover', '#PerformB', mouseoverIcaone);
    $('body').on('mouseout', '#PerformB', mouseoutIcaone);
    $('body').on('click', '#Cancel', function (e) {
        e.preventDefault();
        Success(this);
    });
    $('body').on('mouseover', '#Cancel', mouseoverone);
    $('body').on('mouseout', '#Cancel', mouseoutone);


    //methode for showing component details
    $('body').on('change', '#ConfigType', function () {
        var configTypeId = $(this).val();
        if (configTypeId == 244001) {
            $('#div_component_details').show();
            //document.getElementById('div_component_details').style.display = 'block';
            $('#table_buttons').hide();
        }
        else {
            $('#div_component_details').hide();
            // document.getElementById('div_component_details').style.display = 'none';
            $('#table_buttons').show();
        }
    });


    //method for showing the component type dropdown
    $('body').on('change', '#CompNum', function () {
        switch ($(this).val()) {
            case '1': $('#tractor').show(); $('#DDtractor').show(); $('#trailer').hide(); $('#DDtrailer').hide(); break;
            case '2': $('#tractor').show(); $('#DDtractor').show(); $('#trailer').show(); $('#DDtrailer').show(); break;
            default: $('#tractor').hide(); $('#DDtractor').hide(); $('#trailer').hide(); $('#DDtrailer').hide(); break;
        }
    });
    $('body').on('change', '#MovementClassConfig', function () {
        $("#dropError").hide();
    });
    $('body').on('change', '#CompNum', function () {
        $("#dropError2").hide();
    });
    $('body').on('change', '#ConfigType', function () {
        $("#dropError1").hide();
    });

    $('body').on('keyup', '#AxleWeight', function () {
        AxleWidth();
    });
    $('body').on('keypress', '#AxleWeight', function () {
        hideError();
    });

    $('body').on('keyup', '#MinAxleSpacing', function () {
        AxleSpacing();
    });
    $('body').on('keypress', '#MinAxleSpacing', function () {
        hideError();
    });

    $('body').on('keyup', '#CompNum', function () {
        DropCompNum();
    });
    $('body').on('keypress', '#CompNum', function () {
        hideError();
    });

    $('body').on('keyup', '#TractorWeight', function () {
        tractorWeightValidate();
    });
    $('body').on('keypress', '#TractorWeight', function () {
        hideError();
    });

    $('body').on('keyup', '#TrailerWeight', function () {
        trailerWeightValidate();
    });
    $('body').on('keypress', '#TrailerWeight', function () {
        hideError();
    });

    $('body').on('keyup', '#TractorMaxAxleWeight', function () {
        tracrMaxAxleWtValidate();
    });
    $('body').on('keypress', '#TractorMaxAxleWeight', function () {
        hideError();
    });

    $('body').on('keyup', '#TrailerMaxAxleWeight', function () {
        trailerMaxAxleWtValidate();
    });
    $('body').on('keypress', '#TrailerMaxAxleWeight', function () {
        hideError();
    });

    $('body').on('keyup', '#TractorMinAxleSpacing', function () {
        tracrMaxAxleSpValidate();
    });
    $('body').on('keypress', '#TractorMinAxleSpacing', function () {
        hideError();
    });

    $('body').on('keyup', '#TrailerMinAxleSpacing', function () {
        trailerMaxAxleSpValidate();
    });
    $('body').on('keypress', '#TrailerMinAxleSpacing', function () {
        hideError();
    });

    $('body').on('keyup', '#AxleCount', function () {
        axleCountValidate();
    });
    $('body').on('keypress', '#AxleCount', function () {
        hideError();
    });

    $('body').on('change', '#MovementClassConfig', function () {
        DropValidation();
    });
    $('body').on('click', '#MovementClassConfig', function () {
        hideError();
    });

    $('body').on('change', '#ConfigType', function () {
        DropConfig();
    });
    $('body').on('click', '#ConfigType', function () {
        hideError();
    });

    $('body').on('keyup', '#OverallLength', function () {
        validateLength();
    });
    $('body').on('keypress', '#OverallLength', function () {
        hideError();
    });

    $('body').on('keyup', '#Width', function () {
        validateWidth();
    });
    $('body').on('keypress', '#Width', function () {
        hideError();
    });

});

function DropValidation() {
    var dropValid = true;
    var ddlFruits = document.getElementById("MovementClassConfig");
    if (ddlFruits.value == "") {
        $("#dropError").show();
        dropValid = false;
    }
    else {
        dropValid = true;
        $("#dropError").hide();

    }
    return dropValid;
}
function DropConfig() {
    var dropValidate = true;
    var ddlConfig = document.getElementById("ConfigType");
    if (ddlConfig.value == "") {
        $("#dropError1").show();
        dropValidate = false;
    }
    else {
        dropValidate = true;
        $("#dropError1").hide();

    }
    return dropValidate;
}
function DropComNum() {
    var dropValidCom = true;
    var ddlFruits = document.getElementById("CompNum");
    if (ddlFruits.value == "") {
        $("#dropError2").show();
        dropValidCom = false;
    }
    else {
        $("#dropError2").hide();
        dropValidCom = true;


    }
    return dropValidCom;
}

//function to display the ICA results
function DisplayResult() {
    var result = $('#hf_ICAResult').val();
    //if (result == '' || result == null) {
    //    result = "ICA is not available for this structure section";
    //}

    if (result == 'Suitable' || result == 'Marginally suitable' || result == 'Unsuitable') {

        ShowSuccessModalPopup('Vehicle is "' + result + '" for traversing the structure', 'CloseSuccessModalPopup');

    }
    else {

        ShowSuccessModalPopup(result, 'CloseSuccessModalPopup');


    }

}
function Success() {
    location.reload();
}
function saveICADataConfigType() {
    $.ajax({
        url: '../Structures/ICAVehicleData',
        type: 'POST',
        data: $("#ICAVehicleInfo").serialize(),
        success: function (result) {
            if (result == true) {

                saveDataConfigType();
            }

        },
        error: function (xhr, status) {
        }
    });
}
function saveICAData() {
    $.ajax({
        url: '../Structures/ICAVehicleData',
        type: 'POST',
        data: $("#ICAVehicleInfo").serialize(),
        success: function (result) {
            if (result == true) {

                saveData();
            }

        },
        error: function (xhr, status) {
        }
    });
}
function performICA() {

    startAnimation();
    var movementClassConfig = $('#MovementClassConfig').val();
    var ConfigType = $('#ConfigType').val();
    var CompNum = $('#CompNum').val();
    var TractorComponentType = $('#DDtractor').val();
    var TrailerComponentType = $('#DDtrailer').val();
    //var structureId = structureID;
    //var sectionId = sectionID;
    //var structureName = structName;
    $("#generalSettingsId").load('../Structures/PerformICAVehicle?structName=' + structName + "&ESRN=" + ESRN + "&objICAVehicleModel=" + '' + "&MovementClassConfig=" + movementClassConfig + "&ConfigType=" + ConfigType + "&CompNum=" + CompNum + "&structureId=" + structureId + "&sectionId=" + sectionId + "&TractorComponentType=" + TractorComponentType + "&TrailerComponentType=" + TrailerComponentType,
        function () {
            DisplayResult();
            document.getElementById('checkSuitabilityOfVehicleId').style.display = 'block';
            stopAnimation();
        }
    );
};

function validateGross() {
    var grossValid = true;
    if ($("#GrossWeight").val() != '') {
        if (!numberValidPattern.test($("#GrossWeight").val())) {

            $("#grossValidate").show();
            grossValid = false;
        }
        else {
            $("#grossValidate").hide();
            grossValid = true;
        }
    }

    else {
        $("#grossValidateError").show();
        grossValid = false;
    }
    return grossValid;
}

function validateLength() {
    var lengthValid = true;
    if ($("#OverallLength").val() != '') {
        if (!numberValidPattern.test($("#OverallLength").val())) {

            $("#lengthValidate").show();
            lengthValid = false;
        }
        else {
            $("#lengthValidate").hide();
            lengthValid = true;
        }
    }
    else {
        $("#lengthValidateError").show();
        lengthValid = false;

    }
    return lengthValid;
}
function validateWidth() {
    var widthValid = true;
    if ($("#Width").val() != '') {
        if (!numberValidPattern.test($("#Width").val())) {

            $("#widthValidate").show();
            widthValid = false;
        }
        else {
            $("#widthValidate").hide();
            widthValid = true;
        }
    }
    else {
        $("#widthValidateError").show();
        widthValid = false;
    }
    return widthValid;
}

function AxleWidth() {
    var axleWidthValid = true;
    if ($("#AxleWeight").val() != '') {
        if (!numberValidPattern.test($("#AxleWeight").val())) {

            $("#maxAxlWValidate").show();
            axleWidthValid = false;
        }
        else {
            $("#maxAxlWValidate").hide();
            axleWidthValid = true;
        }
    }
    else {
        $("#maxAxlWValidateError").show();
        axleWidthValid = false;
    }
    return axleWidthValid;
}

function AxleSpacing() {
    var axleSpaceValid = true;
    if ($("#MinAxleSpacing").val() != '') {
        if (!numberValidPattern.test($("#MinAxleSpacing").val())) {

            $("#minAxlSValidate").show();
            axleSpaceValid = false;
        }
        else {
            $("#minAxlSValidate").hide();
            axleSpaceValid = true;
        }
    }
    else {
        $("#minAxlSValidateError").show();
        axleSpaceValid = false;
    }
    return axleSpaceValid;
}

function tractorWeightValidate() {
    var tractorWeightValid = true;
    if ($("#TractorWeight").val() != '') {
        if (!numberValidPattern.test($("#TractorWeight").val())) {

            $("#tmwValidate1").show();
            tractorWeightValid = false;
        }
        else {
            $("#tmwValidate1").hide();
            tractorWeightValid = true;
        }

    }
    else {
        $("#TD11Error").show();
        tractorWeightValid = false;
    }
    return tractorWeightValid;
}
function trailerWeightValidate() {
    var trailerWeightValid = true;
    if ($("#TrailerWeight").val() != '') {
        if (!numberValidPattern.test($("#TrailerWeight").val())) {

            $("#tmwValidate2").show();
            trailerWeightValid = false;
        }
        else {
            $("#tmwValidate2").hide();
            trailerWeightValid = true;
        }
    }
    else {
        $("#TD12Error").show();
        trailerWeightValid = false;

    }
    return trailerWeightValid;
}

function tracrMaxAxleWtValidate() {
    var tractorAxleWeightValid = true;
    if ($("#TractorMaxAxleWeight").val() != '') {
        if (!numberValidPattern.test($("#TractorMaxAxleWeight").val())) {

            $("#tmawValidate1").show();
            tractorAxleWeightValid = false;
        }
        else {
            $("#tmawValidate1").hide();
            tractorAxleWeightValid = true;
        }
    }
    else {
        $("#TD21Error").show();
        tractorAxleWeightValid = false;

    }
    return tractorAxleWeightValid;
}
function trailerMaxAxleWtValidate() {
    var trailerAxleWeightValid = true;
    if ($("#TrailerMaxAxleWeight").val() != '') {
        if (!numberValidPattern.test($("#TrailerMaxAxleWeight").val())) {

            $("#tmawValidate2").show();
            trailerAxleWeightValid = false;
        }
        else {
            $("#tmawValidate2").hide();
            trailerAxleWeightValid = true;
        }
    }
    else {
        $("#TD22Error").show();
        trailerAxleWeightValid = false;

    }
    return trailerAxleWeightValid;
}


function tracrMaxAxleSpValidate() {
    var tractorAxleSpValid = true;
    if ($("#TractorMinAxleSpacing ").val() != '') {
        if (!numberValidPattern.test($("#TractorMinAxleSpacing").val())) {
            $("#tmasValidate1").show();
            tractorAxleSpValid = false;
        }
        else {
            $("#tmasValidate1").hide();
            tractorAxleSpValid = true;
        }
    }
    else {
        $("#TD31Error").show();
        tractorAxleSpValid = false;
    }
    return tractorAxleSpValid;
}

function trailerMaxAxleSpValidate() {
    var trailerAxleSpValid = true;
    if ($("#TrailerMinAxleSpacing").val() != '') {
        if (!numberValidPattern.test($("#TrailerMinAxleSpacing").val())) {
            $("#tmasValidate2").show();
            trailerAxleSpValid = false;
        }
        else {
            $("#tmasValidate2").hide();
            trailerAxleSpValid = true;
        }
    }
    else {
        $("#TD32Error").show();
        trailerAxleSpValid = false;
    }
    return trailerAxleSpValid;
}
function axleCountValidate() {
    var axlecountValid = true;
    if ($("#AxleCount").val() != '') {
        if (!numberValidPattern.test($("#AxleCount").val())) {
            $("#NumValidate").show();
            axlecountValid = false;
        }
        else {
            $("#NumValidate").hide();
            axlecountValid = true;
        }
    }
    else {
        $("#TD41Error").show();
        axlecountValid = false;
    }
    return axlecountValid;
}
function hideError() {
    $("#grossValidateError").hide();
    $("#lengthValidateError").hide();
    $("#widthValidateError").hide();
    $("#maxAxlWValidateError").hide();
    $("#minAxlSValidateError").hide();
    $("#TD11Error").hide();
    $("#TD12Error").hide();
    $("#TD21Error").hide();
    $("#TD22Error").hide();
    $("#TD31Error").hide();
    $("#TD32Error").hide();
    $("#TD41Error").hide();
    $("#dropError").hide();
    $("#dropError1").hide();
    $("#dropError2").hide();
}
function saveDataConfigType() {
    var grossValidate = validateGross();
    var lenthValidate = validateLength();
    var widthValidate = validateWidth();
    var maxAxlWValidate = AxleWidth();
    var minAxlSValidate = AxleSpacing();
    var tmwValidate = tractorWeightValidate();
    var tmawValidate = tracrMaxAxleWtValidate();
    var tmasValidate = tracrMaxAxleSpValidate();
    var trailerWeightValid = trailerWeightValidate();
    var trmawValidate = trailerMaxAxleWtValidate();
    var trailerAxlevalidate = trailerMaxAxleSpValidate();
    var axlecountValidate = axleCountValidate();
    var dropvalid = DropValidation();
    var dropConfigValidate = DropConfig();
    var dropComValidate = DropComNum();
    if (grossValidate && lenthValidate && widthValidate && maxAxlWValidate && minAxlSValidate && tmwValidate && tmawValidate && tmasValidate
        && trailerWeightValid && trmawValidate && trailerAxlevalidate && axlecountValidate && dropvalid && dropConfigValidate
        && dropComValidate) {
        performICA();
    }
}
function saveData() {
    var grossValidate = validateGross();
    var lenthValidate = validateLength();
    var widthValidate = validateWidth();
    var maxAxlWValidate = AxleWidth();
    var minAxlSValidate = AxleSpacing();
    var dropvalid = DropValidation();
    var dropConfigValidate = DropConfig();
    var dropComValidate = DropComNum();

    if (grossValidate && lenthValidate && widthValidate && maxAxlWValidate && minAxlSValidate && dropvalid && dropConfigValidate
        && dropComValidate) {
        performICA();
    }
}
function mouseover() {
    document.getElementById("btnCancel").style.color = "white";
    document.getElementById("btnCancel").style.backgroundColor = "#275795";
}

function mouseout() {
    document.getElementById("btnCancel").style.color = "#275795";
    document.getElementById("btnCancel").style.backgroundColor = "white";
}
function mouseoverIca() {
    document.getElementById("PerformBtn").style.color = "white";
    document.getElementById("PerformBtn").style.backgroundColor = "#275795";
}

function mouseoutIca() {
    document.getElementById("PerformBtn").style.color = "#275795";
    document.getElementById("PerformBtn").style.backgroundColor = "white";
}
function mouseoverone() {
    document.getElementById("Cancel").style.color = "white";
    document.getElementById("Cancel").style.backgroundColor = "#275795";
}

function mouseoutone() {
    document.getElementById("Cancel").style.color = "#275795";
    document.getElementById("Cancel").style.backgroundColor = "white";
}
function mouseoverIcaone() {
    document.getElementById("PerformB").style.color = "white";
    document.getElementById("PerformB").style.backgroundColor = "#275795";
}

function mouseoutIcaone() {
    document.getElementById("PerformB").style.color = "#275795";
    document.getElementById("PerformB").style.backgroundColor = "white";
}

