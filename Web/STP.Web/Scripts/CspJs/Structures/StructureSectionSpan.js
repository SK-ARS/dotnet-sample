    var numberValidPattern = new RegExp(/^(\+)?[0-9\ -]+$/);
    var lengthValidPattern = new RegExp(/^[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)$/);
    $(document).ready(function () {
if($('#hf_EditSaveFlag').val() ==  1) {
            $("#Position").attr("readonly", "readonly");
        }
        $("#SpanNo").attr("readonly", "readonly");
        $('#StructType1').change(); $('#StructType2').change(); $('#StructType3').change();
        $('#SpanConstructionType1').change(); $('#SpanConstructionType2').change(); $('#SpanConstructionType3').change();
        $('#SpanDeckMaterial1').change(); $('#SpanDeckMaterial2').change(); $('#SpanDeckMaterial3').change();
        $('#SpanBearingsType1').change(); $('#SpanBearingsType2').change(); $('#SpanBearingsType3').change();
        $('#SpanFoundationType1').change(); $('#SpanFoundationType2').change(); $('#SpanFoundationType3').change();

        $("#btnSave").on('click', storeSpanData);
        $("#btnCancel").on('click', cancel);
    });
    function cancel() {
        CloseSuccessModalPopup();
        startAnimation();
        $("#generalSettingsId").load('../Structures/StructureDimensions?structureId=' + '@ViewBag.structureId' + "&sectionId=" +  @ViewBag.sectionId + "&structureNm=" + '@ViewBag.structureName' + "&ESRN=" + '@ViewBag.ESRN' + "&strucType=" + @ViewBag.structureType,
            function () {
                $('#managedimensiosId').show();
                stopAnimation();
            }
        );
    }
    function storeSpanData() {
        startAnimation();
        var spanSequenceValid = validateSpanSequence();
        var positionValid = validatePosition();
        var spLengthValid = validateSpLength();
        var duplicateCheck = DuplicateCheck();
        if (spanSequenceValid && positionValid && spLengthValid && duplicateCheck) {
            $.ajax({
                url: '../Structures/StoreSpanData',
                dataType: 'json',
                type: 'POST',
                data: $("#SpanInfo").serialize(),
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
    function DuplicateCheck() {

        var spanPos = $("#Position").val();
        var saveFlag = 0;
        $("#duplicateCheck").hide();
if($('#hf_EditSaveFlag').val() ==  1 && spanPos.length>0) {

            $.ajax({
                url: '/Structures/ValidateSpanPosition',
                dataType: 'json',
                async: false,
                type: 'POST',
                data: { structureId: '@ViewBag.structureId', sectionId: '@ViewBag.sectionId', spanPosition: spanPos },
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
        startAnimation();
        $.ajax({
            url: '../Structures/SaveStructSectionSpan',
            dataType: 'json',
            type: 'POST',
            data: { structureId: '@ViewBag.structureId', sectionId: '@ViewBag.sectionId', editSaveFlag: '@ViewBag.EditSaveFlag' },
            success: function (result) {
                if (result == 1) {
                    $("#generalSettingsId").load('../Structures/StructureDimensions?structureId=' + '@ViewBag.structureId' + "&sectionId=" +  @ViewBag.sectionId + "&structureNm=" + '@ViewBag.structureName' + "&ESRN=" + '@ViewBag.ESRN' + "&strucType=" + @ViewBag.structureType,
                    function () {
                        $('#spanSection').empty();
                        $('#managedimensiosId').show();
                        stopAnimation();
if($('#hf_EditSaveFlag').val() == = "0") {
                            ShowSuccessModalPopup("Structure span saved successfully","cancel");
                        }
                        else {
                            ShowSuccessModalPopup("Structure span updated successfully", "cancel");
                        }

                    }
                );
                }
                else {
                    ShowWarningPopup("Structure span saving failed");
                }
            },
            error: function (xhr, status) {
            }
        });
    }
    function validateSpanSequence() {
        var spanSequenceValid = true;
        if ($("#Sequence").val() != '') {
            if (!numberValidPattern.test($("#Sequence").val())) {

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
            if (!numberValidPattern.test($("#Position").val())) {

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
        if ($("#spLength").val() != '') {
            if (!lengthValidPattern.test($("#spLength").val())) {
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

    $('#StructType1').change(function () {
        var selectedVal = $('#StructType1 option:selected').attr('value');
        var txtValue = $('#TxtStructType1').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#TxtStructType1').prop('disabled', false);
            if ($('#TxtStructType1').val() != "")
                $('#StructType1').val("Other");
        }
        else {
            $('#TxtStructType1').prop('disabled', true);
            $('#TxtStructType1').val("");
        }
    });
    $('#StructType2').change(function () {
        var selectedVal = $('#StructType2 option:selected').attr('value');
        var txtValue = $('#TxtStructType2').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#TxtStructType2').prop('disabled', false);
            if ($('#TxtStructType2').val() != "")
                $('#StructType2').val("Other");
        }
        else {
            $('#TxtStructType2').prop('disabled', true);
            $('#TxtStructType2').val("");
        }
    });
    $('#StructType3').change(function () {
        var selectedVal = $('#StructType3 option:selected').attr('value');
        var txtValue = $('#TxtStructType3').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#TxtStructType3').prop('disabled', false);
            if ($('#TxtStructType3').val() != "")
                $('#StructType3').val("Other");
        }
        else {
            $('#TxtStructType3').prop('disabled', true);
            $('#TxtStructType3').val("");
        }
    });
    $('#SpanConstructionType1').change(function () {
        var selectedVal = $('#SpanConstructionType1 option:selected').attr('value');
        var txtValue = $('#SpanTxtConstructionType1').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#SpanTxtConstructionType1').prop('disabled', false);
            if ($('#SpanTxtConstructionType1').val() != "")
                $('#SpanConstructionType1').val("Other");
        }
        else {
            $('#SpanTxtConstructionType1').prop('disabled', true);
            $('#SpanTxtConstructionType1').val("");
        }
    });

    $('#SpanConstructionType2').change(function () {
        var selectedVal = $('#SpanConstructionType2 option:selected').attr('value');
        var txtValue = $('#SpanTxtConstructionType2').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#SpanTxtConstructionType2').prop('disabled', false);
            if ($('#SpanTxtConstructionType2').val() != "")
                $('#SpanConstructionType2').val("Other");
        }
        else {
            $('#SpanTxtConstructionType2').prop('disabled', true);
            $('#SpanTxtConstructionType2').val("");
        }
    });
    $('#SpanConstructionType3').change(function () {
        var selectedVal = $('#SpanConstructionType3 option:selected').attr('value');
        var txtValue = $('#SpanTxtConstructionType3').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#SpanTxtConstructionType3').prop('disabled', false);
            if ($('#SpanTxtConstructionType3').val() != "")
                $('#SpanConstructionType3').val("Other");
        }
        else {
            $('#SpanTxtConstructionType3').prop('disabled', true);
            $('#SpanTxtConstructionType3').val("");
        }
    });
    $('#SpanDeckMaterial1').change(function () {

        var selectedVal = $('#SpanDeckMaterial1 option:selected').attr('value');
        var txtValue = $('#SpanTxtDeckMaterial1').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#SpanTxtDeckMaterial1').prop('disabled', false);
            if ($('#SpanTxtDeckMaterial1').val() != "")
                $('#SpanDeckMaterial1').val("Other");
        }
        else {
            $('#SpanTxtDeckMaterial1').prop('disabled', true);
            $('#SpanTxtDeckMaterial1').val("");
        }
    });
    $('#SpanDeckMaterial2').change(function () {

        var selectedVal = $('#SpanDeckMaterial2 option:selected').attr('value');
        var txtValue = $('#SpanTxtDeckMaterial2').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#SpanTxtDeckMaterial2').prop('disabled', false);
            if ($('#SpanTxtDeckMaterial2').val() != "")
                $('#SpanDeckMaterial2').val("Other");
        }
        else {
            $('#SpanTxtDeckMaterial2').prop('disabled', true);
            $('#SpanTxtDeckMaterial2').val("");
        }
    });
    $('#SpanDeckMaterial3').change(function () {

        var selectedVal = $('#SpanDeckMaterial3 option:selected').attr('value');
        var txtValue = $('#SpanTxtDeckMaterial3').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#SpanTxtDeckMaterial3').prop('disabled', false);
            if ($('#SpanTxtDeckMaterial3').val() != "")
                $('#SpanDeckMaterial3').val("Other");
        }
        else {
            $('#SpanTxtDeckMaterial3').prop('disabled', true);
            $('#SpanTxtDeckMaterial3').val("");
        }
    });

    $('#SpanBearingsType1').change(function () {

        var selectedVal = $('#SpanBearingsType1 option:selected').attr('value');
        var txtValue = $('#SpanTxtBearingsType1').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#SpanTxtBearingsType1').prop('disabled', false);
            if ($('#SpanTxtBearingsType1').val() != "")
                $('#SpanBearingsType1').val("Other");
        }
        else {
            $('#SpanTxtBearingsType1').prop('disabled', true);
            $('#SpanTxtBearingsType1').val("");
        }
    });
    $('#SpanBearingsType2').change(function () {

        var selectedVal = $('#SpanBearingsType2 option:selected').attr('value');
        var txtValue = $('#SpanTxtBearingsType2').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#SpanTxtBearingsType2').prop('disabled', false);
            if ($('#SpanTxtBearingsType2').val() != "")
                $('#SpanBearingsType2').val("Other");
        }
        else {
            $('#SpanTxtBearingsType2').prop('disabled', true);
            $('#SpanTxtBearingsType2').val("");
        }
    });
    $('#SpanBearingsType3').change(function () {

        var selectedVal = $('#SpanBearingsType3 option:selected').attr('value');
        var txtValue = $('#SpanTxtBearingsType3').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#SpanTxtBearingsType3').prop('disabled', false);
            if ($('#SpanTxtBearingsType3').val() != "")
                $('#SpanBearingsType3').val("Other");
        }
        else {
            $('#SpanTxtBearingsType3').prop('disabled', true);
            $('#SpanTxtBearingsType3').val("");
        }
    });

    $('#SpanFoundationType1').change(function () {

        var selectedVal = $('#SpanFoundationType1 option:selected').attr('value');
        var txtValue = $('#SpanTxtFoundationType1').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#SpanTxtFoundationType1').prop('disabled', false);
            if ($('#SpanTxtFoundationType1').val() != "")
                $('#SpanFoundationType1').val("Other");
        }
        else {
            $('#SpanTxtFoundationType1').prop('disabled', true);
            $('#SpanTxtFoundationType1').val("");
        }
    });
    $('#SpanFoundationType2').change(function () {

        var selectedVal = $('#SpanFoundationType2 option:selected').attr('value');
        var txtValue = $('#SpanTxtFoundationType2').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {
            $('#SpanTxtFoundationType2').prop('disabled', false);
            if ($('#SpanTxtFoundationType2').val() != "")
                $('#SpanFoundationType2').val("Other");
        }
        else {
            $('#SpanTxtFoundationType2').prop('disabled', true);
            $('#SpanTxtFoundationType2').val("");
        }
    });
    $('#SpanFoundationType3').change(function () {

        var selectedVal = $('#SpanFoundationType3 option:selected').attr('value');
        var txtValue = $('#SpanTxtFoundationType3').val();
        if (selectedVal == "Other" || selectedVal == "" && (txtValue.length > 0)) {
            selectedVal = "Other";
        }
        if (selectedVal == "Other") {

            $('#SpanTxtFoundationType3').prop('disabled', false);
            if ($('#SpanTxtFoundationType3').val() != "")
                $('#SpanFoundationType3').val("Other");
        }

        else {

            $('#SpanTxtFoundationType3').prop('disabled', true);

            $('#SpanTxtFoundationType3').val("");
        }
    });
