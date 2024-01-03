//function to convert range for length fields into feet and inches
function ConvertRangeToFeet(_this) {

    $(_this).closest('div').find('.error').text('');
    var range = $(_this).attr('range');//min and max range
    var splitRange = range.split(',');
    var minval = splitRange[0];
    var maxval = splitRange[1];
    if (IsPreference()) {

        if (minval == undefined || maxval == undefined) {
            range = $(_this).attr('range', '');
        }
        else {
            var minvalFt = ConvertToFeet(minval);
            var maxvalFt = ConvertToFeet(maxval);
            range = $(_this).attr('range', '' + minvalFt + ',' + maxvalFt + '');
        }
       
    }
    return range;
   
}
//function to convert feet and inches to metres
function ConvertToMetres(_this) {

    var text = _this;
    var onlyInch = 0;
    var onlyFeet = 0;
    var feet;
    var inches;

    if (text == "0\'0\"") {
        return null;
    }

    //to check whether the value is entered only in feet
    if (text.indexOf('\'') === -1 && text.indexOf('\"') === -1) {
        text = text + '\'' + 0 + '\"';
    }
        //to check whether the value is entered only in inches
    else if (text.indexOf('\'') === -1) {
        onlyInch = 1;
    }

    text = text.replace('\"', '');
    if (onlyInch == 0) {
        var splitValue = text.split('\'');
        feet = splitValue[0];
        inches = splitValue[1];
        if (inches == undefined) {
            inches = 0;
        }
    }
    else {
        feet = 0;
        inches = text;
    }

    var totalInches = feet * 12;
    var Inches = totalInches + (inches * 1);
    //var metres = Inches / 39.370;
    var cm = Inches * 2.54;
    var metres = cm / 100;
    metres = metres.toFixed(4);
    return metres;
}
//function to convert metres to feet
function ConvertToFeet(_this) {

    //var metres = _this;
    //var metreInches = metres * 39.370078740157477;
    //var feet = Math.floor(metreInches / 12);
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
function IterateThroughText() {
    $('.dynamic input:text').each(function () {
        if (IsLengthField(this)) {
            ConvertRangeToFeet(this);
        }
    });
}
//function to check whether the textbox is a length field or not
function IsLengthField(_this) {
    var isLengthField = false;
    var LengthField = $(_this).parent().closest('div').find('.text-normal').text();
    if (LengthField == 'm'|| LengthField == 'ft') {
        isLengthField = true;
    }
    return isLengthField;
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
//function to show data in feet and inches format
function ShowFeet() {
    $('.dynamic input:text').each(function () {
        
        if (IsPreference()) {

            if (IsLengthField(this)) {
                var data = $(this).val();
                if (data.indexOf('\'') === -1) {
                    data = ConvertToFeet(data);
                    if (data != "0\'0\"" && data != "") {
                        $(this).val(data);
                    }
                    else {
                        $(this).val("");
                    }
                }
            }
        }



    })
}
//function to convert mph to kph
function ConvertToKph(_this)
{
    var mph = _this;
    var kph = mph * 1.6093;
    return kph;
}
//function to convert kph to mph
function ConvertToMph(_this)
{
    var kph = this;
    var mph = (kph / 1.6093).toFixed(2);
    return mph;
}