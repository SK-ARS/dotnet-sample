let 
$(document).ready(function () {
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
        showWarningPopDialog('SV data saved successfully.', 'Ok', '', 'ShowReviewSummary', '', 1, 'info');
    }

    $('.table input[type=text]').each(function () {
        if ($(this).val().length > 0) {
        }
    });
    $("#btnNewCal").on('click', ShowHBSVConversion);
    $("#saveSVData").on('click', saveSVData1);
    $("#btnCancel").on('click', BackToPreviousPage1);
});
function saveSVData1() {
    startAnimation();
    var test = $("#DimensionInfo").serialize();
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
        data: { StructId: '@ViewBag.StructureId', SectionId: '@ViewBag.SectionId', SVDerivation: 275002, structName: '@ViewBag.structName', ESRN: '@ViewBag.ESRN' },
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
    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + '@ViewBag.StructureId');
}

function BackToPreviousPage1() {
    window.location.href = '../Structures/ReviewSummary' + EncodedQueryString('structureId=' + '@ViewBag.StructureId');
}

function closeMp() {
    $('#causionReport').html('');
    $('#contactDetails').modal('hide');
    $("#saveSVData").attr("onclick", "saveSVData1()");

}

function ShowHBSVConversion() {   
    startAnimation();
    $("#causionReport").load("../Structures/HBToSVConversion", { structureId: '@ViewBag.StructureId', sectionId: '@ViewBag.SectionId', structName: '@ViewBag.structName', ESRN: '@ViewBag.ESRN' }, function () {
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });

        $('#contactDetails').modal('show');
        stopAnimation();
        $("#pop-warning").css("display", "none");

        $("#saveSVData").attr("onclick", "btnUseManualHB()");
    });   
}


