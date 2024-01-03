var hf_StructureId = $('#hf_StructureId').val()
var hf_SectionId = $('#hdnSectionID').val()
var hf_structName = $('#hf_structName').val()
var hf_ESRN = $('#hf_ESRN').val()
var performBtnFlag = false;

function SVDataInit() {
   
    hf_StructureId = $('#hf_StructureId').val()
    hf_SectionId = $('#hdnSectionID').val() || 0;
    hf_structName = $('#hf_structName').val()
    hf_ESRN = $('#hf_ESRN').val();
    $("#manageSVId").css("display", "block");
    $("#DimensionInfo").validate();

    if ($('#hf_Helpdest_redirect').val() == "true") {
        $("#WithSV80").attr("readonly", "readonly");
        $("#WithoutSV80").attr("readonly", "readonly");
        $("#WithSV100").attr("readonly", "readonly");
        $("#WithoutSV100").attr("readonly", "readonly");
        $("#WithSV150").attr("readonly", "readonly");
        $("#WithoutSV150").attr("readonly", "readonly");
        $("#WithSVTrain").attr("readonly", "readonly");
        $("#WithoutSVTrain").attr("readonly", "readonly");
        $("#WithSVTT").attr("readonly", "readonly");
        $("#WithoutSVTT").attr("readonly", "readonly");
    }
    if ($('#hf_msg').val() == "1") {
        showWarningPopDialog('SV data saved successfully', 'Ok', '', 'ShowReviewSummary', '', 1, 'info');
    }
}

$(document).ready(function () {
    $('.table input[type=text]').each(function () {
        if ($(this).val().length > 0) {
        }
    });
    $('body').on('click', '#btnNewCal', function (e) {
        e.preventDefault();
        performBtnFlag = true;
        ShowHBSVConversion(performBtnFlag);
    });
    $('body').on('click', '#saveSVData', function (e) {
        e.preventDefault();
        var manual = $("#optManualHBToSV").prop("checked");
        var calculated = $("#optCalculateHBToSV").prop("checked");
        var performbtn = $('#hf_performBtnFlag').val();
        if (performbtn == 'true' || performbtn == 'True') {
            if (manual == true) {
                btnUseManual();
            }
            else if (calculated == true) {
                btnUseCalculated();
            }
        }
        else {
            saveSVData1(this);
        }
        
    });
    $('body').on('click', '#DimensionInfo #btnSVCancel', function (e) {
        var performbtn = $('#hf_performBtnFlag').val();
        if (performbtn == 'true' || performbtn == 'True') {
            ManageSVData('li10', 'manageSVId', performbtn);
        } else {
            BackToPreviousPage1(this);
        }
    });
});
$('body').on('keyup', '.edit-normal:not("#des, .span-type-text, .span-value-text")', function (e) {
    
    var value = $(this).val();
    let keycode = e.which || e.keyCode;

    // Check if key pressed is a special character
    if (keycode < 48 ||
        (keycode > 57 && keycode < 65) ||
        (keycode > 90 && keycode < 97) ||
        keycode > 122
    ) {
        // Restrict the special characters
        event.preventDefault();
        return false;
    }
        $(this).closest('.input-field').find('span.text-danger').text('');
    
   
    if (value.match(/[^1-9.]/) == null) {
        return true;
    }
        else {
        this.value = this.value.replace(/[^1-9\.]/, '');
         $(this).closest('.input-field').find('span').text('Only numbers and . are allowed');
       
    }
});
function saveSVData1() {
    
    startAnimation();
    var test = $("#DimensionInfo").serialize();
    var t = $('#WithSVTrain').val();
    $.ajax({
        url: '../Structures/StoreSVData',
        dataType: 'json',
        type: 'POST',
        data: $("#DimensionInfo").serialize(),
        success: function (result) {
            if (result == true) {
                saveDimension();
            }
        },
        error: function (xhr, status) {
        }
    });
}

function saveDimension() {
    $.ajax({
        url: '../Structures/SVDataJson',
        dataType: 'json',
        type: 'POST',
        data: { StructId: hf_StructureId, SectionId: hf_SectionId, SVDerivation: 275002, structName: hf_structName, ESRN: hf_ESRN },
        success: function (result) {
            stopAnimation();
            ShowSuccessModalPopup("SV Data saved successfully", "ShowReviewSummary")
        },
        error: function (xhr, status) {
        }
    });
}

function ShowReviewSummary() {
    CloseSuccessModalPopup();
    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + hf_StructureId);
}

function BackToPreviousPage1() {
    window.location.href = '../Structures/ReviewSummary' + EncodedQueryString('structureId=' + hf_StructureId);
}

function closeMp() {
    $('#causionReport').html('');
    $('#contactDetails').modal('hide');
    $("#saveSVData").attr("onclick", "saveSVData1()");

}

function ShowHBSVConversion(performBtnFlag) {
    startAnimation();
    $("#causionReport").load("../Structures/HBToSVConversion", { structureId: hf_StructureId, sectionId: hf_SectionId, structName: hf_structName, ESRN: hf_ESRN, performbtn: performBtnFlag }, function () {
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });

        $('#contactDetails').modal('show');
        stopAnimation();
        ShowWarningForNoCalculation();
        $("#pop-warning").css("display", "none");

        $("#saveSVData").attr("onclick", "btnUseManualHB()");
    });
}

