var initCount = 0;
var initData = [];
var fromConfig = false;

$(document).ready(function () {

    $('body').on('keypress', '.cstyreSize', function (event) {
        var regex = new RegExp("^[a-z0-9/. ]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        IEBrowser();
    }
    else {
        $('body').on('blur', '.axlewrapper input:text', function (event) {
            if ($(this).is(':visible')) {
                ValidateOnBlur(this);

                var componentId = $(this).closest('.axle').data('componentid');
                AxleTableMethods.CreateAxlesObj(componentId);
            }
        });
    }

    $(window).resize(function () {
        var compId = $('.component-item:visible #Component_Id').val();
        HeaderHeight(compId);
    });
});

function Comp_AxleInit(flag = 0, compId=0) {
    ShowAxleInFeet();
    //This function allows only alphanumeric characters and '/' for the tyresize field.


    $(".page_help").attr('url', '../../Content/ESDAL2_help/axle.html');

    AxleSupressAlpha();
    AxleSupressNumber();
    clearErrorSpan();
    HeaderHeight();
    //fill header
    FillHeaderOnLoad();
    //function show hide header tyre space
    if (flag == 0)
        ShowHideHeaderTyreSpaceCompAxle(compId);
    ReadOldData();
    $('#PageNum').val(3); //page number set to 3
    if (typeof HideFinishButton == 'function') {
        HideFinishButton();
    }

    $('[data-toggle="tooltip"]').tooltip();

};
function ShowHideHeaderTyreSpaceCompAxle(compId = 0) {
    var parentElemId = "";
    var isValid = $('.wheel_space:visible').length == 0 && ($('.nowheels:visible').val() == "" || $('.nowheels:visible').val() == undefined);
    if (compId > 0) {
        parentElemId = '#' + compId;
        isValid = $(parentElemId + ' .wheel_space').length == 0 && ($(parentElemId + ' .nowheels').val() == "" || $(parentElemId + ' .nowheels').val() == undefined);
    }
    if (isValid) {
        $(parentElemId +' .headgrad2').hide();
        $(parentElemId +' td.cstable').remove();
        $(parentElemId +' .sub1').hide();
    }
    else {
        $(parentElemId +' .headgrad2').show();
        $(parentElemId +' .sub1').show();
        //var x = $('.headgrad2').val();
        //x.bold();
    }

    if ($(parentElemId +' .tyre_size').length == 0) {
        $(parentElemId +' .headgrad_tyreSize').hide();
    }
    else {
        $(parentElemId +' .headgrad_tyreSize').show();
    }
}
//function to set header height

function HeaderHeight(componentId = 0) {
    var isModalOpened = $('#axleDetails.modal').is(':visible');
    if (componentId != 0) {
        var height = $('#' + componentId).find('.individualComponentAxle #tbl_Axle thead th:eq(0)').height();
        if (height != undefined) {
            var paddingtop = $('#' + componentId).find('.individualComponentAxle #tbl_Axle thead th:eq(0)').css('padding-top') || "0px";
            var paddingbottom = $('#' + componentId).find('.individualComponentAxle #tbl_Axle thead th:eq(0)').css('padding-bottom') || "0px";
            var paddingtopvalue = paddingtop.replace('px', '');
            var paddingbottomvalue = paddingbottom.replace('px', '');
            var totalheight = parseFloat(height) + parseFloat(paddingtopvalue) + parseFloat(paddingbottomvalue);
            $('#' + componentId).find('.headgradBtn').attr('style', 'height:' + totalheight + 'px;color:none;');
        }
    }
    else {
        var elem = !isModalOpened ? '.individualComponentAxle' : '#axleDetails.modal .individualComponentAxle';
        if ($(elem).length > 0) {
            var height = $(elem + ' #tbl_Axle').find('thead th:eq(0)').height();
            var paddingtop = $(elem + ' #tbl_Axle').find('thead th:eq(0)').css('padding-top') || "0px";
            var paddingbottom = $(elem + ' #tbl_Axle').find('thead th:eq(0)').css('padding-bottom') || "0px";
            if (paddingtop != undefined && paddingbottom != undefined) {
                var paddingtopvalue = paddingtop.replace('px', '');
                var paddingbottomvalue = paddingbottom.replace('px', '');
                var totalheight = parseFloat(height) + parseFloat(paddingtopvalue) + parseFloat(paddingbottomvalue);
                if (($(elem).find('#IsFromConfig').val() == undefined || $(elem).find('#IsFromConfig').val() == 0) && $(elem).find('#ConfigTyreSpace').val() == "True") {
                    totalheight = totalheight - 5;
                }
                if (totalheight != 0)
                    $(elem).find('.headgradBtn').attr('style', 'height:' + totalheight + 'px;color:none;');
            }
        }
    }
}
//function for validationaxle
function validationaxle(x) {
}
function SaveAxleData() {
    ;
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

    if (ValidateAxles() && result && !excessAxleWgt) {
        GetDataToSave();
    }
    else {
        var componentName = $('#Internal_Name').val();
        alert('Component  "' + componentName + '"  saved successfully');
        location.reload();
    }
}
function AxleCopyFromPrev(num, _this) {

    var i = 1;
    var j = num - 1;
    if (ValidateAxlesRow(num - 2, _this)) {
        if ($("#divAllComponent").find('.comp').html() != undefined) {
            if ($("#divAllComponent").find('.comp').html().trim().length > 0) {
                var div = $(_this).parent().closest('.comp').attr('id');
                if (div == undefined)
                    div = "axleDetails";
                var _thisWheelVal = $('#' + div + ' #tbl_Axle tbody tr').eq(num - 2).find('.nowheels').val();
                $('#' + div + ' #tbl_Axle tbody tr:eq(' + j + ')').each(function () {
                    var _thisWheel = $(this).find('.nowheels');
                    copyTyreSpace(_thisWheel, _thisWheelVal);
                });


                $('#' + div + ' #tbl_Axle tbody tr').eq(num - 2).find('input, select').each(function () {
                    var _thisValue = $(this).val();
                    $('#' + div + ' #tbl_Axle tbody tr:eq(' + j  + ')').each(function () {
                        $(this).find('td:eq(' + i + ')').find('input:text').val(_thisValue);
                    });
                    i++;
                });
            }
        }
        else {
            var _thisWheelVal = $('#tbl_Axle tbody tr').eq(num - 2).find('.nowheels').val();
            $('#tbl_Axle tbody tr:eq(' + j + ')').each(function () {
                var _thisWheel = $(this).find('.nowheels');
                copyTyreSpace(_thisWheel, _thisWheelVal);
            });


            $('#tbl_Axle tbody tr').eq(num - 2).find('input, select').each(function () {
                var _thisValue = $(this).val();
                $('#tbl_Axle tbody tr:eq(' + j + ')').each(function () {
                    $(this).find('td:eq(' + i + ')').find('input:text').val(_thisValue);
                });
                i++;
            });
        }
        if ($('#ConfigTypeId').val() == "244012") {
            var disttonextsum = 0;
            if (disttonextsum == 0) {
                $('.component_axle #tbl_Axle').find('.disttonext').each(function () {
                    var _thisVal = $(this).val() == "" ? 0 : $(this).val();
                    disttonextsum = disttonextsum + parseFloat(_thisVal);
                });
            }
            if (disttonextsum != 0) {
                disttonextsum = disttonextsum.toFixed(2);
                $('#VehicleConfigInfo').find('#Wheelbase').val(disttonextsum);
                $('#VehicleConfigInfo').find('#Wheelbase').trigger('blur');
            }
        }
    }
    MaxAxleWeightUpdate(_this);
    FillHeaderOnLoad();

    var componentId = $(_this).closest('.axle').data('componentid');
    AxleTableMethods.CreateAxlesObj(componentId);
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
    $('#tbl_Axle').on('keypress', '.axletextWheel', function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode > 31 && (charCode < 48 || charCode > 57));
    });
}
function ValidateAxlesRow(num, _this) {
    var isvalid = true;
    var movementId = $('#movementTypeId').val();
    $(_this).closest('.AxleComponent').find('#tbl_Axle tbody tr:eq(' + num + ')').find('input:text').each(function () {
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
        if ((this_txt == '' || parseFloat(this_txt) == 0) && (name != 'tyresize' && name.charAt(0) != 'q')) {
            var configurableAxles = $(this).parent().closest('#div_component_general_page').find('.AxleConfig').val();
            //if (configurableAxles == 'True') {
            isvalid = false;
            $(_this).closest('.axlewrapper').find('.error').text('All fields except tyre size and centre spacing are required');
            $(this).focus();
            return false;
            //}
        }
        else if (parseFloat(this_txt) < parseFloat(minval)) {
            $(_this).closest('.axlewrapper').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
            isvalid = false;
            $(this).focus();
            return false;
        }
        else if (movementId == 270006 && (parseFloat(this_txt) == 0) && (field != 'Internal Name' && field != 'Formal Name' && field != 'Notes')) {

            $(_this).closest('.axlewrapper').find('.error').text(field + ' must be greater than 0');
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

        var configurableAxles = $(this).parent().closest('#div_component_general_page').find('.AxleConfig').val();
        //if (configurableAxles == 'True') {
        if ((this_txt == '' || parseFloat(this_txt) == 0) && (name.charAt(0) != 'q' && name != 'tyresize')) {

            isvalid = false;
            $('.axlewrapper .error').text('All fields except tyre size and centre spacing are required');
            $(this).focus();
            return false;
        }
        isvalid = ComponentKeyUpValidation(this);
        if (!isvalid)
            return isvalid;
        //}
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
    var isFromConfig = $('#hiddenConfigFlag').val();

    var componentName = $('#Internal_Name').val();
    var configFlag = $("#hiddenConfigFlag").val();
    $.ajax({
        url: '../Vehicle/SaveAxles',
        data: '{"axleList":' + JSON.stringify(axlesList) + ',"componentId":' + componentId + ',"isEdit":' + isEdit + ',"isFromConfig":' + isFromConfig + '}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        success: function (data) {

            if (data.success) {
                ShowModalPopup('Component  "' + componentName + '"  saved successfully');
                if (configFlag == 1) {
                    ;
                    ImportComponent(componentId);
                }
                else {
                    location.reload();
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
        //contentType: 'application/json; charset=utf-8',
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
        //contentType: 'application/json; charset=utf-8',
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
    var componentId = $('#Component_Id').val();
    axlesList = [];
    $('#' + componentId + ' #tbl_Axle tbody tr').each(function () {
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
        $(this).find('#' + componentId + ' .cstable input:text').each(function () {
            var _thistxt = $(this).val();
            //if (_thistxt != undefined) {
            //    if (unitvalue == 692002) {
            //        _thistxt = ConvertToMetres(_thistxt);
            //    }
            //}
            if (tyreSpace != null) {
                tyreSpace = tyreSpace + "," + _thistxt;
            }
            else {
                tyreSpace = _thistxt;
            }
        });

        if (noOfWheels != "") {
            axlesList.push({ AxleNumId: axleNum, NoOfWheels: noOfWheels, AxleWeight: axleWeight, DistanceToNextAxle: distanceToNxtAxl, TyreSize: tyreSize, TyreCenters: tyreSpace });
        }
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
    //$('body').on('focus', 'input', function (){
    //    $('.validationError').text('');
    //    $('.error').text(''); 
    //});
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
            var componentId = $(this).closest('.axle').data('componentid');
            AxleTableMethods.CreateAxlesObj(componentId);
        }
    });

    $('.axlewrapper input:text').blur(function () {
        if (isvalid) {
            _this = $(this);
            var componentId = $(this).closest('.axle').data('componentid');
            AxleTableMethods.CreateAxlesObj(componentId);
        }
    });
}
function OtherBrowsers() {
    /*$('.axlewrapper input:text').live('blur', function ()*/

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

    if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
        $('.axlewrapper').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
        isvalid = false;

        return false;
    }
    else {
        $('.axlewrapper').find('.error').text('');
        if ($(x).attr('id') == 'wheels') {
            AxleTableMethods.AddUpdateColumnsOnNoOfWheelsChange(x);
        }
    }

    return isvalid;
}
function checkaxle_weight(x, compId = 0) {

    var result = true;
    if (x != 0) {
        var range;
        if (compId != 0)
            range = $('#' + compId).find('#axleweightRange').val();
        else
            range = $('#axleweightRange').val();
        var splitRange = range.split(',');
        var minval = splitRange[0];
        var maxval = splitRange[1];
        if (parseFloat(x) < parseFloat(minval) || parseFloat(x) > parseFloat(maxval)) {
            var msg = 'Total axle weight must be in a tolerance range between ' + minval + ' and ' + maxval;
            if (compId != 0)
                $('#' + compId).find('.axlewrapper').find('.error').text(msg);
            else
                $('.axlewrapper').find('.error').text(msg);
            result = false;
        }
    }
    return result;
}
function copyTyreSpace(_this, numOfWheels) {
    $(_this).closest('tr').find('.cstable').remove();
    for (var i = 1; i < numOfWheels; i++) {
        $(_this).closest('tr').find('.cstyreSize').after('<td class="cstable"><input type="text" value="" name="q" class="axletext frmbdr tyrec3 txt-wheel-count" Maxlength="5"/></td>');
    }
}
function FillHeaderOnLoad() {

    if ($("#divAllComponent").find('.comp').html() != undefined) {
        if ($("#divAllComponent").find('.comp').html().trim().length > 0) {
            $(".comp").each(function () {

                var compId = $(this).attr('id');

                var _headerlen = $('#' + compId).find('.headgrad1').length;
                var headLength = parseInt(_headerlen);
                var greatestVal = 0;
                $('#' + compId).find('.nowheels').each(function () {
                    var _thisWheelVal = $(this).val();
                    if (greatestVal < parseInt(_thisWheelVal)) {
                        greatestVal = parseInt(_thisWheelVal);
                    }
                });

                if ((headLength + 1) < greatestVal) {
                    $('#' + compId).find('.headgrad2').attr('colspan', greatestVal - 1);
                    $('#' + compId).find('.centerspace1').attr('colspan', greatestVal - 1);
                }
                $('#' + compId).find('.headgrad1').remove();
                for (var j = 1; j < greatestVal; j++) {
                    $('#' + compId).find('.sub').append('<th class="headgrad1">' + j + '</th>');
                    if (j == greatestVal - 1) {
                        $('#' + compId).find('.sub').append('<th style="background-color: rgb(213 223 234) !important;" class="headgrad1 3"></th>');
                    }
                    if ($('#' + compId).find('#ConfigTyreSpace').length > 0) {
                        var height = $('#' + compId).find('#tbl_Axle').find('thead th:eq(0)').height();
                        var paddingtop = $('#' + compId).find('#tbl_Axle').find('thead th:eq(0)').css('padding-top');
                        var paddingbottom = $('#' + compId).find('#tbl_Axle').find('thead th:eq(0)').css('padding-bottom');
                        var paddingtopvalue = paddingtop.replace('px', '');
                        var paddingbottomvalue = paddingbottom.replace('px', '');
                        var totalheight = parseFloat(height) + parseFloat(paddingtopvalue) + parseFloat(paddingbottomvalue);
                        $('#' + compId).find('.headgradBtn').attr('style', 'height:' + totalheight + 'px;');
                    }
                }
            });
        }
    }
    else {
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
            if (j == greatestVal - 1) {
                $('.sub').append('<th style="background-color: rgb(213 223 234) !important;" class="headgrad1 3"></th>');
            }
            if ($('#ConfigTyreSpace').length > 0) {
                var height = $('#tbl_Axle').find('thead th:eq(0)').height();
                var paddingtop = $('#tbl_Axle').find('thead th:eq(0)').css('padding-top');
                var paddingbottom = $('#tbl_Axle').find('thead th:eq(0)').css('padding-bottom');
                var paddingtopvalue = paddingtop.replace('px', '');
                var paddingbottomvalue = paddingbottom.replace('px', '');
                var totalheight = parseFloat(height) + parseFloat(paddingtopvalue) + parseFloat(paddingbottomvalue);
                $('.headgradBtn').attr('style', 'height:' + totalheight + 'px;');
            }
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
            if (distanceToNxtAxl != undefined && distanceToNxtAxl.indexOf('\'') === -1) {
                distanceToNxtAxl = ConvertToFeet(distanceToNxtAxl);
                $(this).find('.disttonext').val(distanceToNxtAxl);
            }
            var tyreSpace = null;
            $(this).find('.cstable input:text').each(function () {
                var _thistxt = $(this).val();
                //if (_thistxt != undefined) {
                //    _thistxt = ConvertToFeet(_thistxt);
                //    $(this).val(_thistxt);
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

//function HeaderTyreSpaceCount() {
//    var grtValue = 0;
//    $('.comp').each(function () {
//        var componentId = $(this).attr("id");
//        $('#' + componentId).find('.AxleComponent table tr').each(function () {
//            var _thisVal = parseInt($(this).find('.wheel_space').length);
//            if (_thisVal > grtValue) {
//                grtValue = _thisVal;
//            }
//        });
//        for (var i = 1; i <= grtValue; i++) {
//            $('#' + componentId).find('.tyreSpaceCnt').append('<th style="width: 10%;">' + i + '</th>');
//        }
//        $('#' + componentId).find('.headgrad2').attr('colspan', grtValue);
//    });
//}

/*
 
 */

var AxleTableMethods = {
    AxlesArr: [],
    AxlesItemsArrUndo: [],
    CreateAxlesObj: function (componentId, fromPopup = false, callbackfn) {
        if (componentId == undefined || componentId == null || componentId == 0)
            return;
        componentId = parseInt(componentId);
        //Remove existing component obj
        //if (fromPopup) {
        //    AxleTableMethods.AxlesArr = [];
        //}
        //else {
            for (var i = AxleTableMethods.AxlesArr.length - 1; i >= 0; --i) {
                if (AxleTableMethods.AxlesArr[i].componentId == componentId) {
                    AxleTableMethods.AxlesArr.splice(i, 1);
                }
            }
        //}

        var i = 1;
        var unitvalue = $('#UnitValue').val();
        var wb = 0;
        var axlesObj = [];
        var elem = '#divComponentData-' + componentId + ' .component_axle table.axle tbody tr';
        if (fromPopup) {
            elem = '.popUpAxleDetails .axlebasic table.axle tbody tr';
        }
        var prevCompId = 0;
        $(elem).each(function () {
            if (fromPopup) {
                componentId = $(this).data('componentid');
                if (prevCompId == 0 || prevCompId != componentId) {
                    i = 1;
                }
                prevCompId = componentId;
            }
            var axleNum = i;
            var noOfWheels = $(this).find('.nowheels').val();
            var axleWeight = $(this).find('.axleweight').val();
            var distanceToNxtAxl = $(this).find('.disttonext').val();

            //to check if distance tonext axle value is definedor not
            if (typeof distanceToNxtAxl !== "undefined") {
                if (unitvalue == 692002) {
                    distanceToNxtAxl = ConverteFeetToMetre(distanceToNxtAxl);
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
            });

            var axleItemObj = {
                componentId: componentId,
                AxleNumId: axleNum, NoOfWheels: noOfWheels,
                AxleWeight: axleWeight, DistanceToNextAxle: distanceToNxtAxl,
                TyreSize: tyreSize, TyreCenters: tyreSpace
            };

            if (fromPopup) {
                //We are listing all items here. So we need to update axle items directly
                var currentComponentAxles = AxleTableMethods.AxlesArr.filter(function (obj) {
                    return obj.componentId == componentId;
                });
                if (currentComponentAxles && currentComponentAxles.length > 0) {
                    currentComponentAxles[0].axleItems.push(axleItemObj);
                } else {
                    AxleTableMethods.AxlesArr.push({ componentId: componentId, axleItems: [] });
                    currentComponentAxles = AxleTableMethods.AxlesArr.filter(function (obj) {
                        return obj.componentId == componentId;
                    });
                    currentComponentAxles[0].axleItems.push(axleItemObj);
                }
            } else {
                axlesObj.push(axleItemObj);
            }

            //if (noOfWheels == "" && axleWeight == "" && (distanceToNxtAxl == undefined || distanceToNxtAxl == ""))
            //    axlesList.pop();
            i++;
        }).promise().done(function () {
            if (!fromPopup) {
                AxleTableMethods.AxlesArr.push({ componentId: componentId, axleItems: axlesObj });
            }
            if (callbackfn != null && callbackfn != undefined) {
                callbackfn();
            }
        });
    },
    GetAxlesByComponentId: function (componentId, takeFullList = false) {
        if (typeof AxleTableMethods != 'undefined' && AxleTableMethods.AxlesArr && AxleTableMethods.AxlesArr.length > 0) {
            if (takeFullList) {
                var result = AxleTableMethods.AxlesArr.map(function (a) { return a.axleItems; });
                var allAxles = [];
                for (var i = 0; i < result.length; i++) {
                    var axleItem = result[i];
                    allAxles = allAxles.concat(axleItem);
                }
                return allAxles;
            } else {
                var currentComponentAxles = AxleTableMethods.AxlesArr.filter(function (obj) {
                    return obj.componentId == componentId;
                });
                if (currentComponentAxles && currentComponentAxles.length > 0)
                    return currentComponentAxles[0].axleItems;
                return null;
            }
        }
        return null;
    },
    OpenAxlePopupFromConfigSection: function (_this, loadFull = false) {
        var axleDropCount = $(_this).attr("axleDropCount");
        startAnimation();
        var compId = $(_this).attr('componentId');
        var componentTypeId = parseInt($('#' + compId).find('#vehicleTypeValue').val());
        var compSubId = parseInt($('#' + compId).find('#vehicleSubTypeValue').val());

        var compIds;
        if (loadFull) {
            compIds = $(_this).data('componentids');
            compId = 0;
            var compIdFirst = compIds.split(',')[0];
            componentTypeId = parseInt($('#' + compIdFirst).find('#vehicleTypeValue').val());
            compSubId = parseInt($('#' + compIdFirst).find('#vehicleSubTypeValue').val());
        }
        //var numberOfAxles = $(_this).closest("div").find("#Number_of_Axles").val();
        if ($(_this).closest('#' + compId).length > 0) {
            numberOfAxles = $(_this).closest('#' + compId).find("#Number_of_Axles").val();
        } else {
            numberOfAxles = $(_this).closest('#div_config_general').find("#Number_of_Axles").val();
        }
        if (numberOfAxles == "" || numberOfAxles == undefined) {
            numberOfAxles = 0;
        }
        var configurableAxles = $('#' + compId).find('#div_component_general_page .AxleConfig').val();
        var compWeight = $('#' + compId).find('#div_general #Weight').val();
        //if (configurableAxles == 'True') {

        
        var movementId = $('#movementTypeId').val();

        if (componentTypeId == null) { componentTypeId = 0; }
        if (compSubId == null) { compSubId = 0; }

        var isEdit = $('#IsEdit').val();
        if (isEdit == '') {
            isEdit = false;
        }
        var isCandidate = false;
        if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
            isCandidate = true;
        }
        var planMovement = false;
        if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true" || $('#isMovement').val() == "true") {
            planMovement = true;
        }
        var isEditMovement = false;
        if ($('#IsEditMovement').val() == "true" || $('#IsEditMovement').val() == "True") {
            isEditMovement = true;
        }
        var isLastComponent = false;
        if (loadFull || $('#' + compId + ' #IsLastComponent').val().toLowerCase() == "true") {
            isLastComponent = true;
        }
        //GetCompAxleDetails(compId);
        //var axleList = [];

        //for (var i = 0; i < axlesListFromPopUp.length; i++) {
        //    if (axlesListFromPopUp[i].componentId == compId)
        //        axleList = axlesListFromPopUp[i].axlesListPopUp;
        //}
        var axleListTemp;

        if (typeof AxleTableMethods != 'undefined') {
            if (loadFull) {
                var allCompIds = compIds.split(',');
                for (var i = 0; i < allCompIds.length; i++) {
                    AxleTableMethods.CreateAxlesObj(allCompIds[i], false);
                }
            }
            axleListTemp = AxleTableMethods.GetAxlesByComponentId(compId, loadFull);
        }

        $("#popupDialogue").load("../VehicleConfig/AxlePopUp", {
            axleCount: numberOfAxles, componentId: compId, compIds: compIds, vehicleSubTypeId: compSubId, vehicleTypeId: componentTypeId, movementId: movementId,
            weight: compWeight, IsEdit: isEdit, IsFromConfig: 1, movement: planMovement, isCandidate: isCandidate, isEditMovement: isEditMovement,
            axles: axleListTemp, isLastComponent: isLastComponent
        }, function () {
            stopAnimation();
            $('.modal-dialog .btnOpenAxlePopUp').remove();
            $('#axleDetails').modal({ keyboard: false, backdrop: 'static' });
            $("#popupDialogue").show();
            $("#overlay").show();
            $('#axleDetails').modal('show');
            Comp_AxleInit(1, compId);
            if (axleListTemp && (axleListTemp.length == 0 || axleListTemp[0].TyreCenters == null || axleListTemp[0].TyreCenters == ""))
                $('.popUpAxleDetails .axlewrapper input:text').blur();
            if (compId > 0) {
                ShowCurrentComponent(compId);
                //SetHeaderHeightForConfig(compId);
            } else {
                if (loadFull) {
                    // Temp hide
                    //$('#axleDetails .btn_CopyAll,#axleDetails .btn_AddAxleDetails,#axleDetails .btn_ClearAxleDetails').css('opacity','0');
                }
            }
            setTimeout(function () {
                HeaderHeight();
            }, 500);
            ShowCurrentHideHeaderTyreSpace('.popUpAxleDetails');
        });

        //}
        //else {
        //    stopAnimation();
        //}
    },
    SaveFromPopupAndAddAxleToComponent: function (_this) {
        var compId = $(_this).attr("componentId");

        var axlesList = [];
        var allCompIds = [];

        if (compId == "0" || compId == 0) {
            allCompIds = AxleTableMethods.AxlesArr.map(function (a) { return a.componentId; });
        } else {
            allCompIds.push(compId);
        }

        for (var i = 0; i < allCompIds.length; i++) {
            var componentId = allCompIds[i];
            var parentCompElem = $('#divComponentData-' + componentId);
            AxleTableMethods.CreateAxlesObj(componentId, true, function () {
                if (componentId == undefined)
                    return;
                if (typeof AxleTableMethods != 'undefined') {
                    axlesList = AxleTableMethods.GetAxlesByComponentId(componentId);
                }

                var numberOfAxles = axlesList.length;
                if (numberOfAxles == "" || numberOfAxles == undefined) {
                    numberOfAxles = 0;
                }

                var compWeight = parentCompElem.find('#Weight').val();

                var isFromConfig = $('#HiddenFromConfig').val();

                var componentTypeId = parseInt(parentCompElem.find('#vehicleTypeValue').val());
                var compSubId = parseInt(parentCompElem.find('#vehicleSubTypeValue').val());
                var movementId = $('#movementTypeId').val();

                if (componentId == "") { componentId = 0; }
                if (componentTypeId == null) { componentTypeId = 0; }
                if (compSubId == null) { compSubId = 0; }

                var isEdit = $('#IsEdit').val();
                if (isEdit == '') {
                    isEdit = false;
                }
                var isLastComponent = false;
                if (parentCompElem.find('#IsLastComponent').val().toLowerCase() == "true") {
                    isLastComponent = true;
                }
                var guid = $('#GUID').val();
                var data = {
                    axleCount: numberOfAxles, componentId: componentId,
                    vehicleSubTypeId: compSubId, vehicleTypeId: componentTypeId, movementId: movementId,
                    weight: compWeight, IsEdit: isEdit, isFromConfig: isFromConfig, axles: axlesList, isLastComponent: isLastComponent, GUID: guid
                };
                var url = '../Vehicle/AxleComponent';
                if ($('#hf_IsFromConfig').val() == 1) {
                    url = '../VehicleConfig/AxleComponent';
                }
                $.ajax({
                    url: url,
                    data: data,
                    type: 'POST',
                    async: false,
                    success: function (response) {
                        parentCompElem.find('#axlePage').html(response);
                        HeaderHeight(componentId);
                        AxleTableMethods.ShowHideViewAllAxlePopUpButtonInConfig();
                        Comp_AxleInit(0, componentId);
                        $('.dyntitle').text('Edit axle');
                        MaxAxleWeightUpdate();
                        //ConfigAutoFillOnEdit();//Update
                    },
                    error: function () {

                    },
                    complete: function () {

                    }
                });
                //AxleTableMethods.ShowHideViewAllAxlePopUpButtonInConfig();
                AxleTableMethods.AdjustSingleComponentAxleTableWidth();
            });


        }


    },
    OnAxleTextBoxChange: function (_this) {
        var compId = $(_this).parent().closest('.comp').attr('id');
        $("#btn_cancel").show();
        $('#componentBtn').show();
        var numberOfAxles = $(_this).parent().closest('#div_component_general_page').find('.axledrop').val();
        if (numberOfAxles == "" || numberOfAxles == undefined) {
            numberOfAxles = 0;
        }

        if (numberOfAxles == 0) {
            $('#' + compId + ' #axlePage').html('');
        }

        var configurableAxles = $(_this).parent().closest('#div_component_general_page').find('.AxleConfig').val();
        //if (configurableAxles == 'True') {
        var compWeight = $('#' + compId).find('#Weight').val();
        var isFromConfig = $('#HiddenFromConfig').val();

        var componentTypeId = parseInt($('#' + compId).find('#vehicleTypeValue').val());
        var compSubId = parseInt($('#' + compId).find('#vehicleSubTypeValue').val());
        var movementId = $('#movementTypeId').val();

        if (componentTypeId == null) { componentTypeId = 0; }
        if (compSubId == null) { compSubId = 0; }
        var isLastComponent = false;
        if ($('#' + compId + ' #IsLastComponent').val().toLowerCase() == "true") {
            isLastComponent = true;
        }

        var isEdit = $('#IsEdit').val();
        if (isEdit == '') {
            isEdit = false;
        }
        var guid = $('#GUID').val();

        var axleListTemp;
        if (typeof AxleTableMethods != 'undefined') {
            axleListTemp = AxleTableMethods.GetAxlesByComponentId(compId);
        }
        if (axleListTemp != undefined && axleListTemp != null && axleListTemp.length > 0 && numberOfAxles < axleListTemp.length) {
            //axles count is less than current list count
            var tobeRemoved = axleListTemp.length - numberOfAxles;
            AxleTableMethods.AxlesItemsArrUndo = axleListTemp.splice(axleListTemp.length - tobeRemoved);
            $('#esdalToast').off('hidden.bs.toast');
            $('#esdalToast').on('hidden.bs.toast', function () {
                AxleTableMethods.AxlesItemsArrUndo = [];
                $('#esdalToast').off('hidden.bs.toast');
            })
        }
        var data = {
            axleCount: numberOfAxles, componentId: compId,
            vehicleSubTypeId: compSubId, vehicleTypeId: componentTypeId, movementId: movementId,
            weight: compWeight, IsEdit: isEdit, isFromConfig: isFromConfig, isLastComponent: isLastComponent, GUID: guid,
            axles: axleListTemp
        };
        var url = '../Vehicle/AxleComponent';
        if ($('#hf_IsFromConfig').val() == 1) {
            url = '../VehicleConfig/AxleComponent';
        }
        $.ajax({
            url: url,
            data: data,
            type: 'POST',
            async: false,
            success: function (response) {
                $('#' + compId + ' #axlePage').html(response);
                AxleTableMethods.ShowHideViewAllAxlePopUpButtonInConfig();
                Comp_AxleInit(0, compId);
                HeaderHeight(compId);
                $('.dyntitle').text('Edit axle');

                AxleTableMethods.CreateAxlesObj(compId, false);
                AxleTableMethods.AdjustSingleComponentAxleTableWidth();
            },
            error: function () {

            },
            complete: function () {

            }
        });
        //}
        //else {
        //    $('.axlePopUp_' + compId).hide();
        //}
    },
    AddUpdateColumnsOnNoOfWheelsChange: function (_this) {
        var _thisvalue = $(_this).val();
        if ($("#divAllComponent").find('.comp').html() != undefined) {
            if ($("#divAllComponent").find('.comp').html().trim().length > 0) {
                var div = $(_this).parent().closest('.comp').attr('id');
                if (div == undefined) {
                    div = "popupDialogue";
                }
                var _headerlen = $('#' + div).find('.headgrad1').length;
                var numOfWheels = parseInt(_thisvalue);
                var headLength = parseInt(_headerlen);

                var tyreSpaceCnt = $('#' + div).find('.tyreSpaceCnt').length;
                if (tyreSpaceCnt <= 0) {
                    return false;
                }

                var isvr1 = $('#isvr1').val();
                if ($('#' + div + ' #ConfigTyreSpace').length > 0) {

                    var isTyreSizeRequired = $('#' + div + ' #IsTyreSizeRequired').val();
                    var isTyreCentreSpacingRequired = $('#' + div + ' #IsTyreCentreSpacingRequired').val();
                    if (isTyreSizeRequired != "True" && isTyreSizeRequired != "true"
                        && isTyreCentreSpacingRequired != "True" && isTyreCentreSpacingRequired != "true") {
                        $('#' + div + ' .headgrad2').html('Tyre Centre Spacing');
                    }
                    else {
                        $('#' + div + ' .headgrad2').html('<b>Tyre Centre Spacing *</b>');
                    }
                        var height = $('#' + div + ' #tbl_Axle').find('thead th:eq(0)').height();
                        var paddingtop = $('#' + div + ' #tbl_Axle').find('thead th:eq(0)').css('padding-top');
                        var paddingbottom = $('#' + div + ' #tbl_Axle').find('thead th:eq(0)').css('padding-bottom');
                        var paddingtopvalue = paddingtop.replace('px', '');
                        var paddingbottomvalue = paddingbottom.replace('px', '');
                        var totalheight = parseFloat(height) + parseFloat(paddingtopvalue) + parseFloat(paddingbottomvalue);
                        $('#' + div + ' .headgradBtn').attr('style', 'height:' + totalheight + 'px;');
                    
                }
                
                var greatestVal = 0;
                $('#' + div + ' .nowheels').each(function () {
                    var _thisWheelVal = $(this).val();
                    if (greatestVal < parseInt(_thisWheelVal)) {
                        greatestVal = parseInt(_thisWheelVal);
                    }
                });

                if (greatestVal <= 0) {
                    $('#' + div + ' #tbl_Axle .cstable').remove();
                    $('#' + div + ' #tbl_Axle .tyreSpaceCnt').hide();
                    $('#' + div + ' #tbl_Axle .sub1').hide();
                    return;
                }

                if ((headLength + 1) < greatestVal) {
                    $('#' + div + ' .headgrad2').attr('colspan', numOfWheels - 1);
                    $('#' + div + ' .centerspace1').attr('colspan', numOfWheels - 1);
                }
                else {
                    $('#' + div + ' .headgrad2').attr('colspan', greatestVal - 1);
                }
                $('#' + div + ' .headgrad1').remove();
                for (var j = 1; j < greatestVal; j++) {
                    $('#' + div + ' .sub').append('<th class="headgrad1">' + j + '</th>');
                    if (j == greatestVal - 1) {
                        $('#' + div + ' .sub').append('<th style="background-color: rgb(213 223 234) !important;" class="headgrad1 1"></th>');
                    }
                    $('#' + div + ' .headgrad2').show();
                }
                var tyrecenterspaceval = $(_this).closest('tr').find('.cstable .txt-wheel-count').val();
                if (tyrecenterspaceval == undefined)
                    tyrecenterspaceval = "";
                $(_this).closest('tr').find('.cstable').remove();
                for (var i = 1; i < numOfWheels; i++) {
                    if ($(_this).closest('tr').find('.cstyreSize').length>0)
                        $(_this).closest('tr').find('.cstyreSize').after('<td class="cstable"><input type="text" name="q" class="axletext frmbdr tyrec3 txt-wheel-count" Maxlength="5" value="' + tyrecenterspaceval+'"/></td>');
                    else
                        $(_this).closest('tr').find('.headcol').before('<td class="cstable"><input type="text" value="" name="q" class="axletext frmbdr tyrec3 txt-wheel-count" Maxlength="5"/></td>');
                }

                $('#' + div + ' #tbl_Axle .tyreSpaceCnt').show();
                $('#' + div + ' #tbl_Axle .sub1').show();
                var height = $('#' + div + ' #tbl_Axle').find('thead th:eq(0)').height();
                var paddingtop = $('#' + div + ' #tbl_Axle').find('thead th:eq(0)').css('padding-top');
                var paddingbottom = $('#' + div + ' #tbl_Axle').find('thead th:eq(0)').css('padding-bottom');
                var paddingtopvalue = paddingtop.replace('px', '');
                var paddingbottomvalue = paddingbottom.replace('px', '');
                var totalheight = parseFloat(height) + parseFloat(paddingtopvalue) + parseFloat(paddingbottomvalue);
                $('#' + div + ' .headgradBtn').attr('style', 'height:' + totalheight + 'px;');

                AxleTableMethods.UpdateOtherTrsBasedOnWheelCountChange(_this, greatestVal);//HE-7237
            }
        }
        else {
            var _headerlen = $('.headgrad1').length;
            var numOfWheels = parseInt(_thisvalue);
            var headLength = parseInt(_headerlen);

            if ($('#ConfigTyreSpace').length > 0) {

                $('.headgrad2').text('Tyre Centre Spacing');
                var height = $('#tbl_Axle').find('thead th:eq(0)').height();
                var paddingtop = $('#tbl_Axle').find('thead th:eq(0)').css('padding-top');
                var paddingbottom = $('#tbl_Axle').find('thead th:eq(0)').css('padding-bottom');
                var paddingtopvalue = paddingtop.replace('px', '');
                var paddingbottomvalue = paddingbottom.replace('px', '');
                var totalheight = parseFloat(height) + parseFloat(paddingtopvalue) + parseFloat(paddingbottomvalue);
                $('.headgradBtn').attr('style', 'height:' + totalheight + 'px;');
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
                if (j == greatestVal - 1) {
                    $('.sub').append('<th style="background-color: rgb(213 223 234) !important;" class="headgrad1 2"></th>');
                }
            }
            

            $(_this).closest('tr').find('.cstable').remove();
            //$('#tbl_Axle').find('tr').find('.cstable').not($('#divaxlerow')).remove();
            for (var i = 1; i < numOfWheels; i++) {
                $(_this).closest('tr').find('.cstyreSize').after('<td class="cstable" id="divaxlerow"><input type="text" value="" name="q" class="axletext frmbdr tyrec3 txt-wheel-count" Maxlength="5"/></td>');
                //$('#tbl_Axle').find('tr').find('.cstable').not($('#divaxlerow')).find('.cstyreSize').after('<td class="cstable"></td>');
            }
            $('.tyreSpaceCnt').show();
            //$(_this).closest('tr').next('tr').find('.cstable').remove();
            //$(_this).closest('tr').next('tr').find('.cstyreSize').after('<td class="cstable" id="divaxlerow"></td>');


            var height = $('#tbl_Axle').find('thead th:eq(0)').height();
            var paddingtop = $('#tbl_Axle').find('thead th:eq(0)').css('padding-top');
            var paddingbottom = $('#tbl_Axle').find('thead th:eq(0)').css('padding-bottom');
            var paddingtopvalue = paddingtop.replace('px', '');
            var paddingbottomvalue = paddingbottom.replace('px', '');
            var totalheight = parseFloat(height) + parseFloat(paddingtopvalue) + parseFloat(paddingbottomvalue);
            $('.headgradBtn').attr('style', 'height:' + totalheight + 'px;');
            AxleTableMethods.UpdateOtherTrsBasedOnWheelCountChange(_this, greatestVal);
        }
    },//function for adding tyre spacing text
    UpdateOtherTrsBasedOnWheelCountChange: function (_this, greatestVal) {
        if ($('.tyreSpaceCnt .headgrad1').length <= 0) {
            return;
        }
        if (greatestVal - 1 <= 0) {
            $('.headgrad2:visible').hide();
            $('td.cstable:visible').remove();
            $('.sub1:visible').hide();
            var componentId = $(_this).closest('.component-item-axle-section').find('#Component_Id').val();
            HeaderHeight(componentId);
        }
        var otherTrs = $(_this).closest('tr').siblings('tr');
        if (otherTrs.length > 0) {
            otherTrs.push($(_this).closest('tr'));
        }
        for (var i = 0; i < otherTrs.length; i++) {
            var currentTr = otherTrs[i];
            var cstableCount = $(currentTr).find('.cstable').length;
            var numOfWheelsTemp = greatestVal - 1;

            //3 -> 2
            //if (cstableCount == greatestVal) {
            //    $(currentTr).find('td.cstable:last').remove();
            //}

            if (numOfWheelsTemp > 0 && cstableCount < numOfWheelsTemp) {
                while (numOfWheelsTemp > cstableCount) {
                    if (cstableCount > 0) {
                        $(currentTr).find('td.cstable:last').after('<td class="cstable"></td>');
                    } else {
                        if ($(currentTr).find('.cstyreSize').length > 0)
                            $(currentTr).find('.cstyreSize').after('<td class="cstable"></td>');
                        else
                            $(currentTr).find('.headcol').before('<td class="cstable"></td>');

                    }
                    cstableCount++;
                }
            } else if (cstableCount > numOfWheelsTemp) {
                while (cstableCount > numOfWheelsTemp) {
                    $(currentTr).find('td.cstable:last').remove();
                    cstableCount--;
                }
            }
        }
    },

    ShowHideViewAllAxlePopUpButtonInConfig: function () {
        var totalAxle = 0;
        $('#btnOpenAllAxlePopup').hide();
        $('.comp').each(function () {
            var componentId = $(this).attr("id");
            var numberOfAxles = $('#' + componentId).find('#Number_of_Axles').val();
            var numberOfAxles = numberOfAxles == "" ? 0 : numberOfAxles;
            totalAxle += numberOfAxles;
        }).promise().done(function () {
            if (totalAxle > 0)
                $('#btnOpenAllAxlePopup').show();
            else
                $('#btnOpenAllAxlePopup').hide();
        });
    },
    AxleCopyToAll: function (_this) {
        var i = 1;
        var isValidAxle = ValidateAxlesRow(0, _this);
        var axleComp = $(_this).closest('.AxleComponent');
        if (isValidAxle) {
            if (axleComp.length > 0) {
                var _thisWheelVal = axleComp.find('#tbl_Axle tbody tr').eq(0).find('.nowheels').val();
                axleComp.find('#tbl_Axle tbody tr:not(:eq(0))').each(function () {
                    var _thisWheel = $(this).find('.nowheels');
                    copyTyreSpace(_thisWheel, _thisWheelVal);
                });


                axleComp.find('#tbl_Axle tbody tr').eq(0).find('input:text').each(function () {
                    var _thisValue = $(this).val();
                    axleComp.find('#tbl_Axle tbody tr:not(:eq(0))').each(function () {
                        $(this).find('td:eq(' + i + ')').find('input:text').val(_thisValue);
                    });
                    i++;
                });
            }
            else {
                var tblAxle = $(_this).closest('#tbl_Axle');
                var _thisWheelVal = tblAxle.find('tbody tr').eq(0).find('.nowheels').val();
                tblAxle.find('tbody tr:not(:eq(0))').each(function () {
                    var _thisWheel = $(this).find('.nowheels');
                    copyTyreSpace(_thisWheel, _thisWheelVal);
                });


                tblAxle.find('tbody tr').eq(0).find('input:text').each(function () {
                    var _thisValue = $(this).val();
                    $('#tbl_Axle tbody tr:not(:eq(0))').each(function () {
                        $(this).find('td:eq(' + i + ')').find('input:text').val(_thisValue);
                    });
                    i++;
                });
            }
            if ($('#ConfigTypeId').val() == "244012") {
                var disttonextsum = 0;
                if (disttonextsum == 0) {
                    $('.component_axle #tbl_Axle').find('.disttonext').each(function () {
                        var _thisVal = $(this).val() == "" ? 0 : $(this).val();
                        disttonextsum = disttonextsum + parseFloat(_thisVal);
                    });
                }
                if (disttonextsum != 0) {
                    disttonextsum = disttonextsum.toFixed(2);
                    $('#VehicleConfigInfo').find('#Wheelbase').val(disttonextsum);
                    //$('#VehicleConfigInfo').find('#Wheelbase').trigger('blur');
                }
            }
            MaxAxleWeightUpdate(_this);
            //var axleWeightArray = [];
            //var axleweightMaxval = 0;
            //axleweightsum = 0;
            //$('.component_axle #tbl_Axle').find('.axleweight').each(function () {
            //    var _thisVal = $(this).val() == "" ? 0 : $(this).val();
            //    axleweightsum = axleweightsum + parseFloat(_thisVal);
            //    axleWeightArray.push(parseFloat(_thisVal));
            //});

            //axleweightMaxval = Math.max.apply(Math, axleWeightArray);
            //if (parseFloat(axleweightMax) < parseFloat(axleweightMaxval)) {
            //    axleweightMax = axleweightMaxval;
            //}
            //if (axleweightMaxval != 0) {
            //    $('#VehicleConfigInfo').find('#AxleWeight').val(axleweightMaxval);
            //    //$('#VehicleConfigInfo').find('#AxleWeight').trigger('blur');
            //}
            //if ($('#MovementTypeId').val() == "270006" && ($('#ConfigTypeId').val() == "244001" || $('#ConfigTypeId').val() == "244002")) {
            //    var compId=$(_this).closest('.AxleComponent').closest('.comp').attr('id')
            //    var groundClr = $('#' + compId).find("#Ground_Clearance");
            //    var labelcmpdiv = $(groundClr).parent(".input-field").parent(".dynamic").find(".labelCompDiv");
            //    if (axleweightsum > 150000 && ($('#' + compId).find('#ComponentType_Id').val() == "234002"
            //        || $('#' + compId).find('#ComponentType_Id').val() == "234005")) {
            //        $(groundClr).attr("isrequired", 1);
            //        $(labelcmpdiv).html('');
            //        $(labelcmpdiv).append('<label class="text-normal"><b>Ground Clearance <span> *</span></b></label>');
            //    }
            //    else if (axleweightsum <= 150000 && ($('#' + compId).find('#ComponentType_Id').val() == "234002"
            //        || $('#' + compId).find('#ComponentType_Id').val() == "234005")) {
            //        $(groundClr).attr("isrequired", 0);
            //        $(labelcmpdiv).html('');
            //        $(labelcmpdiv).append('<label class="text-normal">Ground Clearance</label>');
            //    }
                
            //}
        }
        FillHeaderOnLoad();
        var isFromVehicle = $('#IsFromConfig').val();
        if (isFromVehicle != undefined && isFromVehicle != '') {
            ConfigAutoFill($('#axleweight'));
        }

        if (isValidAxle) {
            var greatestVal = 0;
            axleComp.find('#tbl_Axle').find('.nowheels').each(function () {
                var _thisWheelVal = $(this).val();
                if (greatestVal < parseInt(_thisWheelVal)) {
                    greatestVal = parseInt(_thisWheelVal);
                }
            });
            AxleTableMethods.UpdateOtherTrsBasedOnWheelCountChange(_this, greatestVal);

            var componentId = $(_this).closest('.axle').data('componentid');
            AxleTableMethods.CreateAxlesObj(componentId);
        }
        MovementAssessment(false, false);
    },

    AdjustSingleComponentAxleTableWidth: function () {
        var componentCount = $('.comp .divComponentData').length;
        //var windowWidth = $(window).width();
        //if (componentCount == 1 || windowWidth>=1880) {
        //    $('.axle-table-contents-container').css('width', '');
        //    $('.tblaxlebtn').css('width', '95%');
        //    $('.tblaxlebtn').css('margin-right', '0');
        //} else {
        //    $('.tblaxlebtn').css('margin-right', '11px');
        //}
    },
};