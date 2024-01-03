        $(document).ready(function ()
        {
            $("#btnCancel").on('click', BackRevStructSummary);
if($('#hf_Helpdest_redirect').val() ==  "true")
            {
                $("#chkWSBandingLimitDefault").attr("disabled", "disabled");
                $("#chkWSBandingLimitCustom").attr("disabled", "disabled");
                $("#chkGrossWeightEnable").attr("disabled", "disabled");
                $("#chkGrossWeightDisable").attr("disabled", "disabled");
                $("#chkAxelWeightEnable").attr("disabled", "disabled");
                $("#chkAxelWeightDisable").attr("disabled", "disabled");
                $("#chkWeightOverDistanceEnable").attr("disabled", "disabled");
                $("#chkWeightOverDistanceDisable").attr("disabled", "disabled");
                $("#chkAwrScreeningEnable").attr("disabled", "disabled");
                $("#chkAwrScreeningDisable").attr("disabled", "disabled");
                $("#chkSVBandingLimitDefault").attr("disabled", "disabled");
                $("#chkSVBandingLimitCustom").attr("disabled", "disabled");
                $("#chkSV_80Enable").attr("disabled", "disabled");
                $("#chkSV_80Disable").attr("disabled", "disabled");
                $("#chkSV_100Enable").attr("disabled", "disabled");
                $("#chkSV_100Disable").attr("disabled", "disabled");
                $("#chkSV_150Enable").attr("disabled", "disabled");
                $("#chkSV_150Disable").attr("disabled", "disabled");
                $("#chkSVTrainEnable").attr("disabled", "disabled");
                $("#chkSVTrainDisable").attr("disabled", "disabled");
            }

            $('#chkWSBandingLimitDefault').click(function () {
                $('#txtWSBandingMax').attr('disabled', true);
                $('#txtWSBandingMin').attr('disabled', true);
                $('#txtWSBandingMax').val('');
                $('#txtWSBandingMin').val('');
            });

            $('#chkWSBandingLimitCustom').click(function () {
                $('#txtWSBandingMax').attr('disabled', false);
                $('#txtWSBandingMin').attr('disabled', false);
            });

            $('#chkSVBandingLimitDefault').click(function () {
                $('#txtSVBandingMin').attr('disabled', true);
                $('#txtSVBandingMax').attr('disabled', true);
                $('#txtSVBandingMin').val('');
                $('#txtSVBandingMax').val('');
            });

            $('#chkSVBandingLimitCustom').click(function () {

                $('#txtSVBandingMin').attr('disabled', false);
                $('#txtSVBandingMax').attr('disabled', false);
            });

        if ('@Model.CustomWSBandLimitMin' != "" && '@Model.CustomWSBandLimitMax' != "") {
            $('#chkWSBandingLimitCustom').prop('checked', true)
            $('#chkWSBandingLimitDefault').prop('checked', false)
            $("#txtWSBandingMax, #txtWSBandingMin").prop("disabled", false);
        }
        else {
            $('#chkWSBandingLimitCustom').prop('checked', false)
            $('#chkWSBandingLimitDefault').prop('checked', true)
            $("#txtWSBandingMax, #txtWSBandingMin").prop("disabled", false);
        }

        if ('@Model.EnableGrossWeight' == null || '@Model.EnableGrossWeight'=="") {
            $('#spnGrossWeight').text("Not available");
            $("#chkGrossWeightEnable, #chkGrossWeightDisable").prop("checked", false);
            $("#chkGrossWeightEnable, #chkGrossWeightDisable").prop("disabled", true);
        }
        else if ('@Model.EnableGrossWeight' == 1) {
            $('#spnGrossWeight').text("Available");
            $("#chkGrossWeightEnable").prop("checked", true);
            $("#chkGrossWeightDisable").prop("checked", false);
        }
        else if ('@Model.EnableGrossWeight' == 0) {
            $('#spnGrossWeight').text("Available");
            $("#chkGrossWeightEnable").prop("checked", false);
            $("#chkGrossWeightDisable").prop("checked", true);
        }

        if ('@Model.EnableAxleWeight' == null || '@Model.EnableAxleWeight' == "") {
            $('#spnAxelWeight').text("Not available");
            $("#chkAxelWeightEnable, #chkAxelWeightDisable").prop("checked", false);
            $("#chkAxelWeightEnable, #chkAxelWeightDisable").prop("disabled", true);
        }
        else if ('@Model.EnableAxleWeight' == 1) {
            $('#spnAxelWeight').text("Available");
            $("#chkAxelWeightEnable").prop("checked", true);
            $("#chkAxelWeightDisable").prop("checked", false);
        }
        else if ('@Model.EnableAxleWeight' == 0) {
            $('#spnAxelWeight').text("Available");
            $("#chkAxelWeightEnable").prop("checked", false);
            $("#chkAxelWeightDisable").prop("checked", true);
        }

        if ('@Model.EnableWeightOverDist' == null || '@Model.EnableWeightOverDist' == "") {
            $('#spnWeightOverDistance').text("Not available");
            $("#chkWeightOverDistanceEnable, #chkWeightOverDistanceDisable").prop("checked", false);
            $("#chkWeightOverDistanceEnable, #chkWeightOverDistanceDisable").prop("disabled", true);
        }
        else if ('@Model.EnableWeightOverDist' == 1) {
            $('#spnWeightOverDistance').text("Available");
            $("#chkWeightOverDistanceEnable").prop("checked", true);
            $("#chkWeightOverDistanceDisable").prop("checked", false);
         }
        else if ('@Model.EnableWeightOverDist' == 0) {
             $('#spnWeightOverDistance').text("Available");
             $("#chkWeightOverDistanceEnable").prop("checked", false);
             $("#chkWeightOverDistanceDisable").prop("checked", true);
         }

        if ('@Model.EnableAWR' == null || '@Model.EnableAWR' == "") {
            $('#spnAwrScreening').text("Not available");
            $("#chkAwrScreeningEnable, #chkAwrScreeningDisable").prop("checked", false);
            $("#chkAwrScreeningEnable, #chkAwrScreeningDisable").prop("disabled", true);
        }
        else if ('@Model.EnableAWR' == 1) {
            $('#spnAwrScreening').text("Available");
            $("#chkAwrScreeningEnable").prop("checked", true);
            $("#chkAwrScreeningDisable").prop("checked", false);
        }
        else if ('@Model.EnableAWR' == 0) {
            $('#spnAwrScreening').text("Available");
            $("#chkAwrScreeningEnable").prop("checked", false);
            $("#chkAwrScreeningDisable").prop("checked", true);
        }


        if ('@Model.CustomSVBandLimitMin' != "" && '@Model.CustomSVBandLimitMax' != "") {
            $('#chkSVBandingLimitCustom').prop('checked', true)
            $('#chkSVBandingLimitDefault').prop('checked', false)
            $("#txtSVBandingMax, #txtSVBandingMin").prop("disabled", false);
        }
        else {
            $('#chkSVBandingLimitCustom').prop('checked', false)
            $('#chkSVBandingLimitDefault').prop('checked', true)
            $("#txtSVBandingMax, #txtSVBandingMin").prop("disabled", false);
        }

            if ('@Model.EnableSV80' == null || '@Model.EnableSV80' == "") {
            $('#spnSV_80').text("Not available");
            $("#chkSV_80Enable, #chkSV_80Disable").prop("checked", false);
            $("#chkSV_80Enable, #chkSV_80Disable").prop("disabled", true);
        }
        else if ('@Model.EnableSV80' == 1) {
            $('#spnSV_80').text("Available");
            $("#chkSV_80Enable").prop("checked", true);
            $("#chkSV_80Disable").prop("checked", false);
        }
        else if ('@Model.EnableSV80' == 0) {
            $('#spnSV_80').text("Available");
            $("#chkSV_80Enable").prop("checked", false);
            $("#chkSV_80Disable").prop("checked", true);
        }

        if ('@Model.EnableSV100' == null || '@Model.EnableSV100' == "") {
            $('#spnSV_100').text("Not available");
            $("#chkSV_100Enable, #chkSV_100Disable").prop("checked", false);
            $("#chkSV_100Enable, #chkSV_100Disable").prop("disabled", true);
        }
        else if ('@Model.EnableSV100' == 1) {
            $('#spnSV_100').text("Available");
            $("#chkSV_100Enable").prop("checked", true);
            $("#chkSV_100Disable").prop("checked", false);
        }
        else if ('@Model.EnableSV100' == 0) {
            $('#spnSV_100').text("Available");
            $("#chkSV_100Enable").prop("checked", false);
            $("#chkSV_100Disable").prop("checked", true);
        }

        if ('@Model.EnableSV150' == null || '@Model.EnableSV150' == "") {
            $('#spnSV_150').text("Not available");
            $("#chkSV_150Enable, #chkSV_150Disable").prop("checked", false);
            $("#chkSV_150Enable, #chkSV_150Disable").prop("disabled", true);
        }
        else if ('@Model.EnableSV150' == 1) {
            $('#spnSV_150').text("Available");
            $("#chkSV_150Enable").prop("checked", true);
            $("#chkSV_150Disable").prop("checked", false);
         }
        else if ('@Model.EnableSV150' == 0) {
            $('#spnSV_150').text("Available");
            $("#chkSV_150Enable").prop("checked", false);
            $("#chkSV_150Disable").prop("checked", true);
        }

        if ('@Model.EnableSVTrain' == null || '@Model.EnableSVTrain' == "") {
            $('#spnSV-Train').text("Not available");
            $("#chkSVTrainEnable, #chkSVTrainDisable").prop("checked", false);
            $("#chkSVTrainEnable, #chkSVTrainDisable").prop("disabled", true);
        }
        else if ('@Model.EnableSVTrain' == 1) {
            $('#spnSV-Train').text("Available");
            $("#chkSVTrainEnable").prop("checked", true);
            $("#chkSVTrainDisable").prop("checked", false);
         }
        else if ('@Model.EnableSVTrain' == 0) {
            $('#spnSV-Train').text("Available");
            $("#chkSVTrainEnable").prop("checked", false);
            $("#chkSVTrainDisable").prop("checked", true);
        }


        $('#btnConfigure').click(function () {
            @* @Html.("ConfigureBandings","Structures",)*@
            window.location.href = "../Structures/ConfigureBandings" + EncodedQueryString("structureID=" + '@ViewBag.StructureId' + "&sectionID=" + '@ViewBag.SectionId' + "&structName=" + '@ViewBag.StructName' + "&ESRN=" + '@ViewBag.ESRN');
        });

        $('#btnSave').click(function () {
            updateManageICA();
        });

        $("#txtWSBandingMin").keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                (e.keyCode == 65 && e.ctrlKey === true) ||
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                return;
            }
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
        $("#txtWSBandingMax").keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                (e.keyCode == 65 && e.ctrlKey === true) ||
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                return;
            }
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });

        $("#txtSVBandingMin").keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                (e.keyCode == 65 && e.ctrlKey === true) ||
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                return;
            }
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });

        $("#txtSVBandingMax").keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                (e.keyCode == 65 && e.ctrlKey === true) ||
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                return;
            }
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });

    });

    function updateManageICA() {
        
        var WEIGHT_SCREENING_LOWER, WEIGHT_SCREENING_UPPER, ENABLE_WEIGHT_GROSS, ENABLE_WEIGHT_AXLE, ENABLE_WEIGHT_AWR, ENABLE_WEIGHT_OVER_DISTANCE;
        var SV_SCREENING_LOWER, SV_SCREENING_UPPER, ENABLE_SV_80, ENABLE_SV_100, ENABLE_SV_150, ENABLE_SV_TRAIN;

        if ($('#chkWSBandingLimitDefault').attr('checked')) {
            WEIGHT_SCREENING_LOWER = null;
            WEIGHT_SCREENING_UPPER = null;
        } else {
            WEIGHT_SCREENING_LOWER = $("#txtWSBandingMin").val();
            WEIGHT_SCREENING_UPPER = $("#txtWSBandingMax").val();
        }

        if ($("#chkGrossWeightEnable").attr('disabled') && $("#chkGrossWeightDisable").attr('disabled')) {
            ENABLE_WEIGHT_GROSS = null;
        }
        else {
            if (document.getElementById('chkGrossWeightEnable').checked) {
                ENABLE_WEIGHT_GROSS = 1;
            }
            else {
                ENABLE_WEIGHT_GROSS = 0;
            }
        }

        if ($("#chkAxelWeightEnable").attr('disabled') && $("#chkAxelWeightDisable").attr('disabled')) {
            ENABLE_WEIGHT_AXLE = null;
        }
        else {
            if (document.getElementById('chkAxelWeightEnable').checked) {

                ENABLE_WEIGHT_AXLE = 1;
            }
            else {
                ENABLE_WEIGHT_AXLE = 0;
            }
        }

        if ($("#chkWeightOverDistanceEnable").attr('disabled') && $("#chkWeightOverDistanceDisable").attr('disabled')) {
            ENABLE_WEIGHT_OVER_DISTANCE = null;
        }
        else {
            if (document.getElementById('chkWeightOverDistanceEnable').checked) {
                ENABLE_WEIGHT_OVER_DISTANCE = 1;
            }

            else {
                ENABLE_WEIGHT_OVER_DISTANCE = 0;
            }
        }


        if ($("#chkAwrScreeningEnable").attr('disabled') && $("#chkAwrScreeningDisable").attr('disabled')) {
            ENABLE_WEIGHT_AWR = null;
        }
        else {
            if (document.getElementById('chkAwrScreeningEnable').checked) {
                ENABLE_WEIGHT_AWR = 1;
            }
            else {
                ENABLE_WEIGHT_AWR = 0;
            }
        }

        //======================================================

        if ($('#chkSVBandingLimitDefault').attr('checked')) {
            SV_SCREENING_LOWER = null;
            SV_SCREENING_UPPER = null;
        } else {
            SV_SCREENING_LOWER = $("#txtSVBandingMin").val();
            SV_SCREENING_UPPER = $("#txtSVBandingMax").val();
        }


        if ($("#chkSV_80Enable").attr('disabled') && $("#chkSV_80Disable").attr('disabled')) {
            ENABLE_SV_80 = null;
        }
        else {
            if (document.getElementById('chkSV_80Enable').checked) {
                ENABLE_SV_80 = 1;
            }
            else {
                ENABLE_SV_80 = 0;
            }
        }

        if ($("#chkSV_100Enable").attr('disabled') && $("#chkSV_100Disable").attr('disabled')) {
            ENABLE_SV_100 = null;
        }
        else {
                if (document.getElementById('chkSV_100Enable').checked) {
                ENABLE_SV_100 = 1;
            }
            else
                {
                ENABLE_SV_100 = 0;
            }
        }

        if ($("#chkSV_150Enable").attr('disabled') && $("#chkSV_150Disable").attr('disabled')) {
            ENABLE_SV_150 = null;
        }
        else {
            if (document.getElementById('chkSV_150Enable').checked) {

                ENABLE_SV_150 = 1;
            }
            else {
                ENABLE_SV_150 = 0;
            }
        }

        if ($("#chkSVTrainEnable").attr('disabled') && $("#chkSVTrainDisable").attr('disabled')) {
            ENABLE_SV_TRAIN = null;
        }
        else {
            if (document.getElementById('chkSVTrainEnable').checked) {
                ENABLE_SV_TRAIN = 1;
            }
            else {
                ENABLE_SV_TRAIN = 0;
            }
        }
        var WEIGHT_SCREENING_LOWER, WEIGHT_SCREENING_UPPER, ENABLE_WEIGHT_GROSS, ENABLE_WEIGHT_AXLE, ENABLE_WEIGHT_AWR, ENABLE_WEIGHT_OVER_DISTANCE;
        var SV_SCREENING_LOWER, SV_SCREENING_UPPER, ENABLE_SV_80, ENABLE_SV_100, ENABLE_SV_150, ENABLE_SV_TRAIN;
        startAnimation();

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '@Url.Action("UpdateStructureICAUsage", "Structures")',
            data: {
                weightScreeningLower: WEIGHT_SCREENING_LOWER, weightScreeningUpper: WEIGHT_SCREENING_UPPER, svScreeningLower: SV_SCREENING_LOWER, svScreeningUpper: SV_SCREENING_UPPER,
                enableWeightGross : ENABLE_WEIGHT_GROSS, enableWeightAxle : ENABLE_WEIGHT_AXLE, enableWeightAWR:ENABLE_WEIGHT_AWR, enableWeightOverDistance:ENABLE_WEIGHT_OVER_DISTANCE,
                enableSV80: ENABLE_SV_80, enableSV100: ENABLE_SV_100, enableSV150: ENABLE_SV_150, enableSVTrain: ENABLE_SV_TRAIN, structureID: '@ViewBag.StructureId', sectionID: '@ViewBag.SectionId'

            },
            async:false,

            beforeSend: function (xhr) {
            }
        }).done(function (Result) {
            
            if (Result.Success) {
                ShowSuccessModalPopup("ICA usage saved successfully.", "ShowReviewSummary");
             }
             else {
               //  showWarningDialog('Error on the page.', 'Ok', '', ShowReviewSummary, '', 3, 'error');
             }
             //var dataCollection = Result;
             //if (dataCollection.result.length > 0) {
             //}
        }).fail(function (error, a, b) {
        }).always(function (xhr) {
            stopAnimation();
         });
    }


    function ShowReviewSummary() {
        window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + '@ViewBag.structureID');

    }

    function BackRevStructSummary() {
        window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + '@ViewBag.structureID');
        }

