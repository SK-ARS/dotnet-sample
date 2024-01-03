
    $(function () {
        ShowHideHeaderTyreSpace();
        $(".page_help").attr('url', '../../Home/NotImplemented');
        //function header tyre space count
        HeaderTyreSpaceCount();
        DynamicTitle();
        ShowInFeet();
    });


//function to show tyre spacing header
function ShowHideHeaderTyreSpace() {
    
    if ($('.wheel_space').length == 0) {
        $('.headgrad2').hide();
        $('.sub1').hide();
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

//function to get Header Tyre Space Count
function HeaderTyreSpaceCount() {
    var grtValue = 0;
    $('#axlePage table tr').each(function () {
        var _thisVal = parseInt($(this).find('.wheel_space').length);
        if (_thisVal > grtValue) {
            grtValue = _thisVal;
        }
    });
    for (var i = 1; i <= grtValue; i++) {
        $('.tyreSpaceCnt').append('<th class="headgrad1">' + i + '</th>');
    }
    $('.headgrad2').attr('colspan', grtValue);
}


//function Dynamic Title
function DynamicTitle() {
    $('.dyntitle').html('View component');
}

//function to display the value in feet and inches while viewing component
function ShowInFeet() {
    var unit = $('#UnitValue').val();
    if (unit == 692002) {
        $('#div_general table td:nth-child(2)').each(function () {
                
            var data = $(this).text();
            var newdata;
            if (IsLength(this)) {
                newdata = data.replace('ft', '');
                data = ConvertToFeet(newdata) + ' ft';
            }
            //else if (IsSpeedField(this)) {
            //    newdata = data.replace('mph', '');
            //    data = ConvertToMph(newdata) + ' mph';
            //}
            $(this).html(data);
        });

        $('#axlePage table td:nth-child(4)').each(function () {
            var _thisVal = $(this).text();
            if (_thisVal.indexOf('\'') === -1 && _thisVal.indexOf('\"') === -1) {
                if (_thisVal != null || _thisVal != undefined) {
                    _thisVal = ConvertToFeet(_thisVal);
                }
                $(this).text(_thisVal);
            }
            
        });
                
          $('td.wheel_space').map(function () {
              var _thisWheelVal = $(this).text();
              if (_thisWheelVal.indexOf('\'') === -1 && _thisWheelVal.indexOf('\"') === -1) {
                  if (_thisWheelVal != null || _thisWheelVal != undefined) {
                      _thisWheelVal = ConvertToFeet(_thisWheelVal);
                  }
                  $(this).text(_thisWheelVal);
              }
          }).get();

            //$('#axlePage table tr').each(function () {
            //var _thisWheelVal = $(this).find('.wheel_space').text();
            //if (_thisWheelVal.indexOf('\'') === -1 && _thisWheelVal.indexOf('\"') === -1) {
            //    if (_thisWheelVal != null || _thisWheelVal != undefined) {
            //        _thisWheelVal = ConvertToFeet(_thisWheelVal);
            //    }
            //    $(this).find('.wheel_space').text(_thisWheelVal);
            //}
        //});
       
    }
        
}

//function to check whether the field is a length field or not for View Component
function IsLength(_this) {
    var isLengthField = false;
    var LengthField = $(_this).find('.span_unit').text();
    if (LengthField == 'ft') {
        isLengthField = true;
    }
    return isLengthField;
}
    

//function to check whether the textbox is a speed field or not
function IsSpeedField(_this) {
    var isSpeedField = false;
    var speedField = $(_this).find('.span_unit').text();
    if (speedField == 'mph') {
        isSpeedField = true;
    }
    return isSpeedField;
}

//function to convert kph to mph
function ConvertToMph(_this) {
    var kph = _this;
    var mph = kph / 1.6093;
    mph = Math.round(mph * 100) / 100;
    return mph;
}