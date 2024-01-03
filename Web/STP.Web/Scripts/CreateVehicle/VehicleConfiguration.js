var planMovement = false;
function VehicleConfigurationInit() {
    suppressKey();
    IterateThroughTextbox();
    ShowInFeet();
    var planMovement = false;
    if ($('#hf_IsPlanMovmentGlobal').val() == "True" ||$('#PlanMovement').val() == "True" || $('#isMovement').val() == "true" || $('#IsMovement').val() == "True" || $('#IsMovement').val() == "true") {
        planMovement = true;
    }
    var isCandidate = false;
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidate = true;
    }
    //if (planMovement || isCandidate) {
    //    $('body').on('change', '#div_config_general input[type=text]', function (e) {
    //        GetFilteredVehicles();
    //    });
    //}
    if ($('#MovementTypeId').val() != "270006") {
        $('.div_Speed').hide();
    }
    else {
        $('.div_Speed').show();
    }
}
//function to prevent entering non-numeric characters
function suppressKey() {
    $('#div_config_general input:text').each(function () {
        $(this).keypress(function (evt) {
            $(this).closest('div').find('.error').text('');
            var type = $(this).attr('datatype');//datatype of the field
            var text = $(this).val();//textbox value

            if (!IsPreference()) {

                if (type == 'int') {
                    var charCode = (evt.which) ? evt.which : event.keyCode;
                    return !(charCode > 31 && (charCode < 48 || charCode > 57));
                }
                else if (type == 'float') {
                    var charCode = (evt.which) ? evt.which : event.keyCode;
                    return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
                }
            }
            else {
                if (type != 'string') {
                    var charCode = (evt.which) ? evt.which : event.keyCode;
                    return !((charCode != 39 && charCode != 34) && charCode > 31 && (charCode < 48 || charCode > 57));
                }
            }
        });
        $(this).on("paste", function (e) {
            $(this).closest('div').find('.error').text('');
            var type = $(this).attr('datatype');//datatype of the field
            var text = $(this).val();//textbox value

            if (!IsPreference()) {

                if (type == 'int') {
                    if (e.originalEvent.clipboardData.getData('Text').match(/[^\d]/)) {
                        event.preventDefault();
                    }
                }
                else if (type == 'float') {
                    if (e.originalEvent.clipboardData.getData('Text').match(/[^\d]/)) {
                        event.preventDefault();
                    }
                }
            }
        });
        var original = '';
        $(this).on('input', function () {
            if ($(this).attr("id") != "Internal_Name") {
                if ($(this).val().replace(/[^.]/g, "").length > 1) {
                    $(this).val(original);
                } else {
                    original = $(this).val();
                }
            }
        });
    });
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
    if ($('.Config_Details').length == 0) {
        $('#div_config_general input:text').each(function () {
            if (IsLengthFields(this)) {
                ConvertRangeToFeets(this);
            }
        });
    }
    else {
        $('.Config_Details input:text').each(function () {
            if (IsLengthFields(this)) {
                ConvertRangeToFeets(this);
            }
        });
    }
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
    var div = '.Config_Details';
    if ($('.Config_Details').length == 0) {
        div = '#div_config_general';
    }

    $(div+' input:text').each(function () {

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