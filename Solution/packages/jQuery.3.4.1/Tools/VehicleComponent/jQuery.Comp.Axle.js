

var initCount = 0;
var initData = [];

var fromConfig = false;

$(function () {
    debugger;
    ShowAxleInFeet();

    //This function allows only alphanumeric characters and '/' for the tyresize field.
    $('.cstyreSize').bind('keypress', function (event) {
        var regex = new RegExp("^[a-z0-9/. ]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    $(".page_help").attr('url', '../../Content/ESDAL2_help/axle.html');

    AxleSupressAlpha();

    AxleSupressNumber();

    clearErrorSpan();

    HeaderHeight();

    //fill header
    FillHeaderOnLoad();

    //function show hide header tyre space
    ShowHideHeaderTyreSpace();

    ReadOldData();

    $('#PageNum').val(3); //page number set to 3

    if (typeof HideFinishButton == 'function') {
        HideFinishButton();
    }

    //if ($.browser.msie) {
    //    IEBrowser();
    //}
    //else {
    //    OtherBrowsers();
    //}

    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        IEBrowser();
    }
    else {
        OtherBrowsers();
    }

});


function ShowHideHeaderTyreSpace() {
    if ($('.cstable').length == 0) {
        $('.headgrad2').text('');
    }
    else {
        //$('.headgrad2').show();
    }
}

//function to set header height
function HeaderHeight() {

    var height = $('#tbl_Axle').find('thead th:eq(0)').height();
    $('.headgradBtn').attr('style', 'height:' + height + 'px;');
}

//function for validationaxle
function validationaxle(x) {
}


function SaveAxleData() {
    debugger;
    var axleweightsum = 0;
      
    /*********************************************************************************************************************
      code removed for Distance to next axle validation as requested in RM #5583 comment #11 on 2016-01-20 by Anlet
    *********************************************************************************************************************/
    //var nxtAxleDistSum = 0;
    //var spaceToFollowing = $('#DistanceToNxtAxle').val();
    //var currentAxleDistSum = $('#AxleDistanceSum').val();
       
    //if (currentAxleDistSum == null || currentAxleDistSum == '' || currentAxleDistSum == undefined) {
    //    nxtAxleDistSum = parseFloat(spaceToFollowing);
    //}
    //else {
    //    nxtAxleDistSum = parseFloat(spaceToFollowing) + parseFloat(currentAxleDistSum);
    //}

    // to calculate the sum of axle weight
    $('#Config-body').find('.axleweight').each(function () {
        var _thisVal = $(this).val();
        axleweightsum = axleweightsum + parseFloat(_thisVal);
    });

    /*********************************************************************************************************************
     code removed for Distance to next axle validation as requested in RM #5583 comment #11 on 2016-01-20 by Anlet
     *********************************************************************************************************************/
    //// to calculate the sum of distance to next axle
    //$('#Config-body').find('.disttonext').each(function () {
    //    var _thisVal = $(this).val();
    //    nxtAxleDistSum = nxtAxleDistSum + parseFloat(_thisVal);
    //});
    
    if (axleweightsum == 0) {
        $('#div_component_general_page').find('.axleweight').each(function () {
            var _thisVal = $(this).val();
            axleweightsum = axleweightsum + parseFloat(_thisVal);
        });
    }
    var result = checkaxle_weight(axleweightsum);

    /**********************************************
      Developer   : Anlet
      Added on    : 16 Jan 2017
      Modified on : 16 Feb 2017 (for comment #15)
      Purpose     : For RM #6037 comment #10
    **********************************************/
    var isNotify = $('#ISNotif').val();
    var vehicleClassCode = $('#MovementClassification').val();
    var excessAxleWgt = false;
    if (result && isNotify) {        
        $('#div_component_general_page').find('.axleweight').each(function () {            
            var currentAxleWgt = $(this).val();

            // for STGO AIL Cat 1 notifcation && for stgo mobile crane cat a
            if ((vehicleClassCode == 241003 || vehicleClassCode == 241006) && parseFloat(currentAxleWgt) > 11500) {
                excessAxleWgt = true;
                $('.error').text('Max axle weight should be less than or equal to 11500 kg');
            }
            // for STGO AIL Cat 2 notifcation && for stgo mobile crane cat b
            else if ((vehicleClassCode == 241004 || vehicleClassCode == 241007) && parseFloat(currentAxleWgt) > 12500) {
                excessAxleWgt = true;
                $('.error').text('Max axle weight should be less than or equal to 12500 kg');
            }
                // for STGO AIL Cat 3 notifcation && for stgo mobile crane cat c
            else if ((vehicleClassCode == 241005 || vehicleClassCode == 241008) && parseFloat(currentAxleWgt) > 16500) {
                excessAxleWgt = true;
                $('.error').text('Max axle weight should be less than or equal to 16500 kg');
            }
        });                 
    }
    /************************************** Modification for RM #6037 ends here *****************************************/

    /*********************************************************************************************************************
    code removed for Distance to next axle validation as requested in RM #5583 comment #11 on 2016-01-20 by Anlet
    *********************************************************************************************************************/
    //var validAxleDist = checkValidAxleDistance(nxtAxleDistSum);
    //if (ValidateAxles() && result && validAxleDist) {
    if (ValidateAxles() && result && !excessAxleWgt) {
        GetDataToSave();
    }
    else {
        var componentName = $('#Internal_Name').val();
        alert('Component  "' + componentName + '"  saved successfully');
        location.reload();
    }
}


function AxleCopyFromPrev(num) {

    debugger;

    var i = 1;
    if (ValidateAxlesRow(num - 1)) {
        var _thisWheelVal = $('#tbl_Axle tbody tr').eq(num - 1).find('.nowheels').val();
        $('#tbl_Axle tbody tr:eq(' + num + ')').each(function () {
            var _thisWheel = $(this).find('.nowheels');
            copyTyreSpace(_thisWheel, _thisWheelVal);
        });


        $('#tbl_Axle tbody tr').eq(num - 1).find('input, select').each(function () {
            var _thisValue = $(this).val();
            $('#tbl_Axle tbody tr:eq(' + num + ')').each(function () {
                $(this).find('td:eq(' + i + ')').find('input:text').val(_thisValue);
            });
            i++;
        });
    }

    FillHeaderOnLoad();
}


function AxleCopyToAll() {
    debugger;
    var i = 1;
    if (ValidateAxlesRow(0)) {

        var _thisWheelVal = $('#tbl_Axle tbody tr').eq(0).find('.nowheels').val();
        $('#tbl_Axle tbody tr:not(:eq(0))').each(function () {
            var _thisWheel = $(this).find('.nowheels');
            copyTyreSpace(_thisWheel, _thisWheelVal);
        });


        $('#tbl_Axle tbody tr').eq(0).find('input:text').each(function () {
            var _thisValue = $(this).val();
            $('#tbl_Axle tbody tr:not(:eq(0))').each(function () {
                $(this).find('td:eq(' + i + ')').find('input:text').val(_thisValue);
            });
            i++;
        });
    }

    FillHeaderOnLoad();
    //});
}

function AxleSupressAlpha() {
    /*$('.axletext').live('keypress', function (evt)*/
    $('#tbl_Axle').on('keypress', '.axletext', function (evt) {
        var unitvalue = $('#UnitValue').val();
        if (unitvalue != 692002) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        }
    });
}

function AxleSupressNumber() {
    /*$('.axletextWheel').live('keypress', function (evt)*/
    $('#tbl_Axle').on('keypress', '.axletextWheel', function (evt)  {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode > 31 && (charCode < 48 || charCode > 57));
    });
}

function ValidateAxlesRow(num) {
    debugger;
    var isvalid = true;
    var movementId = $('#movementTypeId').val();
    
    $('#tbl_Axle tbody tr:eq(' + num + ')').find('input:text').each(function () {
        var this_txt = $(this).val();
        var type = $(this).attr('datatype');//datatype of the field
        var range = $(this).attr('range');//min and max range
        var field = $(this).attr('validationmsg');//textbox id/name field
        var name = $(this).attr('name');//name  

        var minval = '';
        var maxval = '';
        if (range != undefined) {
            var splitRange = range.split(',');
            if (splitRange.length > 1) {
                minval = splitRange[0];
                maxval = splitRange[1];
            }
        }
        if (this_txt == '' && (name != 'tyresize' && name.charAt(0) != 'q')) {
            isvalid = false;
            $('.error').text('All fields except tyre size and centre spacing are required');
            $(this).focus();
            return false;
        }
        else if (movementId != 270006 && (parseFloat(this_txt) < parseFloat(minval) )) {
            $('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
            isvalid = false;
            $(this).focus();
            return false;
        }
        else if (movementId == 270006 && (parseFloat(this_txt) == 0) && (field != 'Internal Name' && field != 'Formal Name' && field != 'Notes')) {
            
            $('.error').text(field + ' must be greater than 0');
            isvalid = false;
            $(this).focus();
            return false;
        }
    });
    return isvalid;
}

function ValidateAxles() {
    var isvalid = true;
    var unitvalue = $('#UnitValue').val();
    var movementId = $('#movementTypeId').val();

    $('#tbl_Axle tbody').find('input:text').each(function () {
        var this_txt = $(this).val();
        var type = $(this).attr('datatype');//datatype of the field
        var range = $(this).attr('range');//min and max range
        var field = $(this).attr('validationmsg');//textbox id/name field
        var name = $(this).attr('name');//name

        var minval = '';
        var maxval = '';
        var minvalfeet = '';
        var maxvalfeet = '';
        var valuemetres;

        if (range != undefined) {
            var splitRange = range.split(',');
            if (splitRange.length > 1) {
                minval = splitRange[0];
                maxval = splitRange[1];
            }
            /*********************************************************************************************************************
             * code removed for Distance to next axle validation as requested in RM #5583 comment #11 on 2016-01-20 by Anlet
            *********************************************************************************************************************/
            //if (field == 'Distance to next axle' && unitvalue == 692002) {
            //    minvalfeet = ConvertToFeet(minval);
            //    maxvalfeet = ConvertToFeet(maxval);
            //    valuemetres = ConvertToMetres(this_txt);
            //    this_txt = valuemetres;
            //}
           
        }


        if (this_txt == '' && (name.charAt(0) != 'q' && name != 'tyresize')) {
         
            isvalid = false;
            $('.error').text('All fields except tyre size and centre spacing are required');
            $(this).focus();
            return false;
        }
        /*********************************************************************************************************************
        * code removed for Distance to next axle validation as requested in RM #5583 comment #11 on 2016-01-20 by Anlet
        *********************************************************************************************************************/
        //else if (minval != '' && maxval != '' && movementId != 270006) {
            
        //    if (parseFloat(this_txt) < parseFloat(minval) || parseFloat(this_txt) > parseFloat(maxval)) {
        //        if (field == 'Distance to next axle' && unitvalue == 692002) {
        //            $('.error').text(field + ' must be in a range between ' + minvalfeet + ' and ' + maxvalfeet);
        //        }
        //        else {
        //            $('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
        //        }
        //            isvalid = false;
        //            $(this).focus();
        //            return false;
        //        }
        //}
      /*********************************************************************************************************************/
    });
    return isvalid;
}

function AjaxSaveAxle(axlesList) {
    var isEdit = $('#IsEdit').val();
    if (isEdit == "") {
        isEdit = false;
    }
    var componentId = $('#Component_Id').val();
    var componentName = $('#Internal_Name').val();
    $.ajax({
        url: '../Vehicle/SaveAxles',
        data: '{"axleList":' + JSON.stringify(axlesList) + ',"componentId":' + componentId + ',"isEdit":' + isEdit + '}',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        async: false,
        success: function (data) {
            if (data.success) {
                if (fromConfig) {
                    NavigateToSummary();
                }
                else {
                        alert('Component  "' + componentName + '"  saved successfully');
                        location.reload();
                    //showWarningPopDialog('Component  "' + componentName + '"  saved successfully', 'Ok', '', 'ReloadLocation', 'ShowRegisterPage', 1, 'info');
                }
            }
        }
    });
}

//for application
function AjaxSaveAppVehAxle(axlesList) {
    var componentId = $('#Component_Id').val();
    var componentName = $('#Internal_Name').val();
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var configId = GetConfigurationId();
    var isEdit = $('#IsEdit').val();
    $.ajax({
        url: '../Vehicle/SaveAxles',
        data: '{"axleList":' + JSON.stringify(axlesList) + ',"componentId":' + componentId + ',"isApplication":' + true + ',"appRevID":' + ApplicationRevId + ',"vehicleId":' + configId + ',"isEdit":' + isEdit + '}',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        async: false,
        success: function (data) {
            if (data.success) {
                if (fromConfig) {
                    NavigateToSummary();
                }
                else {

                    showWarningPopDialog('Component  "' + componentName + '"  saved successfully', 'Ok', '', 'ReloadLocation', 'ShowRegisterPage', 1, 'info');
                }
            }
        }
    });
}

//for VR1 application
function AjaxSaveVR1VehAxle(axlesList) {
    var componentId = $('#Component_Id').val();
    var componentName = $('#Internal_Name').val();
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var notifid = $("#NotificatinId").val() ? $('#NotificatinId').val() : 0;
    var configId = GetConfigurationId();
    var isEdit = $('#IsEdit').val();
    $.ajax({ 
        url: '../Vehicle/SaveAxles',
        data: '{"axleList":' + JSON.stringify(axlesList) + ',"componentId":' + componentId + ',"isApplication":' + true + ',"isVR1":' + true + ',"appRevID":' + ApplicationRevId + ',"vehicleId":' + configId + ',"NotificationID":' + notifid + ',"isEdit":' + isEdit + '}',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        async: false,
        success: function (data) {
            if (data.success) {
                if (fromConfig) {
                    NavigateToSummary();
                }
                else {

                    showWarningPopDialog('Component  "' + componentName + '"  saved successfully', 'Ok', '', 'ReloadLocation', 'ShowRegisterPage', 1, 'info');
                }
            }
        }
    });
}

function GetDataToSave() {
    var axlesList = []; //Axle list
    var i = 1;
    var unitvalue = $('#UnitValue').val();
    var wb = 0;
    $('#tbl_Axle tbody tr').each(function () {
        var axleNum = i;
        var noOfWheels = $(this).find('.nowheels').val();
        var axleWeight = $(this).find('.axleweight').val();
        var distanceToNxtAxl = $(this).find('.disttonext').val();

        //to check if distance tonext axle value is definedor not
        if (typeof distanceToNxtAxl !== "undefined") {
            if (unitvalue == 692002) {
                distanceToNxtAxl = ConvertToMetres(distanceToNxtAxl);
            }
            wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);
        }

        var tyreSize = $(this).find('.tyresize').val();

        var tyreSpace = null;
        $(this).find('.cstable input:text').each(function () {
            var _thistxt = $(this).val();
            if (tyreSpace != null) {
                tyreSpace = tyreSpace + "," + _thistxt;              
            }
            else {
                tyreSpace = _thistxt;
            }
            if (unitvalue == 692002) {
                tyreSpace = ConvertToMetres(tyreSpace);
            }
        });

        axlesList.push({ AxleNumId: axleNum, NoOfWheels: noOfWheels, AxleWeight: axleWeight, DistanceToNextAxle: distanceToNxtAxl, TyreSize: tyreSize, TyreCenters: tyreSpace });
        i++;
    });
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();
    var validwb = true;
    var axlecount = $('.axledrop').val();  //added for crane workflow changes
    if (isNotify == 'True' || isNotify == 'true') {
        var vc = $("#VehicleClass").val();
        var catAwb = 1.5 * axlecount + 0.5 * (axlecount % 2);
        if (vc == 241006) {
            if (parseFloat(wb) < parseFloat(catAwb)) {
                $('.error').text('Wheelbase should be greater than or equal to ' + catAwb);
                validwb = false;
            }
        }
        isVR1 = 'True';
    }
    if (validwb) {
        if (ApplicationRevId == 0 && !isVR1) {
            AjaxSaveAxle(axlesList);
        }
        else if (isVR1 == 'True') {
            AjaxSaveVR1VehAxle(axlesList);
        }
        else {
            AjaxSaveAppVehAxle(axlesList);
        }
    }
}

//function to clear error span message
function clearErrorSpan() {
    /*$('input').live('focus', function ()*/
    $('body').on('focus', 'input', function (){
        $('.validationError').text('');
        $('.error').text(''); 
    });
}


//function for adding tyre spacing text
function addColumns(_this) {
    var _thisvalue = $(_this).val();
    var _headerlen = $('.headgrad1').length;

    var numOfWheels = parseInt(_thisvalue);
    var headLength = parseInt(_headerlen);

    if ($('#ConfigTyreSpace').length > 0) {

        $('.headgrad2').text('Tyre centre spacing');
        var height = $('#tbl_Axle').find('thead th:eq(0)').height();
        $('.headgradBtn').attr('style', 'height:' + height + 'px;');
    }

    var greatestVal = 0;
    $('.nowheels').each(function () {
        var _thisWheelVal = $(this).val();
        if (greatestVal < parseInt(_thisWheelVal)) {
            greatestVal = parseInt(_thisWheelVal);
        }
    });

    if ((headLength + 1) < greatestVal) {
        $('.headgrad2').attr('colspan', numOfWheels - 1);
        $('.centerspace1').attr('colspan', numOfWheels - 1);
    }
    $('.headgrad1').remove();
    for (var j = 1; j < greatestVal; j++) {
        $('.sub').append('<th class="headgrad1">' + j + '</th>');
        $('.headgrad2').show();
    }

    $(_this).closest('tr').find('.cstable').remove();
    for (var i = 1; i < numOfWheels; i++) {
        $(_this).closest('tr').find('.cstyreSize').after('<td class="cstable"><input type="text" value="" name="q" class="axletext frmbdr tyrec3" Maxlength="5"/></td>');
    }

    var height = $('#tbl_Axle').find('thead th:eq(0)').height();
    $('.headgradBtn').attr('style', 'height:' + height + 'px;');
}

//function for adding column  head on load
function addHeader() {
}

function IEBrowser() {
    var _this;
    var isvalid = true;
    $('.axlewrapper input:text').focus(function () {
        if (_this != null) {
            isvalid = ValidateOnBlur(_this);
        }
    });

    $('.axlewrapper input:text').blur(function () {
        if (isvalid) {
            _this = $(this);
        }
    });
}

function OtherBrowsers() {
    /*$('.axlewrapper input:text').live('blur', function ()*/
    $('.axlewrapper').on('blur', 'input:text', function ()  {
        ValidateOnBlur(this);
    });
}


function ValidateOnBlur(x) {
    var isvalid = true;
    var required = $(x).attr('isrequired');//Is required
    var range = $(x).attr('range');//Range
    var field = $(x).attr('validationmsg');//Validation string
    var text = $(x).val();//value 
    var movementId = $('#movementTypeId').val();
    if (range != undefined) {

        var splitRange = range.split(',');
        var minval = splitRange[0];
        var maxval = splitRange[1];
    }

    if (required == 1 && text == '') {
        $('.axlewrapper').find('.error').text(field + ' is required');
        isvalid = false;

        //$(x).focus();
        return false;
    }
    //else if (movementId == 270006 && parseFloat(text) < parseFloat(minval)) {
    //    $('.axlewrapper').find('.error').text(field + ' must be greater than ' + minval);
    //    isvalid = false;

    //    //$(x).focus();
    //    return false;
    //}
    else if (movementId != 270006 && (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval))) {
        $('.axlewrapper').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
        isvalid = false;

        //$(x).focus();
        return false;
    }
    else {
        $('.axlewrapper').find('.error').text('');
        if ($(x).attr('id') == 'wheels') {
            addColumns(x);
        }
    }

    return isvalid;
}

//function checkValidAxleDistance(distanceSum) {
//    var result = true;
//    var vhclLength = $('#OverallLength').val();

//    if (vhclLength != null || vhclLength != '' || vhclLength != undefined) {
//        if (distanceSum > vhclLength) {
//            $('.axlewrapper').find('.error').text('Total axle distance must be less than vehicle length');
//            result = false;
//        }
//        else {
//            $('#AxleDistanceSum').val(distanceSum);
//        }
//    }
   
//    return result;
//}
function checkaxle_weight(x) {
    var result = true;
    var range = $('#axleweight_range').val();
    var splitRange = range.split(',');
    var minval = splitRange[0];
    var maxval = splitRange[1];
    if (parseFloat(x) < parseFloat(minval) || parseFloat(x) > parseFloat(maxval)) {
        $('.axlewrapper').find('.error').text('Total axle weight must be in a tolerance range between ' + minval + ' and ' + maxval);
        result = false;
    }
    return result;
}

function copyTyreSpace(_this, numOfWheels) {
    $(_this).closest('tr').find('.cstable').remove();
    for (var i = 1; i < numOfWheels; i++) {
        $(_this).closest('tr').find('.cstyreSize').after('<td class="cstable"><input type="text" value="" name="q" class="axletext frmbdr tyrec3" Maxlength="5"/></td>');
    }
}

function FillHeaderOnLoad() {
    var _headerlen = $('.headgrad1').length;

    var headLength = parseInt(_headerlen);

    var greatestVal = 0;
    $('.nowheels').each(function () {
        var _thisWheelVal = $(this).val();
        if (greatestVal < parseInt(_thisWheelVal)) {
            greatestVal = parseInt(_thisWheelVal);
        }
    });

    if ((headLength + 1) < greatestVal) {
        $('.headgrad2').attr('colspan', greatestVal - 1);
        $('.centerspace1').attr('colspan', greatestVal - 1);
    }
    $('.headgrad1').remove();
    for (var j = 1; j < greatestVal; j++) {
        $('.sub').append('<th class="headgrad1">' + j + '</th>');
        if ($('#ConfigTyreSpace').length > 0) {
            $('.headgradBtn').attr('style', 'height:53px;');
        }
    }
}


function ReadOldData() {
    initCount = $('#tbl_Axle').find('input:text').length;
    $('#tbl_Axle').find('input:text').each(function () {
        var _txt = $(this).val();
        initData.push(_txt);
    });
}

function IsChangedData() {
    var hasChange = false;
    var newCount = $('#tbl_Axle').find('input:text').length;

    if (initCount != newCount) {
        hasChange = true;
    }
    else {
        hasChange = CompareData();
    }

    return hasChange;
}

function CompareData() {
    var hasChange = false;
    var i = 0;
    $('#tbl_Axle').find('input:text').each(function () {
        var _txt = $(this).val();
        if (_txt != initData[i]) {
            hasChange = true;
            return false;
        }
        i++;
    });
    return hasChange;
}


function ValidateAndNavigate() {
    fromConfig = true;
    SaveAxleData();
}

function ShowAxleInFeet() {
    var unitvalue = $('#UnitValue').val();
    
    if (unitvalue == 692002) {
        $('#tbl_Axle tbody tr').each(function () {
            var distanceToNxtAxl = $(this).find('.disttonext').val();
            distanceToNxtAxl = ConvertToFeet(distanceToNxtAxl);
            $(this).find('.disttonext').val(distanceToNxtAxl);

            var tyreSpace = null;
            $(this).find('.cstable input:text').each(function () {
                var _thistxt = $(this).val();
                if (_thistxt != undefined) {
                    _thistxt = ConvertToFeet(_thistxt);
                    $(this).val(_thistxt);
                }

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
