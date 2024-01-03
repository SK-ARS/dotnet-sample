var onchangeflag;
var ShipmentVal;
var LoadDivisionVal;
var TotalDistanceOfRoadVal;
var AddressVal;

function ApplicationSupplimentaryInfoInit() {
    onchangeflag = false;
    isVehicleHasChanged = false;
    ShipmentVal = $('#hf_Shipment').val();
    LoadDivisionVal = $('#hf_LoadDivision').val();
    TotalDistanceOfRoadVal = $('#hf_TotalDistanceOfRoad').val();
    AddressVal = $('#hf_Address').val();
    if (($('#IsClone').val() != 1) && ($('#IsRevise').val() != 1)) {
        StepFlag = 5;
        SubStepFlag = 0;
        CurrentStep = "Supplementary Information";
        //$('#plan_movement_hdng').text("PLAN MOVEMENT");
        $('#current_step').text(CurrentStep);
        SetWorkflowProgress(5);
        $('#back_btn').show();
        $('#save_btn').show();
        $('#backbutton').show();
        $('#confirm_btn').hide();
        $('#apply_btn').hide();
    }

    if ($('#SupplInfoSaveFlag').val() == 'true') {
        $('#cancel_suppli_info_btn_cntr').show();
    }

    if (ShipmentVal == 1) {
        btn = document.getElementById("partseatrue");
        btn.checked = true;
        $('#PortNames').attr('disabled', false);
        $('#SeaQuotation').attr('disabled', false);
    }
    else {
        btn = document.getElementById("partseafalse");
        btn.checked = true;
        $('#PortNames').attr('disabled', true);
        $('#SeaQuotation').attr('disabled', true);
    }

    if (LoadDivisionVal == 1) {
        btn = document.getElementById("YesLoadDiv");
        btn.checked = true;

        $('#AdditionalCost').attr('disabled', false);
        $('#RiskNature').attr('disabled', false);
    }
    else {

        btn = document.getElementById("NoLoadDiv");
        btn.checked = true;
        $('#AdditionalCost').attr('disabled', true);
        $('#RiskNature').attr('disabled', true);
    }

    if (TotalDistanceOfRoadVal != "") {
        if (AddressVal == 1) {
            btn = document.getElementById("YesAddress");
            btn.checked = true;
            $('#ProposedMoveDetails').attr('disabled', true);
        }
        else {
            btn = document.getElementById("NoAddress");
            btn.checked = true;

            $('#ProposedMoveDetails').attr('disabled', false);

        }
    }
    else {
        btn = document.getElementById("YesAddress");
        btn.checked = true;
        btn.value = 1;
        $('#ProposedMoveDetails').attr('disabled', true);
    }
    var IsSupplimentaryAlreadySaved = $('#hf_IsSupplimentarySaved').val();
    if (IsSupplimentaryAlreadySaved == "true" || IsSupplimentaryAlreadySaved == "True") {
        //GetLocalObject(StepFlag, updateControls = false);
    } else {
        //GetLocalObject(StepFlag, updateControls = true);
        
    }
    if ($('#supplimentaryinfo').length > 0) {
        CreateLocalObject(StepFlag, true);
    }
    stopAnimation();
}
$(document).ready(function () {
    $('body').on('keyup', '.onchangefunctionVR1Suppl', function (e) {
        e.preventDefault();
        onchangefunctionVR1Suppl(this);
    });
    $('body').on('click', '#CancelEditingSupplInfo', function (e) {
        e.preventDefault();
        CancelEditingSupplInfo(this);
    });
    $('body').on('change', 'input:radio[name="partsea"]', function (e) {
        var partsea = $(this).val();
        if (partsea == 0) {
            $('#PortNames').attr('disabled', true);
            $('#SeaQuotation').attr('disabled', true);
            $("#Shipment").val(0);
        }
        else {
            $('#PortNames').attr('disabled', false);
            $('#SeaQuotation').attr('disabled', false);
            $("#Shipment").val(1);
        }
    });
    $('body').on('change', 'input:radio[name="Address"]', function (e) {
        var partsea = $(this).val();
        if (partsea == 0) {
            $('#ProposedMoveDetails').attr('disabled', false);
            $("#Address").val(0);
        }
        else {
            $('#ProposedMoveDetails').attr('disabled', true);
            $("#Address").val(1);
        }
    });
    $('body').on('change', 'input:radio[name="LoadDiv"]', function (e) {
        var partsea = $(this).val();
        if (partsea == 0) {
            $('#AdditionalCost').attr('disabled', true);
            $('#RiskNature').attr('disabled', true);
            $("#LoadDivision").val(0);
        }
        else {
            $('#AdditionalCost').attr('disabled', false);
            $('#RiskNature').attr('disabled', false);
            $("#LoadDivision").val(1);
        }
    });
    $('body').off('focus', '#supplimentaryinfo .edit-normal,#supplimentaryinfo .form-control');
    $('body').on('focus', '#supplimentaryinfo .edit-normal,#supplimentaryinfo .form-control', function (e) {
        $(this).closest('.input-field').find('.error').html('');
    });
});
var invalidElem;
function checknoncomplusaryfield() {
    var isvalid = true;
    $('.error').html('');
    invalidElem = undefined;
    if ($('#TotalDistanceOfRoad').length < 0) {
        invalidElem = invalidElem == undefined ? $('#TotalDistanceOfRoad') : invalidElem;
        return isValid;
    }

    var decimalRegex = /^[1-9]\d*(\.\d+)?$/;

    if ($('#TotalDistanceOfRoad').length>0 && $('#TotalDistanceOfRoad').val().trim().length == 0) {
        $('#spTotalDistOfRoad').html('Please enter Total distance of the road');
        invalidElem = invalidElem == undefined ? $('#TotalDistanceOfRoad') : invalidElem;
        isvalid = false;
    }
    else if (!decimalRegex.test($('#TotalDistanceOfRoad').val())) {
        $('#spTotalDistOfRoad').html('Please enter valid number');
        invalidElem = invalidElem == undefined ? $('#TotalDistanceOfRoad') : invalidElem;
        isvalid = false;
    }


    if ($('#ApprValueOfLoad').length > 0 && (!decimalRegex.test($('#ApprValueOfLoad').val())) && ($('#ApprValueOfLoad').val().length > 0)) {
        $('#spApprValueOfLoad').html('Please enter valid number');
        invalidElem = invalidElem == undefined ? $('#ApprValueOfLoad') : invalidElem;
        isvalid = false;
    }
    else {
        $('#spApprValueOfLoad').html('');
    }
    if ($('#AdditionalCost').length>0 && (!decimalRegex.test($('#AdditionalCost').val())) && ($('#AdditionalCost').val().length > 0)) {
        if ($('#AdditionalCost').val() != 0) {
            $('#spAdditionalCost').html('Please enter valid number');
            invalidElem = invalidElem == undefined ? $('#AdditionalCost') : invalidElem;
            isvalid = false;
        }

    }
    else {
        $('#spAdditionalCost').html('');
    }
    if ($('#ApprCostOfMovement').length > 0 && (!decimalRegex.test($('#ApprCostOfMovement').val())) && ($('#ApprCostOfMovement').val().length > 0)) {
        $('#spApprCostOfMovement').html('Please enter valid number');
        invalidElem = invalidElem == undefined ? $('#ApprCostOfMovement') : invalidElem;
        isvalid = false;
    }
    else {
        $('#spApprCostOfMovement').html('');
    }

    isvalid = validateLength('#TotalDistanceOfRoad', isvalid, '#spTotalDistOfRoad');
    isvalid = validateLength('#ApprValueOfLoad', isvalid, '#spApprValueOfLoad');
    isvalid = validateLength('#DateOfAuthority', isvalid, '#spDateOfAuthority');
    isvalid = validateLength('#ApprCostOfMovement', isvalid, '#spApprCostOfMovement');
    isvalid = validateLength('#AdditionalCost', isvalid, '#spAdditionalCost');
    isvalid = validateLength('#RiskNature', isvalid, '#spRiskNature');
    isvalid = validateLength('#AdditionalConsideration', isvalid, '#spAdditionalConsideration');
    isvalid = validateLength('#ProposedMoveDetails', isvalid, '#spProposedMoveDetails');
    isvalid = validateLength('#SeaQuotation', isvalid, '#spSeaQuotation');
    isvalid = validateLength('#PortNames', isvalid, '#spPortNames');

    if (!isvalid && invalidElem != undefined) {
        $('body,html').animate({ scrollTop: invalidElem.position().top }, 800);
    }
    return isvalid;
}
function validateLength(elem, currentStatus, errorSpanElem) {
    if ($(elem).length <= 0)
        return currentStatus;
    var elemVal = $(elem).val();
    if (elemVal == '')
        return currentStatus;
    var elemLength = elemVal.length;
    var elemMaxLength = $(elem).data('maxlength')||500;
    if (elemLength > 0 && elemLength > elemMaxLength) {
        $(errorSpanElem).html('Maximum ' + elemMaxLength + ' characters allowed.');
        invalidElem = invalidElem == undefined ? $(elem) : invalidElem;
        currentStatus = false;
    }
    return currentStatus;
}
function SaveSupplimentaryInfo(showPopUp = true, NavigateToLink = '') {
    var chk = checknoncomplusaryfield();
    if (chk == true) {
        var ApplicationRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;

        $.ajax({
            url: '/Application/SaveSupplimentaryInfo?appRevisionId=' + ApplicationRevId,
            dataType: 'json',
            type: 'POST',
            data: $("#supplimentaryinfo").serialize(),
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                $('#hf_IsSupplimentarySaved').val('True')
                RemoveLocalObject(NavigationEnum.ROUTEASSESSMNT_SUPPLY);
                $('#TextChangeFlagVR1').val(false);
                $('#SupplInfoSaveFlag').val(true);
                if (NavigateToLink != '' && NavigateToLink != null) {
                    location.href = NavigateToLink;
                }
                if (showPopUp) {
                    if (result == true) {
                        //showWarningPopDialog('', 'Ok', '', 'CloseSaveForSuppl', '', 1, 'info');
                        var Msg = 'Supplementary Information saved successfully'
                        showToastMessage({
                            message: Msg,
                            type: "success"
                        });
                        NavigateToSupplConfirm();
                    }
                    else {
                        //showWarningPopDialog('', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                        var msg1 = 'Supplementary Information not saved';
                        ShowErrorPopup(msg1);
                    }
                    scrolltotop();
                }
            },
            error: function (xhr, status) {
                //location.reload();
                console.error('error', xhr);
            },
            complete: function () {
                stopAnimation();
            }

        });
    }
}
function CloseSaveForSuppl() {
    $('.pop-message').html('');
    $('.box_warningBtn1').html('');
    $('.box_warningBtn2').html('');
    $('#pop-warning').hide();

    $('.box_warningBtn1').unbind();
    $('.box_warningBtn2').unbind();
    $('#tab_4').hide();

}
function onchangefunctionVR1Suppl() {
    onchangeflag = true;
    $('#TextChangeFlagVR1').val(onchangeflag);
}
