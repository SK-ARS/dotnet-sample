$(document).ready(function () {
    if ($('#AuthorizeMovementGeneral').length == 0) {
        ViewConfigurationInit();
    }
    $('body').on('click', '#btn_back_to_config', function (e) {
        e.preventDefault();
        ViewBackbutton(this);
    }); 
    $('body').on('click', '#view-allAxle', function (e) {
        var vehicleId = $(this).data("vehicleid");
        var movementTypeId = $(this).data("movementtypeid");
        var isFleet = $(this).data("isfleet");
        AllAxleDetailPopUpFleet(vehicleId, movementTypeId, isFleet);
    });
    $('body').on('click', '.view-allaxle-modal-close', function (e) {
        e.preventDefault();
        CloseAllAxlePopUp();
    });
});
function ViewConfigurationInit() {
    if ($('#hf_vehicle_config_page').length > 0) {
        SelectMenu(5);
    }
    ShowHideHeaderTyreSpaceViewConfiguration();
    //HeaderTyreSpaceCount();
    HideAxleSpacingForLastComp();
    IterateThroughTextbox();
    ShowInFeet();
    ShowAxleInFeet();
}

function HeaderTyreSpaceCount(isAllAxle = false) {
    var grtValue = 0;

    if ($('#tyreEmpty').val() != "True" && $('#tyreEmpty').val() != "true") {
        if (isAllAxle) {
            $('.allAxleDetailsPopUp #vehicle-table table tr').each(function () {
                var _thisVal = parseInt($(this).find('.wheel_space').length);
                if (_thisVal > grtValue) {
                    grtValue = _thisVal;
                }
            });
            for (var i = 1; i <= grtValue; i++) {
                $('.allAxleDetailsPopUp .tyreSpaceCnt').append('<th style="width: 10%;">' + i + '</th>');
            }
            $('.allAxleDetailsPopUp .headgrad2').attr('colspan', grtValue);
        }
        else {
            $('#vehicle-table table tr').each(function () {
                var _thisVal = parseInt($(this).find('.wheel_space').length);
                if (_thisVal > grtValue) {
                    grtValue = _thisVal;
                }
            });
            for (var i = 1; i <= grtValue; i++) {
                $('.tyreSpaceCnt').append('<th style="width: 10%;">' + i + '</th>');
            }
            $('.headgrad2').attr('colspan', grtValue);
        }
    }
}

//function to show tyre spacing header
function ShowHideHeaderTyreSpaceViewConfiguration(isAllAxle = false) {
    if (isAllAxle) {
        if ($('.allAxleDetailsPopUp .wheel_space').length == 0) {
            $('.allAxleDetailsPopUp .headgrad2').hide();
            //$('.sub1').hide();
        }
        else {
            $('.allAxleDetailsPopUp .headgrad2').show();
            $('.allAxleDetailsPopUp .sub1').show();
        }

        if ($('.allAxleDetailsPopUp .tyre_size').length == 0) {
            $('.allAxleDetailsPopUp .headgrad_tyreSize').hide();
        }
        else {
            $('.allAxleDetailsPopUp .headgrad_tyreSize').show();
        }
    }
    else {
        if ($('.wheel_space').length == 0) {
            $('.headgrad2').hide();
            //$('.sub1').hide();
        }
        else {
            $('.headgrad2').show();
            $('.sub1').show();
        }

        if ($('.tyre_size').length == 0) {
            $('.headgrad_tyreSize').hide();
        }
        else {
            $('.headgrad_tyreSize').show();
        }
    }
}
function ViewBackbutton() {
    window.location = "/VehicleConfig/VehicleConfigList?IsFromMenu=0";
}
function HideAxleSpacingForLastComp() {

    if ($('#CompoCount').val() == 1) {
        var axl = $("div[id='view-Axle Spacing To Following']");
        axl.hide();
    }
    else if ($('#CompoCount').val() > 1) {
        var cnt = $('#CompoCount').val() - 1;
        var divcomp = $('div#viewCompDetail:eq(' + cnt + ')');
        var axl = $(divcomp).find($("div[id='view-Axle Spacing To Following']"));
        axl.hide();
    }
}

$('body').on('click', '.viewComponentDetail', function (e) {
    e.preventDefault();
    var componentid = $(this).attr('componentid');
    ViewComponentDetails(componentid);

});
function ViewComponentDetails(componentid) {
    var ComponentCntrId = "veh_component_details_" + componentid;
    if (document.getElementById(ComponentCntrId).style.display !== "none") {
        document.getElementById(ComponentCntrId).style.display = "none"
        document.getElementById('chevlon-up-icon_' + componentid).style.display = "none"
        document.getElementById('chevlon-down-icon_' + componentid).style.display = "block"
        $('#spnDetailStatus_' + componentid).text("Show Details");
    }
    else {
        document.getElementById(ComponentCntrId).style.display = "block"
        document.getElementById('chevlon-up-icon_' + componentid).style.display = "block"
        document.getElementById('chevlon-down-icon_' + componentid).style.display = "none"
        $('#spnDetailStatus_' + componentid).text("Hide Details");
    }
}

function IsPreference() {
    var isFeet = false;
    var unit = $('#UnitValue').val();
    if (unit == 692002) {
        isFeet = true;
    }
    return isFeet;
}
function IterateThroughTextbox() {
    $('#vehicles input:text,#vehiclesummary input:text').each(function () {
        if (IsLengthFields(this)) {
            ConvertRangeToFeets(this);
        }
    });

}
//function to check whether the textbox is a length field or not
function IsLengthFields(_this) {
    var isLengthField = false;
    var LengthField = $(_this).parent().closest('div').find('.lblUnit').text();
    //console.log(LengthField);
    if (LengthField == 'm' || LengthField == 'ft') {
        isLengthField = true;
    }
    return isLengthField;
}
//function to convert range for length fields into feet and inches
function ConvertRangeToFeets(_this) {
    $(_this).closest('div').find('.error').text('');
    var range = $(_this).attr('range');//min and max range
    var splitRange = range.split(',');
    var minval = splitRange[0];
    var maxval = splitRange[1];
    if (IsPreference()) {
        var minvalFt = ConvertToFeets(minval);
        var maxvalFt = ConvertToFeets(maxval);

        range = $(_this).attr('range', '' + minvalFt + ',' + maxvalFt + '');

    }
    return range;

}
//function to convert metres to feet
function ConvertToFeets(_this) {
    var needRoundOff = 0;
    var metres = _this;
    var metreInches = metres * 39.370078740157477;

    //to prevent 9'0" from getting converted to 8'11"
    needRoundOff = metreInches % 1;
    if (needRoundOff >= .99) {
        metreInches = Math.ceil(metreInches);
    }

    var feet = parseInt(metreInches / 12);
    var inches = Math.floor(metreInches % 12);
    var result = feet + '\'' + inches + '\"';
    return result;

}
//function to show data in feet and inches format
function ShowInFeet() {

    $('#vehicles input:text, #vehiclesummary input:text').each(function () {

        if (IsPreference()) {

            if (IsLengthFields(this)) {
                var data = $(this).val();
                data = ConvertToFeets(data);
                if (data != "0\'0\"") {
                    $(this).val(data);
                }
                else {
                    $(this).val(null);
                }

            }
        }
    })


    $('#viewComponentDetail .Noborder').each(function () {
        if (IsPreference()) {
            if (IsLengthFields(this)) {
                var data = $(this).text();
                data = ConvertToFeets(data);
                if (data != "0\'0\"") {
                    $(this).text(data);
                }
                else {
                    $(this).text(null);
                }

            }
        }
    })
}

function ShowAxleInFeet(isAllAxle = false) {
    var unitvalue = $('#UnitValue').val();

    if (unitvalue == 692002) {
        var div = "";
        if (isAllAxle) {
            div = ".allAxleDetailsPopUp";
        }
        $(div+' .tblAxle tbody tr').each(function () {
            var distanceToNxtAxl = $(this).find('.disttonext').text();
            if (distanceToNxtAxl.indexOf('\'') === -1) {
                distanceToNxtAxl = ConvertToFeets(distanceToNxtAxl);
                $(this).find('.disttonext').text(distanceToNxtAxl);
            }
            var tyreSpace = null;
            $(this).find('.cstable').each(function () {
                var _thistxt = $(this).text();
                //if (_thistxt != undefined) {
                //    _thistxt = ConvertToFeets(_thistxt);
                //    $(this).text(_thistxt);
                //}

                if (tyreSpace != null) {
                    tyreSpace = tyreSpace + "," + _thistxt;
                }
                else {
                    tyreSpace = _thistxt;
                }

            });
        });
    }
}

function AllAxleDetailPopUpFleet(vehicleId, movementTypeId, isFleet) {
    startAnimation();
    $("#popupDialogue").load("../VehicleConfig/AllAxleDetailsPopUp", {
        vehicleId: vehicleId, movementTypeId: movementTypeId, isFleet: isFleet
    }, function () {
        stopAnimation();
        $('#allAxleDetailsPopUp').modal({ keyboard: false, backdrop: 'static' });
        $("#popupDialogue").show();
        $("#overlay").show();
        $('#allAxleDetailsPopUp').modal('show');
        ShowHideHeaderTyreSpaceViewConfiguration(true);
        HeaderTyreSpaceCount(true);
        ShowAxleInFeet(true);
    });
}

function CloseAllAxlePopUp() {
    $('#allAxleDetailsPopUp').modal('hide');
    $("#overlay").hide();
}