
    $(document).ready(function () {
        $("#optManualHBToSV").on('click', showManualHbToSV);
        CheckSessionTimeOut();
        ShowWarningForNoCalculation();
        $("#dialogue").find('.head').css("width", "690");

        if ('@V_SVDerivation' == "275002") {
            $("#optManualHBToSV").prop("checked", true);
            $("#ManualSV80, #ManualSV100, #ManualSV150, #ManualSVTrain, #ManualSVTT").prop("disabled", false);
            $("#btnUseManual").show();
            $("#btnUseCalculated").hide();
        }
        else if ('@V_SVDerivation' == "275003") {
            $("#optCalculateHBToSV").prop("checked", true);
            $("#ManualSV80, #ManualSV100, #ManualSV150, #ManualSVTrain, #ManualSVTT").prop("disabled", true);
            $("#btnUseManual").hide();
            $("#btnUseCalculated").show();
        }
        $("#pop-warning").css("display", "none");

    })
    function showManualHbToSV() {
      //  $("input[name=ManualSV]").prop("disabled", false);
        $("#ManualSV80, #ManualSV100, #ManualSV150, #ManualSVTrain, #ManualSVTT").prop("disabled", false);
        $("#btnUseManual").show();
        $("#btnUseCalculated").hide();
    }


    function showCalculatedHbToSV() {
        $("#ManualSV80, #ManualSV100, #ManualSV150, #ManualSVTrain, #ManualSVTT").prop("disabled", true);
        $("#btnUseManual").hide();
        $("#btnUseCalculated").show();
    }


    $("#btnUseCalculated").click(function () {
        var v_HBWithLoad = $("#HBWithLoad").val();
        var v_HBWithoutLoad = $("#HBWithoutLoad").val();

        $.ajax({
            type: 'POST',
            //dataType: 'json',
            url: '@Url.Action("GetCalculatedHBToSV", "Structures")',
            data: { structureID: '@ViewBag.StructureId', sectionID: '@ViewBag.SectionId', hbWithLoad: v_HBWithLoad, hbWithoutLoad: v_HBWithoutLoad },
            async: false,

            beforeSend: function (xhr) {
                // startAnimation();
            }
        }).done(function (Result) {
            var dataCollection = Result;
            //var vehicleType = "";
            if (dataCollection.result.length >= 0) {
                ShowSuccessModalPopup("SV Data saved successfully", "ShowSVData");

            //    for (var i = 0; i < dataCollection.result.length; i++) {
            //        vehicleType = dataCollection.result[i].VehicleType;
            //        if (vehicleType == "340002") {
            //            $("#spnCalSV80").html(dataCollection.result[i].CalculatedFactor);
            //        }
            //        else if (vehicleType == "340003") {
            //            $("#spnCalSV100").html(dataCollection.result[i].CalculatedFactor);
            //        }
            //        else if (vehicleType == "340004") {
            //            $("#spnCalSV150").html(dataCollection.result[i].CalculatedFactor);
            //        }
            //        else if (vehicleType == "340005") {
            //            $("#spnCalSVTrain").html(dataCollection.result[i].CalculatedFactor);
            //        }
            //        else if (vehicleType == "340006") {
            //            $("#spnCalSVTT").html(dataCollection.result[i].CalculatedFactor);
            //        }
            //    }
            }

        }).fail(function (error, a, b) {
        }).always(function (xhr) {
            //stopAnimation();
        });
    });

    function btnUseManualHB() {
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
        //var decimalRegex = /^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$/;


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
        else if ((v_SV80.length > 0)){
            if ((!decimalRegex.test(v_SV80)) || parseFloat(v_SV80) < 0 || parseFloat(v_SV80) > 99.999) {
               // $('#spnErrManual').html('Value should be in range 0-99.999');
                errFlag = 1;
            }
        }
        else if ((v_SV100.length > 0)) {
            if ((!decimalRegex.test(v_SV100)) || parseFloat(v_SV100) < 0 || parseFloat(v_SV100) > 99.999) {
               // $('#spnErrManual').html('Value should be in range 0-99.999');
                errFlag = 1;
            }
        }
        else if ((v_SV150.length > 0)) {
            if ((!decimalRegex.test(v_SV150)) || parseFloat(v_SV150) < 0 || parseFloat(v_SV150) > 99.999) {
               // $('#spnErrManual').html('Value should be in range 0-99.999');
                errFlag = 1;
            }
        }
        else if ((v_SVTrain.length > 0)) {
            if ((!decimalRegex.test(v_SVTrain)) || parseFloat(v_SVTrain) < 0 || parseFloat(v_SVTrain) > 99.999) {
               // $('#spnErrManual').html('Value should be in range 0-99.999');
                errFlag = 1;
            }
        }
        else if ((v_SVTT.length > 0)) {
            if ((!decimalRegex.test(v_SVTT)) || parseFloat(v_SVTT) < 0 || parseFloat(v_SVTT) > 99.999) {
                //$('#spnErrManual').html('Value should be in range 0-99.999');
                errFlag = 1;
            }
        }

        //if (errFlag == 0) {
            $(".error").html("");

            $.ajax({
                type: 'POST',
                //dataType: 'json',
                url: '@Url.Action("SaveHBToSV", "Structures")',
                data: { structureID: '@ViewBag.StructureId', sectionID: '@ViewBag.SectionId', hbWithLoad: v_HBWithLoad, hbWithoutLoad: v_HBWithoutLoad, SV80: v_SV80, SV100: v_SV100, SV150: v_SV150, SVTrain: v_SVTrain, SVTT: v_SVTT },
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
                //stopAnimation();
            });
        //}
        //else {
        //    return false;
        //}


    }

    function ShowSVData() {
        //WarningCancelBtn();
        CloseSuccessModalPopup();
        var id = 'li10';
        var idDiv = 'manageSVId';
                    startAnimation();
            $("#generalSettingsId").empty();
            $('#bottomSection').empty();
            var strucType;
if($('#hf_sectionType').val() ==  "underbridge")
            {
                strucType = 1;
            }
if($('#hf_sectionType').val() ==  "overbridge")
            {
                strucType = 2;
            }
            else
            {
                strucType = 3;
            }
            $("#generalSettingsId").load('../Structures/SVData?StructId=' + '@ViewBag.StructureId' + "&sectionId=" +  @ViewBag.sectionId + "&structName=" + '@ViewBag.ESRN' + "&ESRN=" + '@ViewBag.ESRN',
            function () {
                openDetails(id, idDiv);
                stopAnimation();

            }
        );
        //window.location.href = "../Structures/SVData?StructId=" + '@ViewBag.StructureId' + '&SectionId=' + '@ViewBag.SectionId' + '&structName=' + '@ViewBag.structName' + '&ESRN=' + '@ViewBag.ESRN';
    }

    function ShowWarningForNoCalculation() {
        
        var calculatedValueExists = $('#CalculatedExists').val();
        if (calculatedValueExists != 1) {
            ShowInfoPopup("HB-SV conversion could not be performed due to structure properties, please refer to the online help for additional information.", 'CloseInfoPopup');
        }
    }
    function CloseInfoPopup() {
        $('#InfoPopup').modal('hide');
    }
