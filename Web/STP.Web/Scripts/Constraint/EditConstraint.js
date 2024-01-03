var ConstraintID = parseInt($("#ConstraintID").val());
$(document).ready(function () {
    $('body').on('click', '.edit-constraint .btn-manage-constraint', function (e) {
        e.preventDefault();
        ManageConstraint();
    });

    $('body').on('click', '.edit-constraint .btn-manage-constraint-cancel', function () {
        Cancel();
    });

    $('body').on('click', '.edit-constraint .btn-open-constraint-nav', function () {
        OpenNavEditConstraint();
    });

    $('body').on('click', '.edit-constraint .btn-close-constraint-nav', function () {
        CloseNavEditConstraint();
    });

    $('body').on('click', '.edit-constraint .btn-save-constraint', function () {
        SaveConstraintFromReport();
    });
    EditConstraintInit();

    $('body').on('keyup', '.edit-constraint .edit-normal', function () {
        $(this).closest('.row').find('.error').html('')
    });
});

function EditConstraintInit() {
    $("#dialogue").show();
    $("#overlay").attr('style', '');
    $("#overlay").css('display', 'block');
    //$("#overlay").css('background', 'none');
    ConstraintID = parseInt($("#ConstraintID").val());
    $('#StartDateString').datepicker({
        
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        changeMonth: true, changeYear: true,
        minDate: new Date(),
        onSelect: function (selected) {
            var startDate = selected.split("-").reverse().join("/");
            var splitDate = selected.split("/");
            var fromDate = new Date(splitDate[2], splitDate[1] - 1, splitDate[0]);
            var toDate = $("#EndDateString").datepicker('getDate');
            //if (fromDate > toDate) {
            //    $("#EndDateString").datepicker("setDate", fromDate);
            //}
        },
        beforeShow: function (textbox, instance) {
            var rect = textbox.getBoundingClientRect();
            setTimeout(function () {
                var scrollTop = $("body").scrollTop();
                instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
            }, 0);
        }
    });

    $('#EndDateString').datepicker({
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        changeMonth: true, changeYear: true,
        minDate: new Date(),
        beforeShow: function (textbox, instance) {
            var startDate = $("#StartDateString").datepicker('getDate');
            $("#EndDateString").datepicker("option", "minDate", startDate);
            var rect = textbox.getBoundingClientRect();
            setTimeout(function () {
                var scrollTop = $("body").scrollTop();
                instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
            }, 0);
        }
    });
}
function Cancel() {
    if (ConstraintID!= 0) {
        var flageditmode = $('#flageditmode').val();
        $("#dialogue").hide();
        var randomNumber = Math.random();
        $("#dialogue").load('../Constraint/ViewConstraint?ConstraintID=' + ConstraintID + '&flageditmode=' + flageditmode + '&random=' + randomNumber, function () {
            ViewConstraintInit();
        });
    }
}
//function for submit the form for loading html (popup)
function EditHelp_popup() {
    //var url = geturl(location.href);
    var div_name = $('.body').find('.PopupHelpName').attr('id');
    var url = "PopUp" + div_name;

    $('#url_html').val(url);
    $('#flag').val(1);
    $('#form_html_helpbuilder').submit();
}

function SaveConstraintFromReport() {
    $.ajax({
        url: '../Constraint/UpdateConstraint',
        dataType: 'json',
        type: 'POST',
        data: $("#ReportInfo").serialize(),
        success: function (result) {
            if (result) {
                stopAnimation();
                $("#overlay").css('background', '');
                $("#overlay").hide();
                $("#dialogue").hide();
                showToastMessage({
                    message: "Constraint updated successfully",
                    type: "success"
                });
                OpenViewConstraintFromReport();
            }
            else {
                alert(faild);
            }
        },
        error: function (xhr, status) {
        }
    });
}

function OpenViewConstraintFromReport() {
    //$("#pop-warning").unload();
    //$("#pop-warning").hide();
    $("#dialogue").hide();
    CloseSuccessModalPopup();
    var randomNumber = Math.random();
    $("#dialogue").load('../Constraint/ViewConstraint?ConstraintID=' + ConstraintID + '&random=' + randomNumber, function () {
        ViewConstraintInit();
    });
}

function ManageConstraint() {
    if (EditConstraintValidate()) {
        startAnimation("");
        $.ajax({
            url: '../Constraint/ConstraintSaveModel',
            dataType: 'json',
            type: 'POST',
            data: $("#ConstraintInfo").serialize(),
            success: function (result) {

                ConstraintShowReport();
            },
            error: function (xhr, status) {
            }
        });
    }
}
function EditConstraintValidate() {
    var zeroPrecisionRegex = /(\.0{0,20})(0+)$/;
    var isHeightValid = false;
    var isWidthValid = false;
    var isLengthValid = false;

    var isGrossWeightValid = false;
    var isAxleWeightValid = false;
    var isNameValid = false;
    var isDateValid = false;
    var UOM = $('#UOM').val();
    if ($('#ConstraintName').val().trim().length == 0) {

        $('#spCONSTRAINT_NAME').html('Please enter constraint name');
        isNameValid = false;
    }
    else {
        $('#spCONSTRAINT_NAME').html('');
        isNameValid = true;

    }
    if ($('#Height').val().trim().length != 0) {
        if (UOM == 692001) {
            var num = $('#Height').val().trim().replace(zeroPrecisionRegex, '');
            if (isNaN(num) || EditConstraintValidateLength(num, 4)) {
                $('#spMaxHeight').html('The maximum allowed digit is 4');
                isHeightValid = false;
            }
            else {
                if (num < 0) {
                    $('#spMaxHeight').html('Please enter valid height');
                    isHeightValid = false;
                }
                else {
                    $('#spMaxHeight').html('');
                    isHeightValid = true;
                }
            }
        }
        else {
            if (feetinchesEditConst($('#Height').val()) == false) {
                $('#spMaxHeight').html('The format should be ft\' in\"');
                isHeightValid = false;
            } else {
                $('#spMaxHeight').html('');
                isHeightValid = true;
            }
        }

    }
    else {
        $('#spMaxHeight').html('');
        isHeightValid = true;
    }

    if ($('#Width').val().trim().length != 0) {
        if (UOM == 692001) {
            var num = $('#Width').val().trim().replace(zeroPrecisionRegex, '');
            if (isNaN(num) || EditConstraintValidateLength(num, 4)) {
                $('#spMaxWidth').html('The maximum allowed digit is 4');
                isWidthValid = false;
            }
            else {
                if (num < 0) {
                    $('#spMaxWidth').html('Please enter valid width');
                    isWidthValid = false;
                }
                else {
                    $('#spMaxWidth').html('');
                    isWidthValid = true;
                }
            }
        }
        else {
            if (feetinchesEditConst($('#Width').val()) == false) {
                $('#spMaxWidth').html('The format should be ft\' in\"');
                isWidthValid = false;
            } else {
                $('#spMaxWidth').html('');
                isWidthValid = true;
            }
        }

    }
    else {
        $('#spMaxWidth').html('');
        isWidthValid = true;
    }

    if ($('#Length').val().trim().length != 0) {
        if (UOM == 692001) {
            var num = $('#Length').val().trim().replace(zeroPrecisionRegex, '');
            if (isNaN(num) || EditConstraintValidateLength(num, 4)) {
                $('#spMaxLength').html('The maximum allowed digit is 4');
                isLengthValid = false;
            }
            else {
                if (num < 0) {
                    $('#spMaxLength').html('Please enter valid length');
                    isLengthValid = false;
                }
                else {
                    $('#spMaxLength').html('');
                    isLengthValid = true;
                }
            }
        }
        else {
            if (feetinchesEditConst($('#Length').val()) == false) {
                $('#spMaxLength').html('The format should be ft\' in\"');
                isLengthValid = false;
            } else {
                $('#spMaxLength').html('');
                isLengthValid = true;
            }
        }

    }
    else {
        $('#spMaxLength').html('');
        isLengthValid = true;
    }

    if ($('#GrossWeight').val().trim().length != 0) {
        var num = $('#GrossWeight').val().trim().replace(zeroPrecisionRegex,'');

        if (IsValidDecimalEditConst(num) || EditConstraintValidateLength(num, 6)) {
            if (num < 0 || EditConstraintValidateLength(num, 5)) {
                $('#spGrossWeight').html('The maximum allowed digit is 5');
                isGrossWeightValid = false;
            }
            else {
                $('#spGrossWeight').html('');
                isGrossWeightValid = true;
            }
        }
        else {
            $('#spGrossWeight').html('The maximum allowed digit is 5');
            isGrossWeightValid = false;
        }

    }
    else {
        $('#spGrossWeight').html('');
        isGrossWeightValid = true;
    }

    if ($('#AxleWeight').val().trim().length != 0) {
        var num = $('#AxleWeight').val().trim().replace(zeroPrecisionRegex, '');

        if (IsValidDecimalEditConst(num) || EditConstraintValidateLength(num, 6)) {
            if (num < 0 || EditConstraintValidateLength(num, 5)) {
                $('#spAxleWeight').html('The maximum allowed digit is 5');
                isAxleWeightValid = false;
            }
            else {
                $('#spAxleWeight').html('');
                isAxleWeightValid = true;
            }
        }
        else {
            $('#spAxleWeight').html('The maximum allowed digit is');
            isAxleWeightValid = false;
        }

    }
    else {
        $('#spAxleWeight').html('');
        isAxleWeightValid = true;
    }


   
        isDateValid = true;
    

    if (isHeightValid && isWidthValid && isLengthValid && isGrossWeightValid && isAxleWeightValid && isNameValid && isDateValid) {
        $(".error").hide();
        return true;
    }
    else {
        $(".error").css("display", "block");
        return false;
    }
}
function feetinchesEditConst(value) {
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

function IsValidDecimalEditConst(value) {
    var regex = /^\d*(\.)?(\d{0,2})?$/;
    if (regex.test(value)) {
        return true;
    } else {
        return false;
    }
}
function ConstraintShowReport() {
    //$("#dialogue").hide();
    //startAnimation();
    var randomNumber = Math.random();
    ;
    $.ajax({
        url: '../Constraint/ConstraintShowReport',
        dataType: 'json',
        type: 'POST',
        data: $("#ConstraintInfo").serialize(),
        success: function (result) {
            //$('#exampleModalCenter').modal('show');
            SaveConstraintFromReport();
        },
        error: function (xhr, status) {
        }
    });

}
function OpenNavEditConstraint() {

    $("#edit-caution").css("width","345px");
    $("#edit-caution").css("display","block");

    $("#card-swipecon1").css("display","none");
    $("#card-swipecon2").css("display","inline-block");
    $("#card-swipecon2").css("margin-left","415px");
    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("mySidenav1").style.width = "auto";
        }
    }
    var x = window.matchMedia("(max-width: 410px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)

}
function CloseNavEditConstraint() {

    $("#edit-caution").css("width","0px");
    $("#edit-caution").css("display","none");

    $("#card-swipecon1").css("display","inline-block");
    $("#card-swipecon2").css("display","none");

}
function EditConstraintValidateLength(val, length) {
    val = val.split('.');
    if (val[0].length > length) {
        return true;
    }
    return false;
}
