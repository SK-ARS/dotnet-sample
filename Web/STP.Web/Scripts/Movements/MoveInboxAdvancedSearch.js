function MoveInboxAdvancedSearchInit() {
    GrossWeightCountRange();
    WidthCountRange();
    LengthCountRange();
    RigidLengthCountRange();
    HeightCountRange();
    AxleCountRange();
    //$('.gross_weight1').hide();
}
$(document).ready(function () {
    if ($('#hf_IsPlanMovmentGlobal').length == 0) {
        MoveInboxAdvancedSearchInit();
    }
    $('body').on('click', '#AddOption', function (e) {
        SOAAddVehicleFilter(this);
    });
    $('body').on('click', '#RemoveOption', function (e) {
        SOARemoveVehicleFilter(this);
    });
    $('body').on('change', '#SOAVehicleDimension', function (e) {
        SOAVehicleDimension(this);
    });
    $('body').on('change', '.Operator-count-range', function (e) {
        SOAOperatorCountRange(this);
    });

    $('body').on('change', '#WeightCount', function () {
        GrossWeightCountRange();
    });
    $('body').on('change', '#WidthCount', function () {
        WidthCountRange();
    });
    $('body').on('change', '#LengthCount', function () {
        LengthCountRange();
    });
    $('body').on('change', '#RigidLengthCount', function () {
        RigidLengthCountRange();
    });
    $('body').on('change', '#HeightCount', function () {
        HeightCountRange();
    });
    $('body').on('change', '#AxleCount', function () {
        AxleCountRange();
    });

});
function onchangeDelegArrange() {
    var selectedVal = $('#DelegationList option:selected').attr('value');
}

function GrossWeightCountRange() {
    var WeightCount = $("#WeightCount").val();
    if (WeightCount == 2) {
        $(".GrossWeight1").show();
        $("#GrossWeight").parent().removeClass("soa-portal");
    }
    else {
        $(".GrossWeight1").hide();
        $("#GrossWeight").parent().addClass("soa-portal");
    }
}
function WidthCountRange() {
    var WidthCount = $("#WidthCount").val();
    if (WidthCount == 2) {
        $(".OverallWidth1").show();
        $("#OverallWidth").parent().removeClass("soa-portal");
    }
    else {
        $(".OverallWidth1").hide();
        $("#OverallWidth").parent().addClass("soa-portal");
    }
}
function LengthCountRange() {
    var LengthCount = $("#LengthCount").val();
    if (LengthCount == 2) {
        $(".OverallLength1").show();
        $("#OverallLength").parent().removeClass("soa-portal");
    }
    else {
        $(".OverallLength1").hide();
        $("#OverallLength").parent().addClass("soa-portal");
    }
}
function RigidLengthCountRange() {
    var RigidLengthCount = $("#RigidLengthCount").val();
    if (RigidLengthCount == 2) {
        $(".RigidLength1").show();
        $("#RigidLength").parent().removeClass("soa-portal");
    }
    else {
        $(".RigidLength1").hide();
        $("#RigidLength").parent().addClass("soa-portal");
    }
}
function HeightCountRange() {
    var HeightCount = $("#HeightCount").val();
    if (HeightCount == 2) {
        $(".Height1").show();
        $("#Height").parent().removeClass("soa-portal");
    }
    else {
        $(".Height1").hide();
        $("#Height").parent().addClass("soa-portal");
    }
}
function AxleCountRange() {
    var AxleCount = $("#AxleCount").val();
    if (AxleCount == 2) {
        $(".AxleWeight1").show();
        $("#AxleWeight").parent().removeClass("soa-portal");
    }
    else {
        $(".AxleWeight1").hide();
        $("#AxleWeight").parent().addClass("soa-portal");
    }
}



function onchangeDelegArrange() {
    var selectedVal = $('#DelegationList option:selected').attr('value');
}

function SOAAddVehicleFilter(_this) {
    var parentdiv = $(_this).closest('.VehicleFilter');
    var condition = parentdiv.find('#Operator').val();
    var vehicleFiltercount = $("div[class*='VehicleFilter']").length;
    if (condition == "1" || condition == "2") {
        var commonId = "VehicleFilterData_" + vehicleFiltercount;
        parentdiv.find('#errormsg').hide();
        if ($("#VehicleFilterData_" + vehicleFiltercount).length > 0) {
            vehicleFiltercount++;
        }
        var newFilterDiv = "#" + commonId;
        $('#VehicleFilterDiv').append($(parentdiv)[0].outerHTML);
        $('#VehicleFilterDiv .VehicleFilter:last').attr("id", commonId);
        $('#' + commonId).find(".gross_weight1").hide();
        $('#' + commonId).find('#RemoveOption').show();
        parentdiv.find('#AddOption').hide();
        parentdiv.find('#RemoveOption').show();

        $(newFilterDiv).find('.gross_weightUnit').text('kg');
        $(newFilterDiv).find('.gross_weight1Unit').text('kg');

        $(newFilterDiv).find('.vehicletextbox').attr("id", "gross_weight_max_kg");
        $(newFilterDiv).find('.vehicletextbox').attr("name", "gross_weight_max_kg");
        $(newFilterDiv).find('.vehicletextbox1').attr("id", "gross_weight_max_kg1");
        $(newFilterDiv).find('.vehicletextbox1').attr("name", "gross_weight_max_kg1");
        $(newFilterDiv).find('input').val('');
        $(newFilterDiv).find('#SOAVehicleDimension').val('gross_weight_max_kg');
        $(newFilterDiv).find('#OperatorCount').val('<=');
        //$(newFilterDiv).find('#Operator').val('null');
        $(newFilterDiv).find('#OperatorCount').attr('style', 'width: 12rem;');
        $(newFilterDiv).find('.vehicletextbox').attr('style', 'width:60%; padding-left: 0rem;');        //$(newFilterDiv).find('#Operator').val('null');
    }
    else {
        parentdiv.find('#errormsg').show();
    }
}

function SOAOperatorCountRange(_this) {
    var OperatorCount = $(_this).val();
    var divId = $(_this).closest('.VehicleFilter').attr('id');
    if (OperatorCount == "between") {
        $(_this).closest('.VehicleFilter').find(".gross_weight1").show();
        $(_this).attr('style', 'width: 10rem;');
        $(_this).closest('.VehicleFilter').find(".vehicletextbox").attr('style', 'width: 90%; padding-left: 0rem;');
    }
    else {
        $(_this).attr('style', 'width: 12rem;');
        $(_this).closest('.VehicleFilter').find(".gross_weight1").hide();
    }
}

function SOAVehicleDimension(e) {
    var fieldData = $(e).val();
    var parentdiv = $(e).closest('.VehicleFilter');
    parentdiv.find("#gross_weight_max_kg").attr("name", fieldData);
    parentdiv.find("#gross_weight_max_kg").attr("id", fieldData);
    parentdiv.find("#gross_weight_max_kg1").attr("name", fieldData + "1");
    parentdiv.find("#gross_weight_max_kg1").attr("id", fieldData + "1");
    if (fieldData != "gross_weight_max_kg" && fieldData != "max_axle_weight_max_kg") {
        parentdiv.find('.gross_weightUnit').text('m');
        parentdiv.find('.gross_weight1Unit').text('m');
    }
    else {
        parentdiv.find('.gross_weightUnit').text('kg');
        parentdiv.find('.gross_weight1Unit').text('kg');
    }
}
function SOARemoveVehicleFilter(_this) {
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
