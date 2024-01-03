
var hf_V_SVDerivation = $("#hf_V_SVDerivation").val();
var strcutId = $("#hf_StructureId").val();
var sectId = $("#hf_SectionId").val();
var ESRN = $("#hf_ESRN").val();
$("#optManualHBToSV").prop("checked", true);
$(document).ready(function () {
    
    $('body').on('click', '#btnUseManual', function (e) {
        e.preventDefault();
        btnUseManual();
    });
    $('body').on('click', '#btnUseCalculated', function (e) {
        e.preventDefault();
        btnUseCalculated();
    });
    CheckSessionTimeOut();
    $("#optManualHBToSV").prop("checked", true);
    $("#dialogue").find('.head').css("width", "690");

    if (hf_V_SVDerivation == "275002") {
        $("#optManualHBToSV").prop("checked", true);
        $("#ManualSV80, #ManualSV100, #ManualSV150, #ManualSVTrain, #ManualSVTT").prop("disabled", false);
        $("#btnUseManual").show();
        $("#btnUseCalculated").hide();
    }
    else if (hf_V_SVDerivation == "275003") {
        $("#optCalculateHBToSV").prop("checked", true);
        $("#ManualSV80, #ManualSV100, #ManualSV150, #ManualSVTrain, #ManualSVTT").prop("disabled", true);
        $("#btnUseManual").hide();
        $("#btnUseCalculated").show();
    }
    $("#pop-warning").css("display", "none");

});


    function btnUseCalculated() {
        var v_HBWithLoad = $("#HBWithLoad").val();
        var v_HBWithoutLoad = $("#HBWithoutLoad").val();
        var sectionid = $("#hf_SectionId").val();
        $.ajax({
            type: 'POST',
            url: '../Structures/GetCalculatedHBToSV',
            data: { structureID: strcutId, sectionID: sectionid, hbWithLoad: v_HBWithLoad, hbWithoutLoad: v_HBWithoutLoad },
            async: false,

            beforeSend: function (xhr) {
            }
        }).done(function (Result) {
            var dataCollection = Result;
            if (dataCollection.result.length >= 0) {
                ShowSuccessModalPopup("SV Data saved successfully", "ShowSVData");
            }

        }).fail(function (error, a, b) {
        }).always(function (xhr) {
        });
    }

function btnUseManual() {
    var sectionid = $("#hf_SectionId").val();
    var v_HBWithLoad = $("#HBWithLoad").val();
    var v_HBWithoutLoad = $("#HBWithoutLoad").val()
    var v_SV80, v_SV100, v_SV150, v_SVTrain, v_SVTT;
    var v_CtlSV80, v_CtlSV100, v_CtlSV150, v_CtlSVTrain, v_CtlSVTT;
    var errFlag = 0;
    if ($("#optManualHBToSV").prop('checked') == true) {
        v_SV80 = $("#ManualSV80").val();
        v_SV100 = $("#ManualSV100").val();
        v_SV150 = $("#ManualSV150").val();
        v_SVTrain = $("#ManualSVTrain").val();
        v_SVTT = $("#ManualSVTT").val();

        v_CtlSV80 = $("#ManualSV80");
        v_CtlSV100 = $("#ManualSV100");
        v_CtlSV150 = $("#ManualSV150");
        v_CtlSVTrain = $("#ManualSVTrain");
        v_CtlSVTT = $("#ManualSVTT");
    }

    var decimalRegex = /^[0-9]\d*(\.\d+)?$/;
    if (v_HBWithLoad.length > 0 && (!decimalRegex.test(v_HBWithLoad))) {
        $('#spnErrHBWithLoad').html('Please enter a valid value');
        errFlag = 1;
    }

    if (v_HBWithoutLoad.length > 0 && (!decimalRegex.test(v_HBWithoutLoad))) {
        $('#spnErrHBWithoutLoad').html('Please enter a valid value');
        errFlag = 1;
    }

    if (v_SV80.length <= 0 && v_SV100 <= 0 && v_SV150 <= 0 && v_SVTrain <= 0 && v_SVTT <= 0) {
        $('#spnErrManual').html('Please enter a manual values');
        errFlag = 1;
    }
    else if ((v_SV80.length > 0)) {
        if ((!decimalRegex.test(v_SV80)) || parseFloat(v_SV80) < 0 || parseFloat(v_SV80) > 99.999) {
            errFlag = 1;
        }
    }
    else if ((v_SV100.length > 0)) {
        if ((!decimalRegex.test(v_SV100)) || parseFloat(v_SV100) < 0 || parseFloat(v_SV100) > 99.999) {
            errFlag = 1;
        }
    }
    else if ((v_SV150.length > 0)) {
        if ((!decimalRegex.test(v_SV150)) || parseFloat(v_SV150) < 0 || parseFloat(v_SV150) > 99.999) {
            errFlag = 1;
        }
    }
    else if ((v_SVTrain.length > 0)) {
        if ((!decimalRegex.test(v_SVTrain)) || parseFloat(v_SVTrain) < 0 || parseFloat(v_SVTrain) > 99.999) {
            errFlag = 1;
        }
    }
    else if ((v_SVTT.length > 0)) {
        if ((!decimalRegex.test(v_SVTT)) || parseFloat(v_SVTT) < 0 || parseFloat(v_SVTT) > 99.999) {
            errFlag = 1;
        }
    }

    $(".error").html("");

    $.ajax({
        type: 'POST',
        url: '../Structures/SaveHBToSV',
        data: { structureID: strcutId, sectionID: sectionid, hbWithLoad: v_HBWithLoad, hbWithoutLoad: v_HBWithoutLoad, SV80: v_SV80, SV100: v_SV100, SV150: v_SV150, SVTrain: v_SVTrain, SVTT: v_SVTT },
        async: false,

        beforeSend: function (xhr) {
            startAnimation();
        }
    }).done(function (Result) {
        stopAnimation();

        if (Result.result == 1) {
            ShowSuccessModalPopup("SV Data saved successfully", "ShowSVData");

        }

    }).fail(function (error, a, b) {
    }).always(function (xhr) {
    });
}

function ShowSVData() {
    CloseSuccessModalPopup();
var sectionid = $("#hf_SectionId").val();
    var id = 'li10';
    var idDiv = 'manageSVId';
    startAnimation();
    $("#generalSettingsId").empty();
    $('#bottomSection').empty();
    var strucType;
    if ($('#hf_sectionType').val() == "underbridge") {
        strucType = 1;
    }
    if ($('#hf_sectionType').val() == "overbridge") {
        strucType = 2;
    }
    else {
        strucType = 3;
    }
    $("#generalSettingsId").load('../Structures/SVData?StructId=' + strcutId + "&sectionId=" + sectionid + "&structName=" + ESRN + "&ESRN=" + ESRN,
        function () {
            openDetails(id, idDiv);
            stopAnimation();
              SVDataInit();
        });
}

function ShowWarningForNoCalculation() {

    var calculatedValueExists = $('#CalculatedExists').val();
    if (calculatedValueExists != 1) {
        ShowInfoPopup("HB-SV conversion could not be performed due to structure properties, please refer to the online help for additional information.");
    }
}
function CloseInfoPopup() {
    $('#InfoPopup').modal('hide');
}
