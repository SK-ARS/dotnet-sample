    $(document).ready(function () {
        $(".gross_weight1").hide();
        $("#AddOption").on('click', SOAAddVehicleFilter);
        $("#RemoveOption").on('click', SOARemoveVehicleFilter);
        $("#SOAVehicleDimension").on('change', SOAVehicleDimension);
        $("#SOAOperatorCountRange").on('change', SOAOperatorCountRange);       
    });

    function onchangeDelegArrange() {
        var selectedVal = $('#DelegationList option:selected').attr('value');
    }

    function SOAAddVehicleFilter(e) {
        var divId = $(e).closest('.VehicleFilter').attr('id');
        var condition = $('#' + divId).find('#operator').val();
        var vehicleFiltercount = $("div[class*='VehicleFilter']").length;
        if (condition == "1" || condition == "2") {
            $('#' + divId).find('#errormsg').hide();
            var div = $('#div_movement_inbox_filter_advanced').html();
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

    function SOAOperatorCountRange(e) {
        var OperatorCount = $(e).val();
        var divId = $(e).closest('.VehicleFilter').attr('id');
        if (OperatorCount == "between") {
            $('#' + divId).find(".gross_weight1").show();
        }
        else {
            $('#' + divId).find(".gross_weight1").hide();
        }
    }


    function SOAVehicleDimension(e) {
        var fieldData = $(e).val();
        var divId = $(e).closest('.VehicleFilter').attr('id');
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
    function SOARemoveVehicleFilter(e) {
        var divId = $(e).closest('.VehicleFilter').attr('id');
        var vehicleFiltercount = $("div[class*='VehicleFilter']").length;
        if (vehicleFiltercount > 1) {
            $('#' + divId).remove();
            if (vehicleFiltercount == 2) {
                $('.VehicleFilter').find('#AddOption').show();
                $('.VehicleFilter').find('#RemoveOption').hide();
            }
        }
    }

