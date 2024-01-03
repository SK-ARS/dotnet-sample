var spanNumberValidPattern = new RegExp(/^(\+)?[0-9\ -]+$/);
var lengthValidPattern = new RegExp(/^\d{1,4}(\.\d{1,4})?$/);


var sectionIdVal = $('#hf_sectionId').val();
var structureNameVal = $('#hf_structureName').val();
var ESRNVal = $('#hf_ESRN').val();
var structureTypeVal = $('#hf_structureType').val();
var structureIdVal = $('#hf_structureId').val();
var EditSaveFlagVal = $('#hf_EditSaveFlag').val();

function StructureSectionSpanInit() {
    $("#hiddenPosition").attr("disabled", "disabled");
    $(".span-type-text").attr("disabled", "disabled");
    if ($('#hf_EditSaveFlag').val() == 1) {
        $("#hiddenPosition").removeAttr("disabled");
        $("#Position").attr("readonly", "readonly");
        $(".valueEditable").attr("disabled", "disabled");
        AddSpanFormLoad();
    }
    $("#SpanNo").attr("disabled", "disabled");

    $(".spanTypeDrpDwn").find("option").each(function () {
        ChangeCase(this);
    });
}

function AddSpanFormLoad() {
    $(".structure-section-span select").each(function () {
        var selectedElementId = this.id;
        SpanTypeDropDownchange(selectedElementId);
    });
}

function SpanTypeDropDownchange(idVal) {
    var selectedElementId = idVal;
        var selectedVal = $('#' + selectedElementId).val();
        var spanTxtElementId = $("[data-custom-attribute='" + selectedElementId + "']").attr('id');
    var txtValue = $("#" + spanTxtElementId).val();
    if (txtValue != undefined) {
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            $('#' + selectedElementId).val("Other");
            $('#' + spanTxtElementId).prop('disabled', false);
        }
        else if (selectedVal == "" && (txtValue.length <= 0)) {
            $('#' + spanTxtElementId).prop('disabled', true);
        }
        else {
            $('#' + spanTxtElementId).val("");
            $('#' + spanTxtElementId).prop('disabled', true);
        }
    }
}

$(document).ready(function () {
   
    $('body').on('click', '#btnSave', function (e) {
        e.preventDefault();
        storeSpanData(this);
    });
    
    $('body').on('click', '#btnSpanCancel', function (e) {
        e.preventDefault();
        cancel(this);
    });
    $('body').on('change', '.spanTypeDrpDwn', function (e) {
        var selectedElementId = this.id;
        SpanTypeDropDownchange(selectedElementId);
    });
});

function cancel() {
    CloseSuccessModalPopup();
    startAnimation();
    var strucType;
    var structureIdVal = $('#hf_structureId').val();
    var sectionIdVal = $('#hf_sectionId').val();
    if ($('#hf_sectionType').val() == "underbridge") {
        strucType = 1;
    }
    else if ($('#hf_sectionType').val() == "overbridge") {
        strucType = 2;
    }
    else {
        strucType = 3;
    }
    $("#generalSettingsId").load('../Structures/StructureDimensions?structureId=' + structureIdVal + "&sectionId=" + sectionIdVal + "&structureNm=" + structureNameVal + "&ESRN=" + ESRNVal + "&strucType=" + strucType,
        function () {
            $('#spanSection').empty();
            $('#managedimensiosId').show();
            StructureDimensionsInit();
            stopAnimation();
        }
    );
}
function storeSpanData() {
    startAnimation();
    var edit = $('#hf_EditSaveFlag').val();
    var spanSequenceValid = validateSpanSequence();
    var positionValid = validatePosition();
    var spLengthValid = validateSpLength();
    if (edit == "0") {
        var duplicateCheck = DuplicateCheck();
    }
    else {
        duplicateCheck = true;
    }
    if (spanSequenceValid && positionValid && spLengthValid && duplicateCheck) {
        CheckOtherValue();
        $.ajax({
            url: '../Structures/StoreSpanData',
            dataType: 'json',
            type: 'POST',
            data: $("#DimensionInfo").serialize(),
            success: function (result) {
                if (result == true) {
                    saveSpanData();
                }
            },
            error: function (xhr, status) {
            }
        });
    }
    else {
        stopAnimation();
    }

}
function CheckOtherValue() {
    $(".structure-section-span select").each(function () {
        var selectedElementId = this.id;
        var selectedVal = $('#' + selectedElementId).val();
        if (selectedVal == "Other") {
            changedVal = "";
            var spanTxtElementId = $("[data-custom-attribute='" + selectedElementId + "']").attr('id');
            var txtValue = $("#" + spanTxtElementId).val();
            if ($('#' + selectedElementId + ' option:contains(' + txtValue + ')').length) {
                $("#" + selectedElementId + ' option:selected').text(txtValue);
            }
        }
    });
}

function DuplicateCheck() {
    var structureIdVals = $('#hf_structureId').val();
    var spanPos = $("#Position").val();
    var saveFlag = 0;
    $("#duplicateCheck").hide();
    if ($('#hf_EditSaveFlag').val() && hf_EditSaveFlag != 1 && spanPos.length > 0) {

        $.ajax({
            url: '/Structures/ValidateSpanPosition',
            dataType: 'json',
            async: false,
            type: 'POST',
            data: { structureId: structureIdVals, sectionId: sectionIdVal, spanPosition: spanPos },
            beforeSend: function () {

            },
            success: function (result) {
                if (result != null) {
                    if (result.result == "1") {
                        saveFlag = 1;
                        $("#duplicateCheck").show();
                    }
                }
            },
            error: function (xhr, status) {
            },
            complete: function () {

            }
        });
    }

    if (saveFlag == 1) {
        return false;
    }
    else {
        $("#duplicateCheck").hide();
        return true;
    }
}
function saveSpanData() {
    var structureIdVals = $('#hf_structureId').val();
    var EditSaveFlagsVal = $('#hf_EditSaveFlag').val();
    startAnimation();
    $.ajax({
        url: '../Structures/SaveStructSectionSpan',
        dataType: 'json',
        type: 'POST',
        data: { structureId: structureIdVals, sectionId: sectionIdVal, editSaveFlag: EditSaveFlagsVal },
        success: function (result) {
            if (result == 1) {
                if ($('#hf_EditSaveFlag').val() == "0") {
                    stopAnimation();
                    ShowSuccessModalPopup("Structure span saved successfully", "cancel")
                }
                else {
                    stopAnimation();
                    ShowSuccessModalPopup("Structure span updated successfully", "cancel")
                }
            }
            else {
                ShowErrorPopup("Structure span saving failed");
                stopAnimation();
            }
        },
        error: function (xhr, status) {
            stopAnimation();
        }
    });
}
function validateSpanSequence() {
    var spanSequenceValid = true;
    if ($("#Sequence").val() != '') {
        if (!spanNumberValidPattern.test($("#Sequence").val())) {

            $("#spanSequenceValidate").show();
            spanSequenceValid = false;
        }
        else {
            $("#spanSequenceValidate").hide();
            spanSequenceValid = true;
        }
    }
    return spanSequenceValid;
}
function validatePosition() {
    var positionValid = true;
    if ($("#Position").val() != '') {
        if (!spanNumberValidPattern.test($("#Position").val())) {

            $("#positionValidate").show();
            $("#positionReqValidate").hide();
            $("#duplicateCheck").hide();
            positionValid = false;
        }
        else {
            $("#positionValidate").hide();
            $("#positionReqValidate").hide();
            positionValid = true;
        }
    }
    else {
        $("#positionReqValidate").show();
        $("#positionValidate").hide();
        $("#duplicateCheck").hide();
        positionValid = false;
    }
    return positionValid;
}
function validateSpLength() {
    var spLengthValid = true;
    if ($(".spLength").val() != '') {
        if (!lengthValidPattern.test($(".spLength").val())) {
            $("#spLengthValidate").show();
            $("#spLengthReqValidate").hide();
            spLengthValid = false;
        }
        else {
            $("#spLengthValidate").hide();
            $("#spLengthReqValidate").hide();
            spLengthValid = true;
        }
    }
    else {
        $("#spLengthReqValidate").show();
        $("#spLengthValidate").hide();
        spLengthValid = false;
    }
    return spLengthValid;
}

