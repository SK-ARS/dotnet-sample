var CM;
var constraintDetails;
var numberValidation = new RegExp(/^[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)$/);
$(document).ready(function () {
    

    $('body').on('click', '.create-constraint .btn-save-constraint', function () {
        SaveCreateConstraint();
    });

    $('body').on('click', '.create-constraint .btn-cancel-constraint', function () {
        CancelCreateConstraint();
    });

    $('body').on('click', '.create-constraint .btn-open-constraint-nav', function () {
        openNavCreateConstraint();
    });

    $('body').on('click', '.create-constraint .btn-close-constraint-nav', function () {
        closeNavCreateConstraint();
    });

    $('body').on('click', '.create-constraint .btn-cancel-show-warining', function () {
        showConstraintWaring('false');
    });

    $('body').on('click', '.create-constraint .btn-reload-popup', function () {
        reloadFildPopup();
    });

    if ($("#hf_isPartialView").length <= 0) {
        CreateConstraintInit();
    }

    $('body').on('keyup', '.create-constraint .edit-normal', function () {
        $(this).closest('.row').find('.error').html('')
    });
   
});

function CreateConstraintInit() {
    $("#overlay").css('background', '');
    $("#overlay").css('overflow', '');
    $("#EndDateString").attr("autocomplete", "off");
    $("#StartDateString").attr("autocomplete", "off");

    $("#ConstraintTypeId").val(253001);

    $("#mySidenav1").css("display", "none");
    $("#card-swipe1").css("display", "none");
    $("#card-swipe2").css("display", "none");

    $("#dialogue").css("height", "inherit");
    $("#dialogue").css("box-shadow", "inherit");
    $("#overlay").css("background", "none");
    //$("#dialogue").css("width", "auto");
    $("#dialogue").css("margin-left", "0px");
    $("#dialogue").css("margin-top", "150px");

    if ($("#TopologyType").val() == '248001')
        $("#DirectionId").val(248001);

    else if ($("#TopologyType").val() == '248002')
        $("#DirectionId").val(248002);

    else
        $("#DirectionId").val(248003);

    $('#DirectionId').hide();
    var e = document.getElementById("DirectionId");
    if (e) {
        var str2 = e.options[e.selectedIndex].text;
    }

    $("#DirectionName").val(str2);
    CM = $("#CreateConstraint").serialize();
    constraintDetails = getConstraintDetails();

    $('#StartDateString').datepicker({      
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        changeMonth: true, changeYear: true,
        minDate: new Date(),
        onSelect: function (selected) {
            var startDate = selected.split("-").reverse().join("/");
            var splitDate = selected.split("/");
            var fromDate = new Date(splitDate[2], splitDate[1]-1, splitDate[0]);
            var toDate = $("#EndDateString").datepicker('getDate');
            //if (fromDate > toDate ) {
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

    removescroll();
    $("#dialogue").show();
    $("#overlay").show();
    //document.getElementById('OwnedByMe').checked = true;
    if (StructureId == "" || StructureId == undefined) {
        document.getElementById('OwnedByMe').checked = true;
        document.getElementById('Constraints').checked = true;
    }
}

function closeFildPopup() {
    $('#exampleModalCenter5').modal('hide');

}
function reloadFildPopup() {
    location.reload();

}

function closeNavCreateConstraint() {

    $("#createConsDiv").css("width", "0px");
    $("#createConsDiv").css("display", "none");

    $("#card-swipecon1").css("display","inline-block");
    $("#card-swipecon2").css("display","none");
}

function openNavCreateConstraint() {

    $("#createConsDiv").css("width","345px");
    $("#createConsDiv").css("display","block");

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

function SaveCreateConstraint() {

    var chk = ValidateCreateConstraint();
    if (chk == true) {
        //startAnimation();
        var _thisForm = $('#hidden_values');
        for (var i = 0; i < constraintDetails.geometry.OrdinatesArray.length; i++) {
            _thisForm.append('<input type="hidden" name="GEOMETRY.OrdinatesArray[' + i + ']" value="' + constraintDetails.geometry.OrdinatesArray[i] + '"/>');
        }

        //_thisForm.append('<input type="hidden" name="GEOMETRY.sdo_srid" value="' + 27700 + '" />');
        //_thisForm.append('<input type="hidden" name="GEOMETRY.OrdinatesArray[0]" value="' + constraintDetails.geometry.OrdinatesArray[0] + '" />');
        //_thisForm.append('<input type="hidden" name="GEOMETRY.OrdinatesArray[1]" value="' + constraintDetails.geometry.OrdinatesArray[1] + '" />');
        //_thisForm.append('<input type="hidden" name="GEOMETRY.OrdinatesArray" value="' + constraintDetails.geometry.OrdinatesArray + '" />');

        if (constraintDetails.ConstraintReferences != null) {
            var uniqueLinkIds = [];    // contains link ids in constraint
            var uniqueLinks = []; // temporary storage for constraint reference
            $.each(constraintDetails.ConstraintReferences, function (i, el) {

                if ($.inArray(el.constLink, uniqueLinkIds) === -1) {  //removing duplicated link ids in constraint
                    uniqueLinks.push(el);
                    uniqueLinkIds.push(el.constLink)
                }
            });
            constraintDetails.ConstraintReferences = [];  // clear the reference
            constraintDetails.ConstraintReferences = uniqueLinks; // inserting original link ids(removing duplicated ones's)
            for (var i = 0; i < constraintDetails.ConstraintReferences.length; i++) {
                setConstraintRefValues(i);
            }
        }

        var grosswt = $('#GrossWeight').val().trim();
        grosswt = (grosswt * 1000);
        $('#HdnGrossWeightKgs').val(Math.ceil(grosswt));
        var axlewt = $('#AxleWeight').val().trim();
        axlewt = (axlewt * 1000);
        $('#HdnAxleWeightKgs').val(Math.ceil(axlewt));

        if ($('#IsNodeConstraintFlag').is(":checked"))
            $('#HdnIsNodeConstraintFlag').val(true);
        else
            $('#HdnIsNodeConstraintFlag').val(false);

        //var CM = $("#CreateConstraint").serialize();
        $.ajax({
            url: '../Constraint/SavingConstraint',
            type: 'POST',
            cache: false,
            async: false,
            data: $("#CreateConstraint").serialize(),
            success: function (result) {
                SaveCreateConstraintSuccess(result);
            },
            error: function (result) {
            }
        });
    }
    //else {
    //    showWarningPopDialog('Validation failed', '', 'Ok', '', 'closeValidationFailError', 1, 'error');
    //}
}

function SaveCreateConstraintSuccess(result) {
    //stopAnimation();
    //modal-backdrop is not disabling itself
    //$(".modal-backdrop").css("height", "0px");
    $(".modal-backdrop").removeClass("fade");
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");

    if (result.Success == true) {
        var createdConstraintObject = {
            constraintId: result.ConstraintId,
            constraintName: $("#CreateConstraint")[0].ConstraintName.value,
            constraintType: $("#CreateConstraint")[0].ConstraintType.value,
            topologyType: constraintDetails.topologyType,
            geometry: constraintDetails.geometry,
            constraintCode: result.ConstraintCode,
        };
        var FirstCautionFlag = 1;
        ConstraintCreationflag(true, createdConstraintObject);
        $("#pop-warning").hide();
        var randomNumber = Math.random();
        
        //startAnimation();
        
        $("#dialogue").hide();
        $("#dialogue").html("");
        $("#dialogue").load('../Constraint/CreateCaution?ConstraintID=' + result.ConstraintId + '&CautionId=0&mode=add&random=' + randomNumber + '&FirstCautionFlag=' + FirstCautionFlag, function () {
            CreateCautionInit();
            $("#dialogue").show();
            $("#dialogue").css("display", "block");
        });
        //CreateCautionPage();
        //showWarningPopDialog('Constraints And Caution added successfully', 'Ok', '',  1, 'info');
        //showWarningPopDialog('Constraint saved successfully', 'o', 'CreateCautionPage', 'close_alert', '', 1, 'info');

    }
    else {
        ConstraintCreationflag(false, CM);
        if (result.ConstraintId == -2) {
            //$("#ConstraintcreationfailedPopup").show();
            //$('#ConstraintcreationfailedPopup').addClass('show').removeClass('fade');

            ShowErrorPopup("Constraint creation failed. You do not have sufficient privilege to create constraints in the selected roads.");

            //showWarningPopDialog('Constraint creation failed. You do not have sufficient privilege to create constraints in the selected roads.', 'Ok', '', 'close_alert', '', 1, 'info');
        }
        else {
            //$('#ConstraintcreationfailedPopup').addClass('show').removeClass('fade');
            //$("#ConstraintcreationfailedPopup").show();
            //ShowErrorPopup("Constraint creation failed.");

            $('#exampleModalCenter5').modal({ keyboard: false, backdrop: 'static' });
            $('#exampleModalCenter5').modal('show');
            $(".modal-backdrop").removeClass("show");
            $(".modal-backdrop").removeClass("modal-backdrop");

        }
    }

}
function preventvalues(e) {
    var height = $('#Height').val().split('.');
    if (height[1] != "") {
        var len = height[1].split();
    }
    //e = e || window.event;
    //var charCode = (typeof e.which == "undefined") ? e.keyCode : e.which;
    //var charStr = String.fromCharCode(charCode);
    //return false;

}
function ValidateCreateConstraint() {
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
       
            if (UOM == 692001 ) {
                var num = $('#Height').val().trim().replace(zeroPrecisionRegex, '');
                var height = $('#Height').val().split('.');
                if (!numberValidation.test($("#Height").val())) {
                    $('#spMaxHeight').html('Please enter valid height');
                    isHeightValid = false;

                } else
                if (height[1] == "") {
                    $('#spMaxHeight').html('Please enter valid height');
                    isHeightValid = false;

                }
               else
                if (isNaN(num) || CreateConstraintValidateLength(num, 5)) {
                $('#spMaxHeight').html('The maximum allowed digit is 4');
                isHeightValid = false;
                }
                else {
                if (num < 0) {
                    $('#spMaxHeight').html('The maximum allowed digit is 4');
                    isHeightValid = false;
                }
                else if (height[1] != undefined) {
                    if (height[1].length > 3) {
                        $('#spMaxHeight').html('The maximum allowed decimal point is 3');
                        isHeightValid = false;
                    } else {
                        $('#spMaxHeight').html('');
                        isHeightValid = true;
                    }
                }
                else {
                    $('#spMaxHeight').html('');
                    isHeightValid = true;
                }
            }
        }
        else {
            if (feetinches($('#Height').val()) == false) {
                $('#spMaxHeight').html('The format should be ' + "ft'" + 'in"');
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
            var Width = $('#Width').val().split('.');
            if (!numberValidation.test($("#Width").val())) {
                $('#spMaxWidth').html('Please enter valid width');
                isWidthValid = false;
            } else
                if (Width[1] == "") {
                    $('#spMaxWidth').html('Please enter valid width');
                    isWidthValid = false;
                }
                else
            if (isNaN(num) || CreateConstraintValidateLength(num, 5)) {
                $('#spMaxWidth').html('The maximum allowed digit is 4');
                isWidthValid = false;
            }
            else {
                if (num < 0) {
                    $('#spMaxWidth').html('The maximum allowed digit is 4');
                    isWidthValid = false;
                }
                else if (Width[1] != undefined) {
                    if (Width[1].length > 3) {
                        $('#spMaxWidth').html('The maximum allowed decimal point is 3');
                        isWidthValid = false;
                    } else {
                        $('#spMaxWidth').html('');
                        isWidthValid = true;
                    }
                }
                else {
                    $('#spMaxWidth').html('');
                    isWidthValid = true;
                }
            }
        }
        else {
            if (feetinches($('#Width').val()) == false) {
                $('#spMaxWidth').html('The format should be ' + "ft'" + 'in"');
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
            var Length = $('#Length').val().split('.');
            if (!numberValidation.test($("#Length").val())) {
                $('#spMaxLength').html('Please enter valid length');
                isLengthValid = false;
            } else
                if (Length[1] == "") {
                    $('#spMaxLength').html('Please enter valid length');
                    isLengthValid = false;
                }
               
                else
            if (isNaN(num) || CreateConstraintValidateLength(num, 5)) {
                $('#spMaxLength').html('The maximum allowed digit is 4');
                isLengthValid = false;
            }
            else {
                if (num < 0) {
                    $('#spMaxLength').html('The maximum allowed digit is 4');
                    isLengthValid = false;
                }
                else if (Length[1] != undefined) {
                    if (Length[1].length > 3) {
                        $('#spMaxLength').html('The maximum allowed decimal point is 3');
                        isLengthValid = false;
                    } else {
                        $('#spMaxLength').html('');
                        isLengthValid = true;
                    }
                }
                else {
                    $('#spMaxLength').html('');
                    isLengthValid = true;
                }
            }
        }
        else {
            if (feetinches($('#Length').val()) == false) {
                $('#spMaxLength').html('The format should be ' + "ft'" + 'in"');
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
        var num = $('#GrossWeight').val().trim().replace(zeroPrecisionRegex, '');
        var GrossWeight = $('#GrossWeight').val().split('.');
        if (!numberValidation.test($("#GrossWeight").val())) {
            $('#spGrossWeight').html('Please enter valid gross weight');
            isGrossWeightValid = false;
        }else
        if (GrossWeight[1] == "") {
            $('#spGrossWeight').html('Please enter valid gross weight');
            isGrossWeightValid = false;
        }
       
        else
        if (IsValidDecimalCreateConst(num) || CreateConstraintValidateLength(num, 6)) {
            if (num < 0 || CreateConstraintValidateLength(num, 6)) {
                $('#spGrossWeight').html('The maximum allowed digit is 5');
                isGrossWeightValid = false;
            }
            else if (GrossWeight[1] != undefined) {
                if (GrossWeight[1].length > 3) {
                    $('#spGrossWeight').html('The maximum allowed decimal point is 3');
                    isGrossWeightValid = false;
                } else {
                    $('#spGrossWeight').html('');
                    isGrossWeightValid = true;
                }
            }
            else {
                $('#spGrossWeight').html('');
                isGrossWeightValid = true;
            }
        }
        else {
            if (GrossWeight[1] != undefined) {
                if (GrossWeight[1].length > 3) {
                    $('#spGrossWeight').html('The maximum allowed decimal point is 3');
                    isGrossWeightValid = false;
                } else {
                    $('#spGrossWeight').html('');
                    isGrossWeightValid = true;
                }
            }
            else {
                $('#spGrossWeight').html('');
                isGrossWeightValid = true;
            }
        }

    }
    else {
        $('#spGrossWeight').html('');
        isGrossWeightValid = true;
    }

    if ($('#AxleWeight').val().trim().length != 0) {
        var num = $('#AxleWeight').val().trim().replace(zeroPrecisionRegex, '');
        var AxleWeight = $('#AxleWeight').val().split('.');
        if (!numberValidation.test($("#AxleWeight").val())) {
            $('#spAxleWeight').html('Please enter valid axle weight');
            isAxleWeightValid = false;
        }else
        if (AxleWeight[1] == "") {
            $('#spAxleWeight').html('Please enter valid axle weight');
            isAxleWeightValid = false;
        }
        else
        if (IsValidDecimalCreateConst(num) || CreateConstraintValidateLength(num, 6)) {
            if (num < 0 || CreateConstraintValidateLength(num, 6)) {
                $('#spAxleWeight').html('The maximum allowed digit is 5');
                isAxleWeightValid = false;
            }
            else if (AxleWeight[1] != undefined) {
                if (AxleWeight[1].length > 3) {
                    $('#spAxleWeight').html('The maximum allowed decimal point is 3');
                    isAxleWeightValid = false;
                } else {
                    $('#spAxleWeight').html('');
                    isAxleWeightValid = true;
                }
            }
            else {
                $('#spAxleWeight').html('');
                isAxleWeightValid = true;
            }
        }
        else {
            if (AxleWeight[1] != undefined) {
                if (AxleWeight[1].length > 3) {
                    $('#spAxleWeight').html('The maximum allowed decimal point is 3');
                    isAxleWeightValid = false;
                } else {
                    $('#spAxleWeight').html('');
                    isAxleWeightValid = true;
                }
            }
            else {
                $('#spAxleWeight').html('');
                isAxleWeightValid = true;
            }
        }

    }
    else {
        $('#spAxleWeight').html('');
        isAxleWeightValid = true;
    }


    var strstartdate = $('#StartDateString').val();

    var strenddate = $('#EndDateString').val();
    //var result = ValidateMomentDateTime(strstartdate, strenddate, false, format = "DD/MM/YYYY");
   // if (result==2) {
       // $('#spEndDate').html('End date must be greater/equal to start date');
        //isDateValid = false;
    //}
   //else {
       // $('#spEndDate').html('');
        isDateValid = true;
    //}

    if (isHeightValid && isWidthValid && isLengthValid && isGrossWeightValid && isAxleWeightValid && isNameValid && isDateValid) {
        $(".error").hide();
        return true;
    }
    else {
        $(".error").css("display", "block");
        return false;
    }
}

function setConstraintRefValues(i) {
    var _thisForm = $('#hidden_values');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].constLink" value="' + Math.round(constraintDetails.ConstraintReferences[i].constLink) + '" />');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].ToNorthing" value="' + Math.round(constraintDetails.ConstraintReferences[i].toNorthing) + '" />');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].ToEasting" value="' + Math.round(constraintDetails.ConstraintReferences[i].toEasting) + '" />');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].FromNorthing" value="' + Math.round(constraintDetails.ConstraintReferences[i].fromNorthing) + '" />');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].FromEasting" value="' + Math.round(constraintDetails.ConstraintReferences[i].fromEasting) + '" />');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].LinearRef" value="' + Math.round(constraintDetails.ConstraintReferences[i].linearRef) + '" />');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].ToLinearRef" value="' + constraintDetails.ConstraintReferences[i].toLinearRef + '" />');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].FromLinearRef" value="' + constraintDetails.ConstraintReferences[i].fromLinearRef + '" />');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].Easting" value="' + Math.round(constraintDetails.ConstraintReferences[i].easting) + '" />');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].Northing" value="' + Math.round(constraintDetails.ConstraintReferences[i].northing) + '" />');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].IsPoint" value="' + constraintDetails.ConstraintReferences[i].isPoint + '" />');
    _thisForm.append('<input type="hidden" name="ConstraintReferences[' + i + '].Direction" value="' + constraintDetails.ConstraintReferences[i].direction + '" />');
}

function IsValidNumber(value) {
    if (value == parseFloat(value)) {
        return true;
    }
    else {
        return false;
    }
}

function feetinches(value) {
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
function CancelCreateConstraint() {
    $('#pop-warning').hide();

    Closepopup();
    ConstraintCreationflag(false, CM);
}

function Closepopup() {
    $("#dialogue").html('');
    $("#dialogue").hide();

    $("#overlay").hide();
    return false;
}
function showConstraintWaring(show) {
    if (show == 'true') {
        $('#exampleModalCenter').modal({ keyboard: false, backdrop: 'static' });
        $("#exampleModalCenter").modal('show');
        $(".modal-backdrop").removeClass("show");
        $(".modal-backdrop").removeClass("modal-backdrop");
    }
    else {
        $("#exampleModalCenter").modal('hide');
    }
}
$("#ConstraintTypeId").on({
    mouseenter: function () {
        //stuff to do on mouse enter
        $(".custselected").css("display","block !important;");
        var selectedVal = $("#ConstraintTypeId option:selected").text();
        $("#lblSelectedVal").html(selectedVal);

    },
    mouseleave: function () {
        //stuff to do on mouse leave
        $(".custselected").css("display","none !important;");
        //var selectedVal = $("#ConstraintTypeId option:selected").text();


    }
});
function IsValidDecimalCreateConst(value) {
    var regex = /^\d*(\.)?(\d{0,2})?$/;
    if (regex.test(value)) {
        return true;
    } else {
        return false;
    }
}
function CreateConstraintValidateLength(val, length) {
    val = val.split('.');
    if (val[0].length >= length) {
        return true;
    }
    return false;
}