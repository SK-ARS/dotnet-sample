var vehicleId = 0;

var isFromComponent = false;
$(function () {

    if (typeof (CheckIsComponent) == 'function') {
        isFromComponent = CheckIsComponent();
    }

    $(".page_help").attr('url', '../../Home/NotImplemented');
    $(".page_help").click(function(){
        
        window.open($(this).attr("url"), "popupWindow", "width=910,height=620,scrollbars=yes");
    });

    $(".btn_back").live("click", function () {

    });

    $(".btn_Next").live("click", function () {
       
        var _this = $(this);
        var _form = _this.closest("form");

        var validator = $("form").validate(); // obtain validator
        var anyError = false;
        _form.find("input").each(function () {
            if (!validator.element(this)) { // validate every input element inside this step
                anyError = true;
            }
        });

        if (anyError)
            return false; // exit if any error found    


        $.ajax({
            type: 'POST',
            url: _form.attr("action"),
            data: _form.serialize(),
            success: function (data) {
                if (data.Success) {
                    $('#selection').hide();
                    $('#div_general').hide();
                    $('#div_register').show();
                }
            },
            async: false
        });

        //$("#tabs").tabs("option", "active", 1);
        return false;

    });

    $(".btn_Prev").click(function () {
        var selected = $("#tabs").tabs("option", "active");
        $("#tabs").tabs("disable", selected);
        $("#tabs").tabs("enable", selected - 1);
        $("#tabs").tabs("option", "active", selected - 1);
    });

    $('.btn_add').live('click', function () {
        $(this).closest('tr').clone().find("input:text").each(function () {
            $(this).val('').attr('id', function (_, id) { return id });
        }).end().appendTo("#id_Register tbody");
    });


    ShowBackButton();

});

//Onchange function for vehicle type dropdownlist
function VehicleTypeChange(thisVal) {
    var vehicleTypeId = $(thisVal).val();
    //var movementId = $('#Movement').val();
    $('#VehicleSubType option').remove();

    $('#componentDetails').hide("Blind");
    $('#componentDetails').html("");
    if (vehicleTypeId == "") {
        //$('#VehicleType').attr("disabled", "disabled");
        $('#VehicleSubType').attr("disabled", "disabled");
    }
    else {
        FillVehicleSubType(vehicleTypeId);
    }
}

//Onchange function for vehicle subtype dropdownlist
function VehicleSubTypeChange(thisVal) {
    var vehicleSubTypeId = $(thisVal).val();
    var movementId = $('#Movement').val();
    var vehicleTypeId = $('#ComponentType').val();
    if (vehicleSubTypeId == "") {
        $('#componentDetails').hide("Blind");
        $('#componentDetails').html("");
    }
    else {
        FillGeneralData(vehicleSubTypeId, vehicleTypeId);
    }
}

//function to fill vehicle type on movement classification dropdown change
function FillVehicleType(movementId) {
  
    $('#VehicleType option:not(:first-child)').remove();
    var url = '../Vehicle/FillVehicleType';
    $.post(url, { movementId: movementId }, function (data) {
        var datalength = data.type.length;
        for (var i = 0; i < datalength; i++) {
            $('#VehicleType').append('<option value="' + data.type[i].ComponentTypeId + '">' + data.type[i].ComponentName + '</option>');
            $('#VehicleType').attr("disabled", false);
        }
        if (datalength == 1) {
            $('#VehicleType option:eq(1)').prop('selected', true);
            $('#VehicleType').change();
        }
    });
}

function FillGeneralData(vehicleSubType, vehicleTypeId) {
    var islast = false;
    var isNotify = $('#ISNotif').val();
    //if (!isFromComponent) {
    //    var configId = GetConfigurationId();

    //}
    var url = '../Vehicle/ComponentGeneralPage';
    $.post(url, { vehicleSubTypeId: vehicleSubType, vehicleTypeId: vehicleTypeId, isComponent: isFromComponent,  isNotify: isNotify }, function (data) {
        $('#componentDetails').html(data);
        $('#componentDetails').show("Blind");
        SetRangeForSpacing();
        if (isFromComponent == false) {
            $('#div_general').find('#Speed').hide();
        }
        debugger;
        $('#componentImage').attr("style", "display:block")
        var imgurl = "../Content/images/Common/MasterPage/componet_icons/" + $('#Imagename').val() + ".jpg";
        $('#componentImage img').attr("src", imgurl);
        //$('#Axle_Spacing_To_Following').attr('range', '@ViewBag.minval,@ViewBag.maxval');
        //$('#selection').hide();
    });

}

function FillVehicleSubType(vehicleTypeId) {
    $('#VehicleSubType option:not(:first-child)').remove();
    var url = '../Vehicle/FillVehicleSubType';
    $.post(url, { vehicleTypeId: vehicleTypeId }, function (data) {
        debugger;
        var datalength = data.type.length;
        if (datalength == 1) {
            var vehicleTypeId = $('#ComponentType').val();
            var vehicleSubTypeId = data.type[0].SubCompType;
            $('#VehicleSubType').append('<option value="' + data.type[0].SubCompType + '" selected="selected">' + data.type[0].SubCompName + '</option>');
            $('#VehicleSubType').attr("disabled", false);
            FillGeneralData(vehicleSubTypeId, vehicleTypeId);
        }
            
        else if (datalength > 1) {

            $('#componentImage').attr("style", "display:none")
            $('#VehicleSubType').append('<option value="">Select</option>');
            for (var i = 0; i < datalength; i++) {
                $('#VehicleSubType').append('<option value="' + data.type[i].SubCompType + '">' + data.type[i].SubCompName + '</option>');
                $('#VehicleSubType').attr("disabled", false);
                       
            }
            if (isFromComponent) {

                $("#VehicleSubType option:contains(Wheeled load)").remove();
                $("#VehicleSubType option:contains(Recovered vehicle)").remove();
            }
        }
            
    });
}


//function to show back button(){
function ShowBackButton() {
    if (typeof ShowBackBtnComponent == 'function') {
        ShowBackBtnComponent();
    }
}
