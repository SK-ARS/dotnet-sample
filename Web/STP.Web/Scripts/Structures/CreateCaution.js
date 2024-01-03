var hf_StructureId = $('#CreateCautionContainer #hf_StructureId').val();
var hf_SectionId = $('#CreateCautionContainer #hf_SectionId').val();
var hf_mode = $('#CreateCautionContainer #hf_mode').val();
function CreateCautionInit() {
  
    hf_StructureId = $('#CreateCautionContainer #hf_StructureId').val();
    hf_SectionId = $('#CreateCautionContainer #hf_SectionId').val();
    
   
    hf_mode = $('#CreateCautionContainer #hf_mode').val();
    if (hf_mode == "add") {
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

    StanderCautionRadioOnLoadStructure();
}

$(document).ready(function () {

    $('body').on('keyup', '#CreateCautionContainer #editor1', function (e) {
        copyContentStructure();
    });

    $('body').on('keyup', '#CreateCautionContainer #div-validspeedcaution', function (e) {
        validateSpeedCautionStructure();
    });

    $('body').on('keyup', '#CreateCautionContainer #div-validaxlecaution', function (e) {
        validateAxleCautionStructure();
    });

    $('body').on('keyup', '#CreateCautionContainer #div-validgrossweight', function (e) {
        validateGrossCautionStructure();
    });
    $('body').on('keyup', '#CreateCautionContainer #div-validwidth', function (e) {
        validateWidthCautionStructure();
    });
    $('body').on('keyup', '#CreateCautionContainer #div-validlength', function (e) {
        validateLengthCautionStructure();
    });
    $('body').on('keyup', '#CreateCautionContainer #validname', function (e) {
        validateNameStructure();
    });
    $('body').on('keyup', '#CreateCautionContainer #div-validheight', function (e) {
        validateHeightStructure();
    });
    $('body').on('click', '#CreateCautionContainer .addedcausion', function (e) {
        e.preventDefault();
        addingCausionStructure(this);
    });
    $('body').on('click', '#CreateCautionContainer .opnhstory', function (e) {
        e.preventDefault();
        openHistoriesStructure(this);
    });
    $('body').on('click', '#CreateCautionContainer #divcopy', function (e) {
        e.preventDefault();
        copyContentStructure(this);
    });
    $('body').on('click', '#CreateCautionContainer #SaveCaution', function (e) {
        
        e.preventDefault();
        CautionAddReportStructure(this);
        
    });
    $('body').on('click', '#CreateCautionContainer #btn_back', function (e) {
        e.preventDefault();
        BackToListCausionStructure(this);
        $('html, body').animate({
            scrollTop: ($('.titleSOAStructrue').position().top)
        }, 500);
    });
    $('body').on('click', '.bold', function (e) {
        var type = $(this).data("font");
        $(this).toggleClass('active');
        document.execCommand(type, false, null);
    });
    $('body').on('click', '.underline', function (e) {
        var type = $(this).data("font");
        $(this).toggleClass('active');
        document.execCommand(type, false, null);
    });
    $('body').on('click', '.italic', function (e) {
        var type = $(this).data("font");
        $(this).toggleClass('active');
        document.execCommand(type, false, null);
    });

    $('body').on('change', '#CreateCautionContainer input[name=SelectedType]:radio', function (e) {
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

function copyContentStructure() {
    $('#Description').val("");
    var text = $('#editor1').html();
    if (text.includes("&nbsp;")) {
        text = text.replaceAll("&nbsp;", " ")
    }
    $('#Description').val(text);
    $('#Description').text(text);
    $('#Description').value = text;
}
function CheckBoldStructure() {
    var text = $('#Description').val();
    var checkBold = (text.includes("</b>") || text.includes("font-weight: bold"));
    if (checkBold) {
        $('#chkBold').val('true');
    }
    else {

        $('#chkBold').val('false');
    }
}
function CheckItalicStructure() {
    var text = $('#Description').val();
    var checkItalic = (text.includes("</i>") || text.includes("text-decoration-line: underline"));
    if (checkItalic) {
        $('#chkItalic').val('true');
    }
    else {
        $('#chkItalic').val('false');
    }

}
function CheckUnderlineStructure() {
    var text = $('#Description').val();
    var checkUnderline = text.includes("</u>");
    if (checkUnderline) {
        $('#chkUnderline').val('true');
    }
    else {
        $('#chkUnderline').val('false');
    }
}
function BackToListCausionStructure() {
    startAnimation();
    var randomNumber = Math.random();
    $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + hf_StructureId + "&sectionId=" + hf_SectionId + '&random=' + randomNumber,
        function () {
            stopAnimation();
            $('#manageCautionId').css("display",'block');
            ReviewCautionsListInit();
        }
    );
};
function addingCausionStructure(e) {
    var arg1 = $(e).attr("arg1");
    addCausion(arg1);
}
function openHistoriesStructure(e) {
    var arg1 = $(e).attr("arg1");
    openHistory(arg1);
}

function StanderCautionRadioOnLoadStructure() {
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
function validateDescriptionStructure() {
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
function validateNameStructure() {
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
function validateHeightStructure() {
    var UOM = $('#UOM').val();
    var heightValid = true;
    if ($('#Height').val().length != 0) {
        if (UOM == 692001) {
            if (isNaN($('#Height').val()) || $('#Height').val() < 0 || isInValidMTRSStructure($('#Height').val())) { //RM#3969 change for height validation
                $("#heightValidate").show();
                heightValid = false;
            }
        }
        else {
            if (feetinchesStructure($('#Height').val()) == false) {
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
function validateWidthCautionStructure() {
    var UOM = $('#UOM').val();
    var widthValid = true;
    if ($('#Width').val().length != 0) {

        if (UOM == 692001) {
            if (isNaN($('#Width').val()) || $('#Width').val() < 0 || isInValidMTRSStructure($('#Width').val())) { //RM#3969 change for Width validation
                $('#widthValidate').show();
                widthValid = false;
            }
        }
        else {
            if (feetinchesStructure($('#Width').val()) == false) {
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
function validateLengthCautionStructure() {
    $('#lengthValidate').hide();
    var UOM = $('#UOM').val();
    var lengthValid = true;
    if ($('#Length').val().length != 0) {
        if (UOM == 692001) {
            if (isNaN($('#Length').val()) || $('#Length').val() < 0 || isInValidMTRSStructure($('#Length').val())) { // RM#3969 change for Length validation
                $('#lengthValidate').show();
                lengthValid = false;
            }
        }
        else {
            if (feetinchesStructure($('#Length').val()) == false) {
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
function validateGrossCautionStructure() {
    var grossWeightValid = true;
    if ($('#GrossWeight').val().length != 0) {
        /*$('#grossWeightReqValidate').hide();*/
        if (isNaN($('#GrossWeight').val()) || $('#GrossWeight').val() < 0 || IsValidDecimalStructure($('#GrossWeight').val()) == false || (UOM == 692002 && isInValidTonnesStructure($('#GrossWeight').val()))) { //GrossWeight validation.
            $('#grossWeightValidate').show();
            /* $('#grossWeightReqValidate').hide();*/
            grossWeightValid = false;
        }
        else {
            $('#grossWeightValidate').hide();
            grossWeightValid = true;
        }
    }
    else {
       /* $('#grossWeightReqValidate').show();*/
        $('#grossWeightValidate').hide();
        grossWeightValid = true;
    }
    return grossWeightValid;
}
function validateAxleCautionStructure() {
    var axleWeightValid = true;
    if ($('#AxleWeight').val().length != 0) {
        $('#axleWeightReqValidate').hide();
        if (isNaN($('#AxleWeight').val()) || $('#AxleWeight').val() < 0 || IsValidDecimalStructure($('#AxleWeight').val()) == false || (UOM == 692002 && isInValidTonnesStructure($('#AxleWeight').val()))) { //AxleWeight validation.
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
       /* $('#axleWeightReqValidate').show();*/
        $('#axleWeightValidate').hide();
        axleWeightValid = true;
    }
    return axleWeightValid;
}
function validateSpeedCautionStructure() {
    var UOM = $('#UOM').val();
    var speedValid = true;
    if ($('#Speed').val().length != 0) {
        $('#speedReqValidate').hide();
        if (UOM == 692001) { //RM#3969 change for Speed max length validation.
            if (isNaN($('#Speed').val()) || $('#Speed').val() < 0 || isInValidKPHStructure($('#Speed').val())) {
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
            if (isNaN($('#Speed').val()) || $('#Speed').val() < 0 || isInValidMPHStructure($('#Speed').val())) {
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
        //$('#speedReqValidate').show();
        $('#speedValidate').hide();
        speedValid = true;
    }
    return speedValid;
}
function CautionAddReportStructure() {
    //check validation       
    CheckBoldStructure();
    CheckItalicStructure();
    CheckUnderlineStructure();
    var descValid = validateDescriptionStructure();
    var nameValid = validateNameStructure();
    var heightValid = validateHeightStructure();
    var widthValid = validateWidthCautionStructure();
    var lengthValid = validateLengthCautionStructure();
    var grossValid = validateGrossCautionStructure();
    var axleValid = validateAxleCautionStructure();
    var speedValid = validateSpeedCautionStructure();
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
                    CautionShowReportStructure();
                }
                else if (result == 'sessionnull') {
                    redirectCautionStructure();
                }
                $('html, body').animate({
                    scrollTop: ($('#causionReport').position().top)
                }, 1000);
            },
            error: function (xhr, status) {
            }
        });
    } else {
        $('html, body').animate({
            scrollTop: ($('#CautionAddReportContainer').position().top)
        }, 1000);
    }
}
function CautionShowReportStructure() {
    startAnimation();
    var randomNumber = Math.random();
    $('#generalSettingsId').hide();
    $("#causionReport").load('../Structures/CautionAddReport?StructureID=' + hf_StructureId + '&SectionID=' + hf_SectionId + '&random=' + randomNumber,
        function () {
            stopAnimation();
            CautionAddReportInit();
        }
    );
}

function isNonAlphaNumStructure(evt) { // Validation For Special Characters & < >
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
function redirectCautionStructure() {
    window.location.href = "../Account/Login";
}
function IsValidDecimalStructure(value) {

    var regex = /^\d+(\.\d{1,2})?$/;
    if (regex.test(value)) {
        return true;
    } else {
        return false;
    }
}
function IsValidNumberStructure(value) {
    if (value == parseInt(value)) {
        return true;
    }
    else {
        return false;
    }
}
function feetinchesStructure(value) {
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

function isInValidMTRSStructure(value) { //RM#3969 added new function
    if (!isNaN(value)) {
        if (parseFloat(value) <= 9999.99) {
            return false;
        }
    }
    return true;
}

function isInValidKPHStructure(value) { //RM#3969 added new function
    if (!isNaN(value)) {
        if (parseFloat(value) <= 160.94) {
            return false;
        }
    }
    return true;
}

function isInValidMPHStructure(value) { //RM#3969 added new function
    if (!isNaN(value)) {
        if (parseFloat(value) <= 99.99) {
            return false;
        }
    }
    return true;
}

function isInValidTonnesStructure(value) { //RM#3969 added new function
    if (!isNaN(value)) {
        if (parseFloat(value) <= 99999.99) {
            return false;
        }
    }
    return true;
}

