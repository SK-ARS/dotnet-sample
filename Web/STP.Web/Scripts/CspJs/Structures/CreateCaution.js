    function copyContent() {
        
        $('#Description').val("");
        var text = $('#editor1').html();
        if (text.includes("&nbsp;")) {
            text = text.replaceAll("&nbsp;"," ")
        }
        $('#Description').val(text);
        $('#Description').text(text);
        $('#Description').value = text;
    }
    function CheckBold() {
        var text = $('#Description').val();
        var checkBold = (text.includes("</b>") || text.includes("font-weight: bold"));
        if (checkBold) {
            $('#chkBold').val('true');
        }
        else {

            $('#chkBold').val('false');
        }        
    }
    function CheckItalic() {
        var text = $('#Description').val();
        var checkItalic = (text.includes("</i>") || text.includes("text-decoration-line: underline"));
        if (checkItalic) {
            $('#chkItalic').val('true');
        }
        else {
            $('#chkItalic').val('false');
        }       

    }
    function CheckUnderline() {
        var text = $('#Description').val();
        var checkUnderline = text.includes("</u>");
        if (checkUnderline) {
            $('#chkUnderline').val('true');
        }
        else {
            $('#chkUnderline').val('false');
        }        
    }
    function BackToListCausion() {
        startAnimation();
        var randomNumber = Math.random();
        $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + '@ViewBag.StructureId' + "&sectionId=" + @ViewBag.sectionId + '&random=' + randomNumber,
            function () {
                stopAnimation();
                document.getElementById('manageCautionId').style.display = 'block';
            }
        );
    };
    $(document).ready(function () {
       
        $("#editor1").keyup('click', copyContent);
        $("#div-validspeedcaution").keyup('click', validateSpeedCaution);
        $("#div-validaxlecaution").keyup('click', validateAxleCaution);
        $("#div-validgrossweight").keyup('click', validateGrossCaution);
        $("#div-validwidth").keyup('click', validateWidthCaution );
        $("#validname").keyup('click', validateName);
        $("#div-validheight").keyup('click', validateHeight);
        $(".addedcausion").on('click', addingCausion);
        $(".opnhstory").on('click', openHistories);
        $("#divcopy").on('click', copyContent);
        $("#SaveCaution").on('click', CautionAddReport);
        $("#btn_back").on('click',BackToListCausion);
        if ("@ViewBag.mode" == "add") {
            $('#chkBold').val('false');
            $('#chkItalic').val('false');
            $('#chkUnderline').val('false');
        }
        else {
            var text = $('#Description').val();
            $('#editor1').html(text);
            $('#Description').text(text);
            $('#Description').value = text;
        }
        
        StanderCautionRadioOnLoad();
        $("input[name=SelectedType]:radio").change(function () {
            if ($(this).val() != "StandardCaution") {
                $('#chkBold').attr('disabled', false);
                $('#chkItalic').attr('disabled', false);
                $('#chkUnderline').attr('disabled', false);
                $('#editor1').attr('contenteditable', true);
                $('#editor1').removeClass('selectColor');
                $('#linkBold').attr('disabled', false);
                $('#linkItalic').attr('disabled', false);
                $('#linkUnderline').attr('disabled', false);
            }
            else {
                $('#chkBold').val('false');
                $('#chkItalic').val('false');
                $('#chkUnderline').val('false');
                $('#chkBold').attr('checked', false);
                $('#chkItalic').attr('checked', false);
                $('#chkUnderline').attr('checked', false);
                $('#chkBold').attr('disabled', true);
                $('#chkItalic').attr('disabled', true);
                $('#chkUnderline').attr('disabled', true);
                $('#Description').val('');
                $('#editor1').html('');
                $('#editor1').attr('contenteditable', false);
                $('#editor1').addClass('selectColor');
                $('#linkBold').attr('disabled', true);
                $('#linkItalic').attr('disabled', true);
                $('#linkUnderline').attr('disabled', true);
            }
        });
    });
    function addingCausion(e) {
        var arg1 = e.currentTarget.dataset.arg1;

        addCausion(arg1);
    }
    function openHistories(e) {
        var arg1 = e.currentTarget.dataset.arg1;

        openHistory(arg1);
    }



    function StanderCautionRadioOnLoad() {
        if ($("#hdnStandardCaution").val() != 'StandardCaution') {
            $('#chkBold').attr('disabled', false);
            $('#chkItalic').attr('disabled', false);
            $('#chkUnderline').attr('disabled', false);
            $('#editor1').attr('contenteditable', true);
            $('#editor1').removeClass('selectColor');
        }
        else {
            $('#chkBold').attr('disabled', true);
            $('#chkItalic').attr('disabled', true);
            $('#chkUnderline').attr('disabled', true);
            $('#Description').val('');
            $('#editor1').html('');
            $('#editor1').attr('contenteditable', false);
            $('#editor1').addClass('selectColor');
            $('#linkBold').attr('disabled', true);
            $('#linkItalic').attr('disabled', true);
            $('#linkUnderline').attr('disabled', true);
        }
    }
    function validateDescription() {
        var descValid = true;
        if ($('#editor1').text().length != 0) {
            var DescText = $('#editor1').text();
            var SpecialChar1 = DescText.search("&");
            var SpecialChar2 = DescText.search("<");
            var SpecialChar3 = DescText.search(">");
            if (SpecialChar1 != -1 || SpecialChar2 != -1 || SpecialChar3 != -1) { // checking the text contains any special character in(& < >)
                $("#descValidate").show();
                descValid = false;

            }
            else {
                $("#descValidate").hide();
                descValid = true;
            }
        }
        return descValid;
    }
    function validateName() {
        var nameValid = true;
        if ($('#CautionName').val().length == 0) {
            $("#nameReqValidate").show();
            nameValid = false;
        }
        else {
            $("#nameReqValidate").hide();
            nameValid = true;
        }
        return nameValid;
    }
    function validateHeight() {
        var UOM = $('#UOM').val();
        var heightValid = true;
        if ($('#Height').val().length != 0) {
            if (UOM == 692001) {
                if (isNaN($('#Height').val()) || $('#Height').val() < 0 || isInValidMTRS($('#Height').val())) { //RM#3969 change for height validation
                    $("#heightValidate").show();
                    heightValid = false;
                }
            }
            else {
                if (feetinches($('#Height').val()) == false) {
                    $('#heightValidate').show();
                    heightValid = false;
                }
                else {
                    $('#heightValidate').hide();
                    heightValid = true;
                }
            }
        }
        else {
            $('#heightValidate').hide();
            heightValid = true;
        }
        return heightValid;
    }
    function validateWidthCaution() {
        var UOM = $('#UOM').val();
        var widthValid = true;
        if ($('#Width').val().length != 0) {

            if (UOM == 692001) {
                if (isNaN($('#Width').val()) || $('#Width').val() < 0 || isInValidMTRS($('#Width').val())) { //RM#3969 change for Width validation
                    $('#widthValidate').show();
                    widthValid = false;
                }
            }
            else {
                if (feetinches($('#Width').val()) == false) {
                    $('#widthValidate').show();
                    widthValid = false;
                }
                else {
                    $('#widthValidate').hide();
                    widthValid = true;
                }
            }
        }
        else {
            $('#widthValidate').hide();
            widthValid = true;
        }
        return widthValid;
    }
    function validateLengthCaution() {
        var UOM = $('#UOM').val();
        var lengthValid = true;
        if ($('#Length').val().length != 0) {
            if (UOM == 692001) {
                if (isNaN($('#Length').val()) || $('#Length').val() < 0 || isInValidMTRS($('#Length').val())) { // RM#3969 change for Length validation
                    $('#lengthValidate').show();
                    lengthValid = false;
                }
            }
            else {
                if (feetinches($('#Length').val()) == false) {
                    $('#lengthValidate').show();
                    lengthValid = false;
                }
                else {
                    $('#lengthValidate').hide();
                    lengthValid = true;
                }
            }
        }
        else {
            $('#lengthValidate').hide();
            lengthValid = true;
        }
        return lengthValid;
    }
    function validateGrossCaution() {
        var grossWeightValid = true;
        if ($('#MaxGrossWeightKgs').val().length != 0) {
            $('#grossWeightReqValidate').hide();
            if (isNaN($('#MaxGrossWeightKgs').val()) || $('#MaxGrossWeightKgs').val() < 0 || IsValidDecimal($('#MaxGrossWeightKgs').val()) == false || isInValidTonnes($('#MaxGrossWeightKgs').val())) { //MaxGrossWeightKgs validation.
                $('#grossWeightValidate').show();
                $('#grossWeightReqValidate').hide();
                grossWeightValid = false;
            }
            else {
                $('#grossWeightValidate').hide();
                grossWeightValid = true;
            }
        }
        else {
            $('#grossWeightReqValidate').show();
            $('#grossWeightValidate').hide();
            grossWeightValid = false;
        }
        return grossWeightValid;
    }
    function validateAxleCaution() {
        var axleWeightValid = true;
        if ($('#MaxAxleWeightKgs').val().length != 0) {
            $('#axleWeightReqValidate').hide();
            if (isNaN($('#MaxAxleWeightKgs').val()) || $('#MaxAxleWeightKgs').val() < 0 || IsValidDecimal($('#MaxAxleWeightKgs').val()) == false || isInValidTonnes($('#MaxAxleWeightKgs').val())) { //MaxAxleWeightKgs validation.
                $('#axleWeightValidate').show();
                $('#axleWeightReqValidate').hide();
                axleWeightValid = false;
            }
            else {
                $('#axleWeightValidate').hide();
                axleWeightValid = true;
            }
        }
        else {
            $('#axleWeightReqValidate').show();
            $('#axleWeightValidate').hide();
            axleWeightValid = false;
        }
        return axleWeightValid;
    }
    function validateSpeedCaution() {
        var UOM = $('#UOM').val();
        var speedValid = true;
        if ($('#Speed').val().length != 0) {
            $('#speedReqValidate').hide();
            if (UOM == 692001) { //RM#3969 change for Speed max length validation.
                if (isNaN($('#Speed').val()) || $('#Speed').val() < 0 || isInValidKPH($('#Speed').val())) {
                    $('#speedValidate').show();
                    $('#speedReqValidate').hide();
                    speedValid = false;
                }
                else {
                    $('#speedValidate').hide();
                    speedValid = true;
                }
            }
            else {
                if (isNaN($('#Speed').val()) || $('#Speed').val() < 0 || isInValidMPH($('#Speed').val())) {
                    $('#speedValidate').show();
                    $('#speedReqValidate').hide();
                    speedValid = false;
                }
                else {
                    $('#speedValidate').hide();
                    speedValid = true;
                }
            }
        }
        else {
            $('#speedReqValidate').show();
            $('#speedValidate').hide();
            speedValid = false;
        }
        return speedValid;
    }
    function CautionAddReport() {

        //check validation       
        CheckBold();
        CheckItalic();
        CheckUnderline();
        var descValid = validateDescription();
        var nameValid = validateName();
        var heightValid = validateHeight();
        var widthValid = validateWidthCaution();
        var lengthValid = validateLengthCaution();
        var grossValid = validateGrossCaution();
        var axleValid = validateAxleCaution();
        var speedValid = validateSpeedCaution();
        if (descValid && nameValid && heightValid && widthValid && lengthValid && grossValid &&
            axleValid && speedValid) {            
            $("#SelectedTypeName").val($("input[name=SelectedType]:checked").val());
            var data = $("#CautionInfo").serialize();
            $.ajax({
                url: '../Structures/StoreCautionData',
                dataType: 'json',
                type: 'POST',
                data: data,
                success: function (result) {
                    if (result == true) {
                        CautionShowReport();
                    }
                    else if (result == 'sessionnull') {
                        redirectCaution();
                    }
                },
                error: function (xhr, status) {
                }
            });

        }

    }
    function CautionShowReport() {
        startAnimation();
        var randomNumber = Math.random();
        $('#generalSettingsId').hide();
        $("#causionReport").load('../Structures/CautionAddReport?StructureID=' + @ViewBag.StructureID + '&SectionID=' + @ViewBag.SectionID + '&random=' + randomNumber,
            function () {
                stopAnimation();
            }
        );
    }

        function isNonAlphaNum(evt) { // Validation For Special Characters & < >
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var charStr = String.fromCharCode(charCode);
            if (charStr == "&" || charStr == "<" || charStr == ">") {
                // alert(charStr + " is not Allowed ");
                $('#errdesc').show();
                return false;
            }
            else {
                $('#errdesc').hide();
                return true;
            }
        }
    function redirectCaution() {
        window.location.href = "@Url.Action("Login", "Account")";
    }
        function IsValidDecimal(value) {

            var regex = /^\d+(\.\d{1,2})?$/;
            if (regex.test(value)) {
                return true;
            } else {
                return false;
            }
        }
        function IsValidNumber(value) {
            if (value == parseInt(value)) {
                return true;
            }
            else {
                return false;
            }
        }
    function feetinches(value) {
        var rex = /^(\d+)'(\d+)(?:''|")$/;
        
        var match = rex.exec(value);
            var flag = true;
            var feet, inch;
            if (match) {
                feet = parseInt(match[1], 10);
                inch = parseInt(match[2], 10);
                if ((feet * 12) + inch > 393700) { //RM#3969 change for feet inches validation.
                    flag = false;
                }
            }
            if (match) {
                return flag;
            }
            else
                return false;
        }

        function isInValidMTRS(value) { //RM#3969 added new function
            if (!isNaN(value)) {
                if (parseFloat(value) <= 9999.99) {
                    return false;
                }
            }
            return true;
        }


        function isInValidKPH(value) { //RM#3969 added new function
            if (!isNaN(value)) {
                if (parseFloat(value) <= 160.94) {
                    return false;
                }
            }
            return true;
        }

        function isInValidMPH(value) { //RM#3969 added new function
            if (!isNaN(value)) {
                if (parseFloat(value) <= 99.99) {
                    return false;
                }
            }
            return true;
        }

        function isInValidTonnes(value) { //RM#3969 added new function
            if (!isNaN(value)) {
                if (parseFloat(value) <= 99999.99) {
                    return false;
                }
            }
            return true;
        }

