    var numberValidPattern = new RegExp(/^[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)$/);
    $(document).ready(function ()
    {
            //For Document status viewer
if($('#hf_Helpdest_redirect').val() ==  "true")
            {
                $("#Desc").attr("readonly", "readonly");
                $("#ObjectCarried").attr("readonly", "readonly");
                $("#ObjectCrossed").attr("readonly", "readonly");
                $("#SkewAngle").attr("readonly", "readonly");
                $("#Length").attr("readonly", "readonly");
                $("#MaxLength").attr("readonly", "readonly");
                $("#SpansCount").attr("readonly", "readonly");
                $("#DecksCount").attr("readonly", "readonly");
                $("#ConstructionType1").attr("disabled", "disabled");
                $("#ConstructionType2").attr("disabled", "disabled");
                $("#ConstructionType3").attr("disabled", "disabled");
                $("#DeckMaterial1").attr("disabled", "disabled");
                $("#DeckMaterial2").attr("disabled", "disabled");
                $("#DeckMaterial3").attr("disabled", "disabled");
                $("#BearingsType1").attr("disabled", "disabled");
                $("#BearingsType2").attr("disabled", "disabled");
                $("#BearingsType3").attr("disabled", "disabled");
                $("#FoundationType1").attr("disabled", "disabled");
                $("#FoundationType2").attr("disabled", "disabled");
                $("#FoundationType3").attr("disabled", "disabled");
                $("#CarrigewayWidth").attr("readonly", "readonly");
                $("#DeckWidth").attr("readonly", "readonly");
            }

            $('#ConstructionType1').change(); $('#ConstructionType2').change(); $('#ConstructionType3').change();
            $('#DeckMaterial1').change(); $('#DeckMaterial2').change(); $('#DeckMaterial3').change();
            $('#BearingsType1').change(); $('#BearingsType2').change(); $('#BearingsType3').change();
        $('#FoundationType1').change(); $('#FoundationType2').change(); $('#FoundationType3').change();


            if ('@Model' != null && '@Model.DeckId' != 0 && '@Model.DeckId' != null)
            {
                $("#hdn_DeckID").val(@Html.Raw(Json.Encode(Model.DeckId)));
            }
            if ('@Model' != null && '@Model.CarrigeWayId' != 0 && '@Model.CarrigeWayId' != null) {
                $("#hdn_CarrNo").val(@Html.Raw(Json.Encode(Model.CarrigeWayId)));
        }


        $("#addSpan").on('click', addSpan);
        $(".deleteSpan").on('click', deleteStructureSpan);
        $(".editDimension").on('click', editStructureDimension);
        $("#btnSaveData").on('click', saveData);
        $("#btnSuccess").on('click', Success);
    });

    function saveData() {
        if (@ViewBag.strucType == 2 || @ViewBag.strucType == 3) {
            StoreDimensionData();
        }
        var skewAngleValidate = validateSkewAngle();
        var lengthValidate = validateLength();
        var maxLengthValidate = validateMaxLength();
        var numSpanValidate = validateNumSpan();
        var numDeckValidate = validateNumDecks();
        var carriageValidate = validateCarriage();
        var deckWidthValidate = validateDeckWidth();
        if (skewAngleValidate && lengthValidate && maxLengthValidate && numSpanValidate && numDeckValidate &&
            carriageValidate && deckWidthValidate) {
            StoreDimensionData();
        }

    }
    function StoreDimensionData() {
        $.ajax({
            url: '../Structures/StoreDimensionData',
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
        startAnimation();
        $.ajax({
            url: '../Structures/EditDimCONSTRAINTS',
            dataType: 'json',
            type: 'POST',
            data: {structureId: '@ViewBag.structureId', sectionId: '@ViewBag.sectionId'},
            success: function (result) {
                $("#generalSettingsId").load('../Structures/StructureDimensions?structureId=' + '@ViewBag.structureId' + "&sectionId=" +  @ViewBag.sectionId + "&structureNm=" + '@ViewBag.structName' + "&ESRN=" + '@ViewBag.ESRN' + "&strucType=" + @ViewBag.strucType,
                    function () {
                        $('#managedimensiosId').show();
                        stopAnimation();
                        if (result == true) {
                            ShowSuccessModalPopup("Structure dimensions saved successfully", "Success")
                        }
                        else {
                            ShowErrorPopup("Structure dimensions saving failed");
                        }
                    }
                );

            },
            error: function (xhr, status) {
            }
        });
    }
    function addSpan() {
        startAnimation();
        $('#spanSection').load('@Url.Action("StructureSectionSpan", "Structures")', { structureId: '@ViewBag.structureId', sectionId: '@ViewBag.sectionId', spanNo: 0, editSaveFlag: 0, structureName: '@ViewBag.structName', ESRN: '@ViewBag.ESRN', structureType: @ViewBag.strucType}, function () {
            $("#dimensionView").empty();
            stopAnimation();
        });
    }
    function editDimension(spanNo) {
        startAnimation();
        $('#spanSection').load('@Url.Action("StructureSectionSpan", "Structures")', { structureId: '@ViewBag.StructureId', sectionId: '@ViewBag.sectionId', spanNo: spanNo, editSaveFlag: 1, structureName :'@ViewBag.structName',ESRN : '@ViewBag.ESRN',structureType : @ViewBag.strucType }, function () {
            $("#dimensionView").hide();
            stopAnimation();
        });
    }

    function deleteSpan(spanNo) {
        $("#hdnDeleteSpanNo").val(spanNo);
        ShowWarningPopup('Do you want to delete this span?', "DeleteSpanConfirm");
    }
    function DeleteSpanConfirm() {
        CloseWarningPopup();
        var spanNo = $("#hdnDeleteSpanNo").val();
        $.ajax({
            url: '/Structures/DeleteStructureSpan',
            dataType: 'json',
            async: false,
            type: 'POST',
            data: { structureId: '@ViewBag.structureId', sectionId: '@ViewBag.sectionId', spanNo: spanNo },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result == '1') {
                    ShowSuccessModalPopup("Span deleted successfully", "reload")
                }
                else {
                    ShowErrorPopup("Span deletion failed");
                }
            },
            error: function (xhr, status) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    function Success() {
        CloseSuccessModalPopup();
        location.reload();
    }
    function reload() {
        CloseSuccessModalPopup();
        startAnimation();
        $("#generalSettingsId").load('../Structures/StructureDimensions?structureId=' + '@ViewBag.structureId' + "&sectionId=" +  @ViewBag.sectionId + "&structureNm=" + '@ViewBag.structName' + "&ESRN=" + '@ViewBag.ESRN' + "&strucType=" + @ViewBag.strucType,
            function () {
                $('#managedimensiosId').show();
                stopAnimation();
            }
        );
    }
    $('#ConstructionType1').change(function () {
        var selectedVal = $('#ConstructionType1 option:selected').attr('value');

        if (selectedVal == "Other") {
            $('#TxtConstructionType1').prop('disabled', false);
            if ($('#TxtConstructionType1').val() != "")
                $('#ConstructionType1').val("Other");
        }
        else {
            $('#TxtConstructionType1').prop('disabled', true);
            $('#TxtConstructionType1').val("");
        }
    });
    $('#ConstructionType2').change(function () {

        var selectedVal = $('#ConstructionType2 option:selected').attr('value');

        if (selectedVal == "Other") {
            $('#TxtConstructionType2').prop('disabled', false);
            if ($('#TxtConstructionType2').val() != "")
                $('#ConstructionType2').val("Other");
        }
        else {
            $('#TxtConstructionType2').prop('disabled', true);
            $('#TxtConstructionType2').val("");
        }
    });
    $('#ConstructionType3').change(function () {

        var selectedVal = $('#ConstructionType3 option:selected').attr('value');

        if (selectedVal == "Other") {
            $('#TxtConstructionType3').prop('disabled', false);
            if ($('#TxtConstructionType3').val() != "")
                $('#ConstructionType3').val("Other");
        }
        else {
            $('#TxtConstructionType3').prop('disabled', true);
            $('#TxtConstructionType3').val("");
        }
    });

    $('#DeckMaterial1').change(function () {

        var selectedVal = $('#DeckMaterial1 option:selected').attr('value');

        if (selectedVal == "Other") {
            $('#TxtDeckMaterial1').prop('disabled', false);
            if ($('#TxtDeckMaterial1').val() != "")
                $('#DeckMaterial1').val("Other");
        }
        else {
            $('#TxtDeckMaterial1').prop('disabled', true);
            $('#TxtDeckMaterial1').val("");
        }
    });
    $('#DeckMaterial2').change(function () {

        var selectedVal = $('#DeckMaterial2 option:selected').attr('value');

        if (selectedVal == "Other") {
            $('#TxtDeckMaterial2').prop('disabled', false);
            if ($('#TxtDeckMaterial2').val() != "")
                $('#DeckMaterial2').val("Other");
        }
        else {
            $('#TxtDeckMaterial2').prop('disabled', true);
            $('#TxtDeckMaterial2').val("");
        }
    });
    $('#DeckMaterial3').change(function () {

        var selectedVal = $('#DeckMaterial3 option:selected').attr('value');

        if (selectedVal == "Other") {
            $('#TxtDeckMaterial3').prop('disabled', false);
            if ($('#TxtDeckMaterial3').val() != "")
                $('#DeckMaterial3').val("Other");
        }
        else {
            $('#TxtDeckMaterial3').prop('disabled', true);
            $('#TxtDeckMaterial3').val("");
        }
    });

    $('#BearingsType1').change(function () {

        var selectedVal = $('#BearingsType1 option:selected').attr('value');

        if (selectedVal == "Other") {
            $('#TxtBearingsType1').prop('disabled', false);
            if ($('#TxtBearingsType1').val() != "")
                $('#BearingsType1').val("Other");
        }
        else {
            $('#TxtBearingsType1').prop('disabled', true);
            $('#TxtBearingsType1').val("");
        }
    });
    $('#BearingsType2').change(function () {

        var selectedVal = $('#BearingsType2 option:selected').attr('value');

        if (selectedVal == "Other") {
            $('#TxtBearingsType2').prop('disabled', false);
            if ($('#TxtBearingsType2').val() != "")
                $('#BearingsType2').val("Other");
        }
        else {
            $('#TxtBearingsType2').prop('disabled', true);
            $('#TxtBearingsType2').val("");
        }
    });
    $('#BearingsType3').change(function () {

        var selectedVal = $('#BearingsType3 option:selected').attr('value');

        if (selectedVal == "Other") {
            $('#TxtBearingsType3').prop('disabled', false);
            if ($('#TxtBearingsType3').val() != "")
                $('#BearingsType3').val("Other");
        }
        else {
            $('#TxtBearingsType3').prop('disabled', true);
            $('#TxtBearingsType3').val("");
        }
    });

    $('#FoundationType1').change(function () {

        var selectedVal = $('#FoundationType1 option:selected').attr('value');

        if (selectedVal == "Other") {
            $('#TxtFoundationType1').prop('disabled', false);
            if ($('#TxtFoundationType1').val() != "")
                $('#FoundationType1').val("Other");
        }
        else {
            $('#TxtFoundationType1').prop('disabled', true);
            $('#TxtFoundationType1').val("");
        }
    });
    $('#FoundationType2').change(function () {

        var selectedVal = $('#FoundationType2 option:selected').attr('value');

        if (selectedVal == "Other") {
            $('#TxtFoundationType2').prop('disabled', false);
            if ($('#TxtFoundationType2').val() != "")
                $('#FoundationType2').val("Other");
        }
        else {
            $('#TxtFoundationType2').prop('disabled', true);
            $('#TxtFoundationType2').val("");
        }
    });
    $('#FoundationType3').change(function () {

        var selectedVal = $('#FoundationType3 option:selected').attr('value');

        if (selectedVal == "Other") {

            $('#TxtFoundationType3').prop('disabled', false);
            if ($('#TxtFoundationType3').val() != "")
                $('#FoundationType3').val("Other");
        }

        else {

            $('#TxtFoundationType3').prop('disabled', true);

            $('#TxtFoundationType3').val("");
        }
    });
    function validateSkewAngle() {
        
        var skewAngleValid = true;
        if ($("#SkewAngle").val() != '') {
            if (!numberValidPattern.test($("#SkewAngle").val())) {

                $("#skewAngleValidate").show();
                skewAngleValid = false;
            }
            else {
                $("#skewAngleValidate").hide();
                skewAngleValid = true;
            }
        }
        return skewAngleValid;
    }
    function validateLength() {
        var lengthValid = true;
        if ($("#Length").val() != '') {
            if (!numberValidPattern.test($("#Length").val())) {

                $("#lengthValidate").show();
                lengthValid = false;
            }
            else {
                $("#lengthValidate").hide();
                lengthValid = true;
            }
        }
        return lengthValid;
    }
    function validateMaxLength() {
        var maxSpanValid = true;
        if ($("#MaxLength").val() != '') {
            if (!numberValidPattern.test($("#MaxLength").val())) {

                $("#maxSpanValidate").show();
                maxSpanValid = false;
            }
            else {
                $("#maxSpanValidate").hide();
                maxSpanValid = true;
            }
        }
        return maxSpanValid;
    }
    function validateNumSpan() {
        var numSpanValid = true;
        if ($("#SpansCount").val() != '') {
            if (!numberValidPattern.test($("#SpansCount").val())) {

                $("#numSpanValidate").show();
                numSpanValid = false;
            }
            else {
                $("#numSpanValidate").hide();
                numSpanValid = true;
            }
        }
        return numSpanValid;
    }
    function validateNumDecks() {
        var numDeckValid = true;
        if ($("#DecksCount").val() != '') {
            if (!numberValidPattern.test($("#DecksCount").val())) {

                $("#numDeckValidate").show();
                numDeckValid = false;
            }
            else {
                $("#numDeckValidate").hide();
                numDeckValid = true;
            }
        }
        return numDeckValid;
    }
    function validateCarriage() {
        var carriageValid = true;
        if ($("#CarrigeWayWidth").val() != '') {
            if (!numberValidPattern.test($("#CarrigeWayWidth").val())) {

                $("#carriageValidate").show();
                carriageValid = false;
            }
            else {
                $("#carriageValidate").hide();
                carriageValid = true;
            }
        }
        return carriageValid;
    }
    function validateDeckWidth() {
        var deckWidthValid = true;
        if ($("#DeckWidth").val() != '') {
            if (!numberValidPattern.test($("#DeckWidth").val())) {

                $("#deckWidthValidate").show();
                deckWidthValid = false;
            }
            else {
                $("#deckWidthValidate").hide();
                deckWidthValid = true;
            }
        }
        return deckWidthValid;
    }
    function deleteStructureSpan(e) {
        var position = e.currentTarget.dataset.Position;
        deleteSpan(position);
    }
    function editStructureDimension(e) {
        var position = e.currentTarget.dataset.Position;
        editDimension(position);
    }
