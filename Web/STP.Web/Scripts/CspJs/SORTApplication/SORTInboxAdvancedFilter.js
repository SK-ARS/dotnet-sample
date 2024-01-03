    $(document).ready(function() {
        $(".gross_weight1").hide();
        $("#table-head").on('click', viewSORTAdvHaulier);
        $("#AddOption").on('click', SortAddVehicleFilter);
    });
    function AllowNumericOnly() {
        // isnumeric
        $('.isnumeric').keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });
    }
    $(function () {
        AllowNumericOnly();
    });

    function SortAddVehicleFilter(_this) {
        
        var divId = $(_this).closest('.VehicleFilter').attr('id');
        var condition = $('#' + divId).find('#operator').val();
        var vehicleFiltercount = $("div[class*='VehicleFilter']").length;
        if (condition == "1" || condition == "2") {
            $('#' + divId).find('#errormsg').hide();
            var div = $('#viewSORTAdvHaulier').html();
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

    function SortOperatorCountRange(_this) {
        
        var OperatorCount = $(_this).val();
        var divId = $(_this).closest('.VehicleFilter').attr('id');
        if (OperatorCount == "between") {
            $('#' + divId).find(".gross_weight1").show();
        }
        else {
            $('#' + divId).find(".gross_weight1").hide();
        }
    }


    function SortVehicleDimension(_this) {
        
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
    function SortRemoveVehicleFilter(_this) {        
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
