var isvalid = true;
var flag = true;
var userTypeIDVal = $('#hf_userTypeID').val();
var fromAffectedPartiesVal = $('#hf_fromAffectedParties').val();
var Granter_IDVal = $('#hf_Granter_ID').val();
var GranterNameVal = $('#hf_GranterName').val();
var Grantor_name = $('#hdnGrantor_Org').val();
var Grant_ID = $('#GranterID').val();

function grantby() {
    $("#GrantedBy").autocomplete({
        source: function (request, response) {
            ;
            $.ajax({
                url: '../Dispensation/OrganisationSummary',
                dataType: "json",
                data: {
                    SearchString: request.term, page: 0, pageSize: 0
                },
                success: function (data) {
                    ;
                    response($.map(data, function (item) {
                        return { label: item.OrganisationName, value: item.OrganisationId };
                    }));
                },
                error: function (jqXHR, exception, errorThrown) {
                    console.log(errorThrown);
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            // Set selection
            $('#GrantedBy').val(ui.item.label); // display the selected text
            $('#SelectOrganisationId').val(ui.item.value); // save selected id to input
            return false;
        },
        focus: function (event, ui) {
            /* $("#GrantedBy").val(ui.item.label);*/
            return false;
        }

    });

}
$(document).ready(function () {
    $('body').on('click', '#btn_saveDispensation', function (e) {

        var hf = $('#hf_IsPlanMovmentGlobal').val();
        if (hf != "True") {
            e.preventDefault();
            var fromAffectedParties = $('#hf_fromAffectedParties').val();
            var hideLayout = $('#hf_hidelayout').val();
            savedispensations(fromAffectedParties, hideLayout, false);
        }
        else {
            e.preventDefault();
            savedispensations(true, true, true);
        }
    });
    $('body').on('click', '#GrantedBy', function (e) {
        e.preventDefault();
        grantby();
    });

    if ($('#hf_IsPlanMovmentGlobal').length == 0) {
        if ((userTypeIDVal == 696001) || (userTypeIDVal == 696002)) {
        }
        if (userTypeIDVal == 696007) {
        }
    }
    if (fromAffectedPartiesVal == 'True' || fromAffectedPartiesVal == 'true') {
        $('#confirm_btn').attr("disabled", true);
        $('#back_btn').hide();
    }

    $("#FromDate").datepicker({
        dateFormat: "dd-mm-yy",
        changeYear: true,
        changeMonth: true,
        minDate: new Date(),
        onSelect: function (selected) {
            var frmdt = selected.split("-").reverse().join("/");
            var dt = new Date(frmdt);
            var endDate = $("#ToDate").val();
            var dtnewdate1 = endDate.split("-").reverse().join("/");
            dt.setDate(dt.getDate());// + 1
            //  $("#ToDate").datepicker("option", "minDate", dt || 1);
            if (dt > new Date(dtnewdate1)) {
                $("#ToDate").datepicker("setDate", dt);
            }
        },
        beforeShow: function (a, b) {
            var cnt = 0;
            var interval = setInterval(function () {
                cnt++;
                if (b.dpDiv.is(":visible")) {
                    var parent = b.input.closest("div");
                    b.dpDiv.position({ my: "left top", at: "left bottom", of: parent });
                    clearInterval(interval);
                } else if (cnt > 50) {
                    clearInterval(interval);
                }
            }, 10);
        }
    });
    $("#ToDate").datepicker({
        dateFormat: "dd-mm-yy",
        changeYear: true,
        changeMonth: true,
        minDate: new Date(),
        onSelect: function (selected) {
            //selected = selected.replaceAll('-', ' ');
            //var dt = new Date(selected);
            //dt.setDate(dt.getDate() - 1);
            //$("#FromDate").datepicker("option", "maxDate", dt);
            var date = $(this).datepicker('getDate');
            if (date) {
                date.setDate(date.getDate()); //- 1
            }
            //  $('#FromDate').datepicker('option', 'maxDate', date );
        },
        beforeShow: function (a, b) {
            var cnt = 0;
            var interval = setInterval(function () {
                cnt++;
                if (b.dpDiv.is(":visible")) {
                    var parent = b.input.closest("div");
                    b.dpDiv.position({ my: "left top", at: "left bottom", of: parent });
                    clearInterval(interval);
                } else if (cnt > 50) {
                    clearInterval(interval);
                }
            }, 10);
        }
    });

    
    RemoveError();
    ShowVehicleRestrications();

    $("#Gross").keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
    });
    $("#Axle").keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
    });
    $("#Width").keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
    });
    $("#Height").keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
    });
    $("#Length").keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
    });

    $("#Gross").change(function (e) {
        
        var a = $('#Gross').val();
        if (a > 99999999) {
            $('#errorChk').text("Gross weight exceeds the maximum allowed weight 99999999 kg");
            $('#Axle').attr('readonly', true);
            $('#Height').attr('readonly', true);
            $('#Width').attr('readonly', true);
            $('#Length').attr('readonly', true);
            flag = 1;
        }
        else {
            $('#errorChk').text("");
            $('#Axle').attr('readonly', false);
            $('#Height').attr('readonly', false);
            $('#Width').attr('readonly', false);
            $('#Length').attr('readonly', false);
            flag = 0;
        }
    });
    $("#Axle").change(function (e) {
        var a = $('#Axle').val();
        if (a > 99999999) {
            $('#errorChk').text("Axle weight exceeds the maximum allowed weight 99999999 kg");
            $('#Gross').attr('readonly', true);
            $('#Height').attr('readonly', true);
            $('#Width').attr('readonly', true);
            $('#Length').attr('readonly', true);
            flag = 1;
        }
        else {
            $('#errorChk').text("");
            $('#Gross').attr('readonly', false);
            $('#Height').attr('readonly', false);
            $('#Width').attr('readonly', false);
            $('#Length').attr('readonly', false);
            flag = 0;
        }
    });
    $("#Length").change(function (e) {
        var len = $("#Length").val();
        if (len > 999.99) {
            $('#errorChk').text("Length exceeds the maximum allowed length 999.999 m");
            $('#Axle').attr('readonly', true);
            $('#Height').attr('readonly', true);
            $('#Width').attr('readonly', true);
            $('#Gross').attr('readonly', true);
            flag = 1;
        }
        else {
            $('#errorChk').text("");
            $('#Axle').attr('readonly', false);
            $('#Height').attr('readonly', false);
            $('#Width').attr('readonly', false);
            $('#Gross').attr('readonly', false);
            flag = 0;
        }
    });
    $("#Width").change(function (e) {
        var len = $("#Width").val();
        if (len > 999.99) {
            $('#errorChk').text("Width exceeds the maximum allowed width 999.999 m");
            $('#Axle').attr('readonly', true);
            $('#Height').attr('readonly', true);
            $('#Length').attr('readonly', true);
            $('#Gross').attr('readonly', true);
            flag = 1;
        }
        else {
            $('#errorChk').text("");
            $('#Axle').attr('readonly', false);
            $('#Height').attr('readonly', false);
            $('#Length').attr('readonly', false);
            $('#Gross').attr('readonly', false);
            flag = 0;
        }
    });
    $("#Height").change(function (e) {
        var len = $("#Height").val();
        if (len > 999.99) {
            $('#errorChk').text("Height exceeds the maximum allowed Height 999.999 m");
            $('#Axle').attr('readonly', true);
            $('#Width').attr('readonly', true);
            $('#Length').attr('readonly', true);
            $('#Gross').attr('readonly', true);
            flag = 1;
        }
        else {
            $('#errorChk').text("");
            $('#Axle').attr('readonly', false);
            $('#Width').attr('readonly', false);
            $('#Length').attr('readonly', false);
            $('#Gross').attr('readonly', false);
            flag = 0;
        }
    });

    $('body').on('click', '#btn_cancel', function (e) {
        e.preventDefault();
        btncancel();
    });

    $('body').off('focus', '#helpdeskDelegation .edit-normal,#helpdeskDelegation .form-control');
    $('body').on('focus', '#helpdeskDelegation .edit-normal,#helpdeskDelegation .form-control', function (e) {
        $(this).closest('.input-field').find('.field-validation-error').html('');
    });
});

function grantby() {
    $("#GrantedBy").autocomplete({
        source: function (request, response) {
            ;
            $.ajax({
                url: '../Dispensation/OrganisationSummary',
                dataType: "json",
                data: {
                    SearchString: request.term, page: 0, pageSize: 0
                },
                success: function (data) {
                    ;
                    response($.map(data, function (item) {
                        return { label: item.OrganisationName, value: item.OrganisationId };
                    }));
                },
                error: function (jqXHR, exception, errorThrown) {
                    console.log(errorThrown);
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            // Set selection
            $('#GrantedBy').val(ui.item.label); // display the selected text
            $('#SelectOrganisationId').val(ui.item.value); // save selected id to input
            return false;
        },
        focus: function (event, ui) {
            /* $("#GrantedBy").val(ui.item.label);*/
            return false;
        }

    });

}

function savedispensations(fromAffectedParties, hideLayout,hideCancel=true) {
    var mode = $('#hf_mode').val();
    var Grantor_name = $('#hdnGrantor_Org').val();
    var Granter_ID = $('#hdnGranter_ID').val();
    if ($('#Granter_ID').val() != "") {
        Granter_ID = $('#Granter_ID').val();
    }
    if ($('#Granter_ID').val() != "" && $('#Granter_ID').val()!='0') {
        mode = "create";
        $("#ErrorPage").val('False');
    }
    
    $('.field-validation-error').html('');
    var isValid = ValidateDateDispensation();
    if (!isValid)
        return false;
    if (isValid) {
        $.ajax({
            url: '../Dispensation/CreateDispensation',
            type: 'POST',
            data: $("#DispensationInfo").serialize() + "&mode=" + mode + "&fromAffectedParties=" + fromAffectedParties + "&Granter_ID=" + Granter_ID + "&hideLayout=" + hideLayout,
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (document.getElementById("General") !== null) {
                    $('#div_dispensation').hide();
                    $('#div_dispensation').html(result);
                    $("#General").html('');
                    var error = $("#ErrorPage").val();
                    if (error == 'True') {
                        $('#div_dispensation').html('');
                        $("#General").html(result);
                        if (fromAffectedPartiesVal == 'True' || fromAffectedPartiesVal == 'true') {
                            $('#divCreateDispensation').unwrap('#banner-container');
                        }
                    }
                    else {
                        $('#confirm_btn').removeAttr("disabled");
                        $('#back_btn').show();
                        $("#General").hide();
                        $('#route-assessment').show();
                        $("#btncreatedispensation").hide();
                        $('#div_dispensation').show();
                        if (hideCancel)
                            $('#btn_cancel').hide();
                        if (Granter_ID == "" && Grantor_name != "")
                            ViewDispensationAffParties(Granter_IDVal, GranterNameVal);
                        else {
                            ViewDispensationAffParties(Granter_ID, Grantor_name);
                        }
                    }
                }
                else {
                    if (!result.Success) {
                        $('body').html(result);
                    }
                    else {
                        var DRefNo = $('#DispensationReferenceNo').val();
                        if (mode == 'Edit') {
                            ShowSuccessModalPopup('Dispensation reference number "' + DRefNo + '" updated successfully', "RedirectToManageDispensationList");
                        }
                        else {
                            ShowSuccessModalPopup('New dispensation reference number "' + DRefNo + '" created successfully', "RedirectToManageDispensationList");
                        }
                        
                    }
                   
                }
            },
            error: function (xhr, status) {
                stopAnimation();
            },
            complete: function () {
                stopAnimation();
            }
             
        });
    }
    //stopAnimation();
}

function RedirectToManageDispensationList() {
    var url = "../Dispensation/ManageDispensation";
    window.location.href = url;
}

function btncancel() {
    /*if ("#hf_IsPlanMovmentGlobal".val() != "True") {*/
    var Grantor_name = $('#hdnGrantor_Org').val();
     var Grant_ID = $('#Granter_ID').val();
    var fromAffectedPartiesVal = $('#DispensationInfo').attr("fromaffectedparties");
        if (fromAffectedPartiesVal == 'True' || fromAffectedPartiesVal == 'true') {
            $("#General").hide();
            $('#route-assessment').show();
            $("#btncreatedispensation").hide();
            $('#div_dispensation').show();
            ViewDispensationAffParties(Grant_ID, Grantor_name);
            $('#confirm_btn').removeAttr("disabled");
            $('#back_btn').show();
        }
        else {
            var url = "../Dispensation/ManageDispensation";
            window.location.href = url;
        }
   /* }*/
    }
    

function RemoveError() {
    $('body').find('input:text').click(function () {
        $(this).closest('tr').find('.field-validation-error').text('');
        $(this).closest('tr').find('.field-validation-error span').text('');
    });
}

var isValid1 = true;

function ValidateDateDispensation() {

    var isValid = true;

    var dt1 = $('#FromDate').val();
    var dt2 = $('#ToDate').val();
    var dtnewdate1 = dt1.split("-").join("/");
    var dtnewdate2 = dt2.split("-").join("/");

    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var hour = d.getHours();
    var minute = d.getMinutes();
    var second = d.getSeconds();
    var output = (('' + day).length < 2 ? '0' : '') + day + '/' +
        (('' + month).length < 2 ? '0' : '') + month + '/' +
        d.getFullYear() + ' ' +
        (('' + hour).length < 2 ? '0' : '') + hour + ':' +
        (('' + minute).length < 2 ? '0' : '') + minute;


    var ArrayDateTime1 = dtnewdate1.split(' ');
    var ArrayDMY1 = ArrayDateTime1[0].split('/');
    var NewDate1 = new Date(ArrayDMY1[2], ArrayDMY1[1], ArrayDMY1[0], 0, 0, 0, 0);
    var TimeOfDate1 = NewDate1.getTime();

    var ArrayDateTime2 = dtnewdate2.split(' ');
    var ArrayDMY2 = ArrayDateTime2[0].split('/');
    var NewDate2 = new Date(ArrayDMY2[2], ArrayDMY2[1], ArrayDMY2[0], 0, 0, 0, 0);
    var TimeOfDate2 = NewDate2.getTime();

    var ArrayDateTime3 = output.split(' ');
    var ArrayDMY3 = ArrayDateTime3[0].split('/');
    var NewTodayDate = new Date(ArrayDMY3[2], ArrayDMY3[1], ArrayDMY3[0], 0, 0, 0, 0);
    var TimeOfTodayDate = NewTodayDate.getTime();
    if (ArrayDMY1[0] == "31" && ArrayDMY2[0] == "01") {
        if (ArrayDMY1[1] < ArrayDMY2[1]) {
            $('#spnFromDate').html('');
            $('#spnToDate').html('');

        }
    }
    else {
        // ensure both evaluate to true
        if (dt1 && dt2) {
            if (TimeOfDate1 > TimeOfDate2) {
                $('#spnToDate').html('To date must be greater than from date.');
                $('#spnFromDate').html('');
                isValid = false;
            }
            else {
                //if (TimeOfDate1 < TimeOfTodayDate) {
                //    $('#spnFromDate').html('Date must be today\'s date or greater than today\'s date.');
                //    $('#spnToDate').html('');
                //    isValid = false;
                //}
                //else {
                $('#spnFromDate').html('');
                $('#spnToDate').html('');

            }
        }
    }
    if ($('#DispensationReferenceNo').length > 0 && $('#DispensationReferenceNo').val() == '') {
        $('#err_drn').html('Dispensation reference no is required.');
        isValid = false;
    }
    if ($('#Summary').length > 0 && $('#Summary').val() == '') {
        $('#err_summary').html('Brief description is required.');
        isValid = false;
    }
    if ($('#Description').length > 0 && $('#Description').val() == '') {
        $('#err_descp').html('Description is required.');
        isValid = false;
    }
    if ($('#GrantedBy').length > 0 && $('#GrantedBy').val() == '') {
        $('#err_GrantedBy').html('Issued by is required.');
        isValid = false;
    }

    return isValid;
}
