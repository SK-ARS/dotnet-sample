

$(function () {
    //var regex = "[0-9.]";
    //$('.div_config_general input:text').attr('datatype', 'int').filter_input({ regex: '[0-9]', live: true });
    //$('.div_config_general input:text').attr('datatype', 'float').filter_input({ regex: regex, live: true });
    //$('.unit_text').filter_input({ regex: regex, live: true });
    suppressKey();
    IterateThroughTextbox();
    ShowInFeet();
    if ($('.div_config_general'))
        ShowInFeetSpan();
    // suppressInvalidFloat();
    var travelSpeed = $('#TravelSpeedVal').val();
    if (travelSpeed == null || travelSpeed == '') {
        $('#SpeedUnits').val(229001);
    }
    else if (travelSpeed != null) {
        $('#SpeedUnits').val(travelSpeed);
    }
    
});

function suppressKey() {
  
    $('.div_config_general input:text').each(function () {
        $(this).keypress(function (evt) {
            $(this).closest('div').find('.error').text('');
            var type = $(this).attr('datatype');//datatype of the field
            var text = $(this).val();//textbox value
            
            //if not isprefrence()
            if (!IsPreference()) {
                if (type == 'int') {
                    //this.value = this.value.replace(/[^0-9\.]/g, '');
                    var charCode = (evt.which) ? evt.which : event.keyCode;
                    return !(charCode > 31 && (charCode < 48 || charCode > 57));
                }
                else if (type == 'float') {
                    var charCode = (evt.which) ? evt.which : event.keyCode;
                    return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
                }
            }

                //else if ispreference
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

//function supress invalid float
function suppressInvalidFloat() {
    $('.div_config_general input:text').each(function () {
        $(this).keyup(function (evt) {
            $(this).closest('div').find('.error').text('');
            var type = $(this).attr('datatype');//datatype of the field
            var text = $(this).val();//textbox value

            if (type == 'float') {
                //$(this).val($(this).val().replace('[0-9]', ''));
                //$(this).val(text.replace(12,2));
            }

        });
    });
}

function IterateThroughTextbox() {
    $('.div_config_general input:text').each(function () {
        if (IsLengthFields(this)) {
            ConvertRangeToFeets(this);
        }
    });
}

//function to check whether the textbox is a length field or not
function IsLengthFields(_this) {
    var isLengthField = false;
    var LengthField = $(_this).parent().closest('td').find('.spanunit').text();
    //console.log(LengthField);
    if (LengthField == 'm' || LengthField == 'ft') {
        isLengthField = true;
    }
    return isLengthField;
}

//function to check whether the textbox is a speed field or not
function IsSpeedField(_this) {
    var isSpeedField = false;
    var speedField = $(_this).parent().closest('td').find('.spanunit').text();
    if (speedField == 'kph' || speedField == 'mph') {
        isSpeedField = true;
    }
    return isSpeedField;
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

//checking whether the user preference is imperial or not
function IsPreference() {
    var isFeet = false;
    var unit = $('#UnitValue').val();
    //console.log(unit);

    if (unit == 692002) {
        isFeet = true;
    }
    return isFeet;
}

//function to convert metres to feet
function ConvertToFeets(_this) {
    //var metres = _this;
    //var metreInches = metres * 39.370078740157477;
    //var feet = Math.ceil(metreInches / 12);
    //var inches = Math.floor(metreInches % 12);
    //var result = feet + '\'' + inches + '\"';
    //return result;

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

//function to convert kph to mph
function ConvertToMph(_this) {
    var kph = _this;
    var mph = kph / 1.6093;
    mph = Math.round(mph * 100) / 100;
    return mph;
}




//function to show data in feet and inches format
function ShowInFeet() {
    $('.div_config_general input:text').each(function () {

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
}

function ShowInFeetSpan() {

    //to search for elements belonging to the class .padd10
    $('.div_config_general').find('.padd10').each(function () {

        // checking whether the user preference is imperial or metric
        if (IsPreference()) {
            var data = $(this).text();
            var speedField = $(this).parent().closest('div').find('.text-normal').text();
            var unit = speedField.search('ft'); // checking whether it is a length field
            if (unit > 0) {                
                data = ConvertToFeets(data);
                if (data != "0\'0\"") {
                    $(this).text(data);
                }
                else {
                    $(this).text("0\'0\"");
                }
            }
        }
    });
}