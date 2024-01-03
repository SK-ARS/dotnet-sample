var numberValidation = new RegExp(/^[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)$/);
$(document).ready(function () {
    $("#dialogue").show();
    $("#overlay").css("background", "rgba(0, 0, 0, 0.0)");
    StanderCautionRadioOnLoad();

    //$('#Description').attr('disabled', true);//#2313
    $('#FirstCautionFlag').val($("#hfFirstCautionFlag").val());

    $('body').on('click', '.create-caution .btn-open-nav', function () {
        OpenNavCreateCaution();
    });

    $('body').on('click', '.create-caution .btn-close-nav', function () {
        CloseNavCreateCaution();
    });

    $('body').on('click', '.create-caution .btn-add-caution-report', function () {
        CautionAddReport();
    });
    $('body').on('focus', '.create-caution #CautionName', function () {
        $('#spanCautionName').hide();
    });

    $('body').on('click', '.create-caution .btn-close-create-caution', function () {
        closeCreateCaution();
    });

    $('body').on('click', '.create-caution .fontStyle', function () {
        var type = $(this).data('type');
        $(this).toggleClass('active')
        document.execCommand(type, false, null);
    });

    $('body').on('change', '.create-caution input[name=SelectedType]:radio', function () {
        if ($(this).val() != "StandardCaution") {//#2313
            $('#chkBold').attr('disabled', false);
            $('#chkItalic').attr('disabled', false);
            $('#chkUnderline').attr('disabled', false);
            $('#Description').attr('disabled', false);//#2313


            $('#Description').attr('contenteditable', true);
            $('#Description').css('background', 'white');
            $('#Description').css('overflow', 'auto');
            $('.message-text').removeClass('div-disabled');

        }
        else {
            $('#chkBold').attr('checked', false);
            $('#chkItalic').attr('checked', false);
            $('#chkUnderline').attr('checked', false);
            $('#chkBold').attr('disabled', true);
            $('#chkItalic').attr('disabled', true);
            $('#chkUnderline').attr('disabled', true);
            $('#Description')[0].innerHTML = "";//#2313
            //$('#Description').attr('disabled', true);//#2313
            $('#Description').attr('contenteditable', false);
            $('#Description').css('background', 'lightgray');
            $('.message-text').addClass('div-disabled');
        }
    });

    $('body').on('keyup', '.create-caution .edit-normal', function () {
        $(this).closest('.row').find('.error').hide();
    });
});

function CreateCautionInit() {
    StanderCautionRadioOnLoad();
}


function CloseNavCreateCaution() {

    $("#add-caution").css("width","0px");
    $("#add-caution").css("display","none");

    $("#card-swipecon1").css("display","inline-block");
    $("#card-swipecon2").css("display","none");

    //document.getElementById("MakingcontraintsInfo").style.width = "0";
    //document.getElementById("createConsDiv").style.width = "0";
    //document.getElementById("card-swipecon1").style.display = "none";
    //document.getElementById("card-swipecon2").style.display = "block";

}

function OpenNavCreateCaution() {

    $("#add-caution").css("width","345px");
    $("#add-caution").css("display","block");

    $("#card-swipecon1").css("display","none");
    $("#card-swipecon2").css("display","inline-block");
    $("#card-swipecon2").css("margin-left","415px");

    //document.getElementById("createConsDiv").style.width = "345px";
    //document.getElementById("card-swipecon1").style.display = "block"
    //document.getElementById("card-swipecon2").style.display = "block"
    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("mySidenav1").style.width = "auto";
        }
    }
    var x = window.matchMedia("(max-width: 410px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
    //function myFunction(x) {
    //    if (x.matches) { // If media query matches
    //        document.getElementById("mySidenav1").style.width = "auto";
    //    }
    //}
    //var x = window.matchMedia("(max-width: 410px)")
    //myFunction(x) // Call listener function at run time
    //x.addListener(myFunction)
}

function closeCreateCaution() {
    var randomNumber = Math.random();
    $(".modal-backdrop").css("height", "0px");
    OpenDialogueCreateCaution('../Constraint/ReviewCautionsList?constraintId=' + $('#CONSTRAINT_ID').val() + '&random=' + randomNumber
        , "review-caution-list");
}
function OpenDialogueCreateCaution(url, container) {
    startAnimation();
    $.ajax({
        async: false,
        type: "GET",
        url: url,
        processdata: true,
        success: function (response) {
            if (container != '' && container != undefined) {
                $("#dialogue").html($(response).closest('.' + container)[0]);
            } else {
                $("#dialogue").html(response);
            }
            stopAnimation();
            $("#dialogue").show();
            $("#overlay").show();
            //$("#overlay").css('background', 'none');
        },
        error: function (result) {
        }
    });
}
$('body').on('click', '.btn-back-addcausion', function () {
    closeCautionSpanReviewCaut();
});
function closeCautionSpanReviewCaut() {
    $("#dialogue").show();
    //startAnimation();
    var randomNumber = Math.random();
    //$(".modal-backdrop").css("height", "0px");

    //var ViewConstraintsdetails = $("#ViewConstraintsdetails").val();
    var flageditmode = $('#flageditmode').val();
    $("#dialogue").load('../Constraint/ViewConstraint?ConstraintID=' + $('#ConstraintID').val() + '&flageditmode=' + flageditmode + '&random=' + randomNumber, function () {
        ViewConstraintInit();
    });
}
function onSave() {
    // if (document.getElementById('check-caution').style.display !== "none") {
    document.getElementById('check-caution').style.display = "block"
    document.getElementById('add-caution').style.display = "none"
}

function StanderCautionRadioOnLoad() {
    if ($("#hdnStandardCaution").val() != 'StandardCaution') {//#2313
        $('#chkBold').attr('disabled', false);
        $('#chkItalic').attr('disabled', false);
        $('#chkUnderline').attr('disabled', false);
        //$('#Description').attr('disabled', false);//#2313
        $('.message-text').removeClass('div-disabled');

    }
    else {
        $('#chkBold').attr('disabled', true);
        $('#chkItalic').attr('disabled', true);
        $('#chkUnderline').attr('disabled', true);
        //$('#Description').attr('disabled', true);//#2313
        $('.message-text').addClass('div-disabled');


    }
}

function valiadteCaution() {
    var zeroPrecisionRegex = /(\.0{0,20})(0+)$/;
    //check validation
    var flageCheck = true;
    var isHeightValid = false;
    var isWidthValid = false;
    var isLengthValid = false;
    var isSpeedValid = false;
    var isGrossWeightValid = false;
    var isAxleWeightValid = false;
    var isNameValid = false;
    var isDateValid = false;
    var UOM = $('#UOM').val();
    $('#spMAX_HEIGHT_MTRS').hide();
    $('#spMAX_WIDTH_MTRS').hide();
    $('#spMAX_LENGTH_MTRS').hide();
    $('#spMAX_GROSS_WEIGHT_KGS').hide();
    $('#spMAX_AXLE_WEIGHT_KGS').hide();
    $('#spMIN_SPEED_KPH').hide();

    var cautionName = $('#CautionInfo #CautionName').val();
    if (cautionName == "") {
        $('#spanCautionName').html('Please enter caution name');
        isNameValid = false;
    }
    else {
        $('#spanCautionName').html('');
        isNameValid = true;

    }
    //code added for checking validation on the field Specific Action when user do a copy/paste a text


    //var one = $('#Description').innerhtml();
    var dis = $("#Description").val();
    if ($('#Description')[0].innerHTML.length != 0) {
        var DescText = $('#Description')[0].innerHTML;
        var SpecialChar1 = DescText.search("&");
        var SpecialChar2 = DescText.search("<");
        var SpecialChar3 = DescText.search(">");
        if (SpecialChar1 != -1 || SpecialChar2 != -1 || SpecialChar3 != -1) { // checking the text contains any special character in(& < >)
            $('#errdesc').show();
            flageCheck = false;
        }

    }
    if ($('#Height').val().length != 0) {
        if (UOM == 692001) {
            var num = $('#Height').val().trim().replace(zeroPrecisionRegex, '');
            var height = $('#Height').val().split('.');
            if (!numberValidation.test($("#Height").val())) {
                $('#spMAX_HEIGHT_MTRS').html('Please enter valid height');
                isHeightValid = false;

            } else
                if (height[1] == "") {
                    $('#spMAX_HEIGHT_MTRS').html('Please enter valid height');
                    isHeightValid = false;

                }
               
                else
                    if (isNaN($('#Height').val()) || $('#Height').val() < 0 || CreateCautionValidateLength(num, 5)) {
                        $('#spMAX_HEIGHT_MTRS').html('The maximum allowed digit is 4');
                        isHeightValid = false;
                    }
                    else if (height[1] != undefined) {
                        if (height[1].length > 3) {
                            $('#spMAX_HEIGHT_MTRS').html('The maximum allowed decimal point is 3');
                            isHeightValid = false;
                        } else {
                            $('#spMAX_HEIGHT_MTRS').html('');
                            isHeightValid = true;
                        }
                    }
                    else {
                        $('#spMAX_HEIGHT_MTRS').html('');
                        isHeightValid = true;
                    }
        }
        else {
            if (feetinches1($('#Height').val()) == false) {
                $('#spMAX_HEIGHT_MTRS').html('The format should be ' + "ft'" + 'in"');

                $('#spMAX_HEIGHT_MTRS').show();
                isHeightValid = false;
            }
            else {
                $('#spMAX_HEIGHT_MTRS').html('');
                isHeightValid = true;
            }
        }
    }
    else {
        $('#spMAX_HEIGHT_MTRS').html('');
        isHeightValid = true;
    }

    if ($('#Width').val().length != 0) {
        var num = $('#Width').val().trim().replace(zeroPrecisionRegex, '');
        if (UOM == 692001) {
            var Width = $('#Width').val().split('.');
            if (!numberValidation.test($("#Width").val())) {
                $('#spMAX_WIDTH_MTRS').html('Please enter valid width');
                isWidthValid = false;
            } else
                if (Width[1] == "") {
                    $('#spMAX_WIDTH_MTRS').html('Please enter valid width');
                    isWidthValid = false;
                }
                
                else
                    if (isNaN(num) || num < 0 || CreateCautionValidateLength(num, 5)) {
                        $('#spMAX_WIDTH_MTRS').html('The maximum allowed digit is 4');
                        isWidthValid = false;
                    } else if (Width[1] != undefined) {
                        if (Width[1].length > 3) {
                            $('#spMAX_WIDTH_MTRS').html('The maximum allowed decimal point is 3');
                            isWidthValid = false;
                        } else {
                            $('#spMAX_WIDTH_MTRS').html('');
                            isWidthValid = true;
                        }
                    }
                    else {
                        $('#spMAX_WIDTH_MTRS').html('');
                        isWidthValid = true;
                    }
        }
        else {
            if (feetinches1($('#Width').val()) == false) {
                $('#spMAX_WIDTH_MTRS').html('The format should be ' + "ft'" + 'in"');
                $('#spMAX_WIDTH_MTRS').show();
                isWidthValid = false;
            } else {
                $('#spMAX_WIDTH_MTRS').html('');
                isWidthValid = true;
            }
        }
    }
    else {
        $('#spMAX_WIDTH_MTRS').html('');
        isWidthValid = true;
    }
    if ($('#Length').val().length != 0) {
        var num = $('#Length').val().trim().replace(zeroPrecisionRegex, '');
        if (UOM == 692001) {
            var Length = $('#Length').val().split('.');
            if (!numberValidation.test($("#Length").val())) {
                $('#spMAX_LENGTH_MTRS').html('Please enter valid length');
                isLengthValid = false;
            } else
                if (Length[1] == "") {
                    $('#spMAX_LENGTH_MTRS').html('Please enter valid length');
                    isLengthValid = false;
                }
                
                else
                    if (isNaN(num) || num < 0 || CreateCautionValidateLength(num, 5)) {
                        $('#spMAX_LENGTH_MTRS').html('The maximum allowed digit is 4');
                        isLengthValid = false;
                    } else if (Length[1] != undefined) {
                        if (Length[1].length > 3) {
                            $('#spMAX_LENGTH_MTRS').html('The maximum allowed decimal point is 3');
                            isLengthValid = false;
                        } else {
                            $('#spMAX_LENGTH_MTRS').html('');
                            isLengthValid = true;
                        }
                    }
                    else {
                        $('#spMAX_LENGTH_MTRS').html('');
                        isLengthValid = true;
                    }
        }
        else {
            if (feetinches1(num) == false) {
                $('#spMAX_LENGTH_MTRS').html('The format should be ' + "ft'" + 'in"');
                $('#spMAX_LENGTH_MTRS').show();
                isLengthValid = false;
            } else {
                $('#spMAX_LENGTH_MTRS').html('');
                isLengthValid = true;
            }
        }
    }
    else {
        $('#spMAX_LENGTH_MTRS').html('');
        isLengthValid = true;
    }
    var cc = $('#GrossWeight').val();
    if ($('#GrossWeight').val().length != 0) {
        var num = $('#GrossWeight').val().trim().replace(zeroPrecisionRegex, '');
        var GrossWeight = $('#GrossWeight').val().split('.');
        if (!numberValidation.test($("#GrossWeight").val())) {
            $('#spMAX_GROSS_WEIGHT_KGS').html('Please enter valid gross weight');
            isGrossWeightValid = false;
        } else
            if (GrossWeight[1] == "") {
                $('#spMAX_GROSS_WEIGHT_KGS').html('Please enter valid gross weight');
                isGrossWeightValid = false;
            }
            
            else
                 if (GrossWeight[1] != undefined) {
                    if (GrossWeight[1].length > 3) {
                        $('#spMAX_GROSS_WEIGHT_KGS').html('The maximum allowed decimal point is 3');
                        isGrossWeightValid = false;
                    } else {
                        $('#spMAX_GROSS_WEIGHT_KGS').html('');
                        isGrossWeightValid = true;
                    }
                }else if (isNaN(num) || num < 0 || CreateCautionIsValidDecimal(num) == false || CreateCautionValidateLength(num, 6)) {
                    $('#spMAX_GROSS_WEIGHT_KGS').html('The maximum allowed digit is 5');
                    isGrossWeightValid = false;
                } 
                else {
                    $('#spMAX_GROSS_WEIGHT_KGS').html('');
                    isGrossWeightValid = true;
                }
    }
    else {
        $('#spMAX_GROSS_WEIGHT_KGS').html('');
        isGrossWeightValid = true;
    }
    if ($('#AxleWeight').val().length != 0) {
        var num = $('#AxleWeight').val().trim().replace(zeroPrecisionRegex, '');
        var AxleWeight = $('#AxleWeight').val().split('.');
        if (!numberValidation.test($("#AxleWeight").val())) {
            $('#spMAX_AXLE_WEIGHT_KGS').html('Please enter valid axle weight');
            isAxleWeightValid = false;
        } else
            if (AxleWeight[1] == "") {
                $('#spMAX_AXLE_WEIGHT_KGS').html('Please enter valid axle weight');
                isAxleWeightValid = false;
            }
            
            else
                 if (AxleWeight[1] != undefined) {
                    if (AxleWeight[1].length > 3) {
                        $('#spMAX_AXLE_WEIGHT_KGS').html('The maximum allowed decimal point is 3');
                        isAxleWeightValid = false;
                    } else {
                        $('#spMAX_AXLE_WEIGHT_KGS').html('');
                        isAxleWeightValid = true;
                    }
                 } else if (isNaN(num) || num < 0 || CreateCautionIsValidDecimal(num) == false || CreateCautionValidateLength(num, 6)) {
                     $('#spMAX_AXLE_WEIGHT_KGS').html('The maximum allowed digit is 5');
                     isAxleWeightValid = false;
                 }
                else  {
                    $('#spMAX_AXLE_WEIGHT_KGS').html('');
                    isAxleWeightValid = true;
                }
    }
    else {
        $('#spMAX_AXLE_WEIGHT_KGS').html('');
        isAxleWeightValid = true;
    }
    if ($('#Speed').val().length != 0) {
        var num = $('#Speed').val().trim().replace(zeroPrecisionRegex, '');
        var Speed = $('#Speed').val().split('.');
        if (!numberValidation.test($("#Speed").val())) {
            $('#spMIN_SPEED_KPH').html('Please enter valid speed');
            isSpeedValid = false;
        } else
            if (Speed[1] == "") {
                $('#spMIN_SPEED_KPH').html('Please enter valid axle speed');
                isSpeedValid = false;
            }
            
            else
                if (isNaN(num) || CreateCautionValidateLength(num, 3)) {
                    $('#spMIN_SPEED_KPH').html('The maximum allowed digit is 2');
                    $('#spMIN_SPEED_KPH').show();
                    isSpeedValid = false;
                }
                else {
                    if (num < 0) {
                        $('#spMIN_SPEED_KPH').html('The maximum allowed digit is 2');
                        isSpeedValid = false;
                    }
                    else if (Speed[1] != undefined) {
                        if (Speed[1].length > 3) {
                            $('#spMIN_SPEED_KPH').html('The maximum allowed decimal point is 3');
                            isSpeedValid = false;
                        } else {
                            $('#spMIN_SPEED_KPH').html('');
                            isSpeedValid = true;
                        }
                    }
                    else {
                        $('#spMIN_SPEED_KPH').html('');
                        isSpeedValid = true;
                    }
                }
    } else {
        $('#spMIN_SPEED_KPH').html('');
        isSpeedValid = true;
    }

    //if (flageCheck == false) {
    //    return false;
    //}
    if (isSpeedValid && isHeightValid && isWidthValid && isLengthValid && isGrossWeightValid && isAxleWeightValid && isNameValid) {
        $(".error").hide();
        return true;
    }
    else {
        $(".error").css("display", "block");
        return false;
    }
}


function CautionAddReport() {
    var chk = valiadteCaution();
    if (chk == true) {
        CheckBoldStructure();
        CheckItalicStructure();
        CheckUnderlineStructure();
        //check validation end
        $("#SelectedTypeName").val($("input[name=SelectedType]:checked").val());
        $.ajax({
            url: '../Constraint/StoreCautionData',
            dataType: 'json',
            type: 'POST',
            data: $("#CautionInfo").serialize(),
            success: function (result) {
                if (result == true) {
                    CautionShowReportCreateCaution();
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

function CheckBoldStructure() {
    var text = $("#Description").length > 0 ? $("#Description")[0].innerHTML : "";
    var checkBold = (text.includes("</b>") || text.includes("font-weight: bold"));
    if (checkBold) {
        $('#chkBold').val('true');
    }
    else {

        $('#chkBold').val('false');
    }
}
function CheckItalicStructure() {
    var text = $("#Description").length > 0 ? $("#Description")[0].innerHTML : "";
    var checkItalic = (text.includes("</i>") || text.includes("text-decoration-line: underline"));
    if (checkItalic) {
        $('#chkItalic').val('true');
    }
    else {
        $('#chkItalic').val('false');
    }

}
function CheckUnderlineStructure() {
    var text = $("#Description").length > 0 ? $("#Description")[0].innerHTML : "";
    var checkUnderline = text.includes("</u>");
    if (checkUnderline) {
        $('#chkUnderline').val('true');
    }
    else {
        $('#chkUnderline').val('false');
    }
}
function feetinches1(value) {
    var rex = /^(\d+)'(\d+)(?:''|")$/;
    var match = rex.exec(value);
    var feet, inch;
    if (match) {
        feet = parseInt(match[1], 10);
        inch = parseInt(match[2], 10);
    }
    if (match)
        return true;
    else
        return false;
}
function CreateCautionIsValidDecimal(value) {
    var regex = /^\d*(\.)?(\d{0,2})?$/;
    if (regex.test(value)) {
        return true;
    } else {
        return false;
    }
}

function CautionShowReportCreateCaution(viewCaution = 0) {
    $("#dialogue").hide();
    var descriptionElement = document.getElementById("Description");
    var textContent = descriptionElement ? descriptionElement.textContent : "";
    var description = textContent.split('&nbsp;').join('_');

    startAnimation();
    var randomNumber = Math.random();
    
    $.ajax({
        url: '../Constraint/CautionAddReport',
        type: 'POST',
        cache: true,
        data: JSON.stringify({ ConstraintID: $('#ConstraintID').val(), random: randomNumber, description: description }),
        success: function (result) {
            stopAnimation();
            $("#dialogue").show();
            $("#overlay").show();
            $("#dialogue").html($(result).closest('.caution-add-report')[0]);
        }

    });
}
function redirectCaution() {
    window.location.href = "../Account/Login";
}

function isNonAlphaNum(evt) { // Validation For Special Characters & < >
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    var charStr = String.fromCharCode(charCode);
    if (charStr == "&" || charStr == "<" || charStr == ">") {
        alert(charStr + " is not Allowed ");
        $('#errdesc').show();
        return false;
    }
    else {
        $('#errdesc').hide();
        return true;
    }
}

function Textformating(txt) {
    document.execCommand(txt, false, null);
}

function CreateCautionValidateLength(val, length) {
    val = val.split('.');
    if (val[0].length >= length) {
        return true;
    }
    return false;
}